using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

/// <summary>
/// Il tabellone in verità non esiste. 
/// E' formato da una serie di quadranti che vengono identificati con la posizione (ID_riga,ID_colonna) del centro del quadrante stesso.
/// Se ad esempio dico di disegnare il quadrante (4,4) la funzione provvederà a creare tutte le caselle che circondano quella posizione, 
/// oltre ovviamente alla casella stessa del centro.
/// In base alla posizione (ID_riga,ID_colonna) è in grado di capire quale texture usare.
/// 
/// ps: I numeriMagici = { 1, 4, 7, 10, 13, 16, 19 } sono le righe/colonne che contengono i pianeti (e quindi il centro dei quadranti)
/// in un tabellone con dimensione massima 7x7 quadranti (21x21 caselle).
/// </summary>
/*
    MODIFICA BY EMA
    Come stabilito ieri sera il tabellone è costituito da una lista di caselle, 
    generata dall'oggetto MapGenerator
    Le caselle possono essere vuote (non disegnate), pianeti, oppure caselle normali
    in questo caso mantengono un riferimento ad un oggetto nave (il campo .Occupante).

    */

namespace Quantum_Game
{
    public class Tabellone : Riquadro
    {
        // COSTRUTTORE
        /// <summary>
        /// Costruttore dell'oggetto Tabellone, con impostazioni del riquadro grafico
        /// </summary>
        /// <param name="lista">Una lista di caselle (da sin a dx, dall'alto al basso)</param>
        /// <param name="righe">Numero di caselle per colonna</param>
        /// <param name="colonne">Numero di caselle per riga</param>
        /// <param name="TopLeft">Coordinate in pixel dell'angolo in alto a sinistra</param>
        /// <param name="Largh">Larghezza del riquadro del tabellone in pixel</param>
        /// <param name="Alt">Altezza del riquadro tabellone in pixel</param>
        public Tabellone(List<Tile> lista, int righe, int colonne, float xRel, float yRel, int LarghSchermo, int AltSchermo) : base(xRel, yRel, 1f, 0.98f, LarghSchermo, AltSchermo)
        {
            _righe = righe; _colonne = colonne;
            _listaCaselle = lista;

            //Calcolo il lato delle caselle:
            float h = Altezza / (float)_righe; float w = Larghezza / (float)_colonne;
            _latoCasella = (w <= h) ? (int)w : (int)h;
            //rettangoli per lo spritebatch:
            _target = new Rectangle(0, 0, _latoCasella, _latoCasella);
            _source = new Rectangle(0, 0, 100, 100);  // 100 è il lato del nostro tileset di prova!! 
                                                      //va sostituito con quello definitivo

            _IdSelezione = -1;
            _idSelezioneDestro = -1;
            _idMouseOver = -1;
            _partitaIniziata = false;

            Quantum.Ridimensionamento += GestisciRidimensionamento;
        }

        // PROPRIETA' PUBBLICHE
        public Tile Id2Tile(int ID) { return _listaCaselle[ID]; }
        public int Tile2Id(Tile t) { return _listaCaselle.FindIndex(x => x.Equals(t)); }

        public int LarghezzaTilePx { get { return _latoCasella; } }

        public int IdMouseOver { get { return _idMouseOver; } }
        public Tile TileClkSn { get { return _tileClkSn; } }
        public Tile TileClkDx { get { return _tileClkDx; } }

        // METODI PUBBLICI

            // restituisce il pianeta più vicino alla casella argomento
        public Pianeta PianetaPiùVicino(Casella casella)
        {
            int Id, n, m;
            Id = _listaCaselle.FindIndex(c => c.Equals(casella));
            id2nm(Id, out n, out m);
            n = (n / 3) * 3; m /= 3 * 3;
            Pianeta tempPlan = _listaCaselle[nm2id(n + 1, m + 1)] as Pianeta;
            return tempPlan;
        }
            // disegna il tabellone e le navi
        public override void Draw(SpriteBatch spriteBatch, Texture2D tileset)
        {
            int x, y;
            for (int Idx = 0; Idx < _listaCaselle.Count; Idx++)
            {
                if (_listaCaselle[Idx].Esistente)
                {             // se la casella non fa parte del gioco non la disegna

                    //calcolo delle coordinate su cui disegnare:
                    id2xy(Idx, out x, out y);
                    _target.X = x + Offset.X;
                    _target.Y = y + Offset.Y;

                    //calcolo del tipo di tile (semplificato, manca il tileset!!!)
                    switch (_listaCaselle[Idx].Tipo)
                    {

                        case QuantumTile.casella:
                            _source.X = 0;
                            _source.Y = 0;
                            break;
                        case QuantumTile.orbita:
                            _source.X = 100;
                            _source.Y = 0;
                            break;
                        default:
                            _source.X = 200;
                            _source.Y = 0;
                            break;
                    }

                    // l'istruzione draw vera e propria
                    spriteBatch.Draw(tileset, _target, _source, Color.White);
                    if (_listaCaselle[Idx].EunaCasella)
                    {
                        Casella tempCas = _listaCaselle[Idx] as Casella;
                        if (tempCas.Occupante != null)
                        {
                            _source.X = 300;
                            spriteBatch.Draw(tileset, _target, _source, tempCas.Occupante.SpriteColor);
                        }
                    }
                }
            }
        }
            // illumina la casella su cui sta il mouse
        public void DisegnaSelezione(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (_idMouseOver >= 0 && _partitaIniziata)
            {
                _target.X = _SelezPixCoord.X; _target.Y = _SelezPixCoord.Y;
                spriteBatch.Draw(texture, _target, Color.IndianRed);
            }
        }
            // Al momento, del tutto inutile. Viene chiamata una volta per partita
        public void InizioPartita(object sender, EventArgs args)
        {
            _partitaIniziata = true;
        }



        // PROPRIETA' PRIVATE

            // restituisce il Tile su cui si è cliccato col sinistro
        private Tile _tileClkSn { get { return (_IdSelezione < 0) ? null : _listaCaselle[_IdSelezione]; } }
            //stessa cosa col click destro
        private Tile _tileClkDx { get { return (_idSelezioneDestro < 0) ? null : _listaCaselle[_idSelezioneDestro]; } }

        // METODI PRIVATI 
            //Funzioncine private di conversione fra le coordinate id ad una dimensione
            // e le coordinate x,y (pixel) oppure n, m
        private void id2xy(int id, out int x, out int y)
        {
            x = ((id % _colonne) * _latoCasella);
            y = (id / _colonne) * _latoCasella;
        }
        private void id2nm(int id, out int n, out int m)
        {
            n = id % _colonne; m = id / _colonne;
        }
        private int nm2id(int n, int m)
        {
            return n + m * _colonne;
        }
        /// <summary>
        /// Converte una coordinata in pixel nell'Id della casella corrispondente
        /// Restituisce false se il click è fuori dai bordi del tabellone
        /// </summary>
        private bool coordinatePixel2Casella(ref int x, ref int y)
        {
            float tempX = x; float tempY = y;
            tempX -= Offset.X; tempY -= Offset.Y;
            x = (int)Math.Floor(tempX / _latoCasella);
            y = (int)Math.Floor(tempY / _latoCasella);
            if (x < 0 || x > _colonne - 1 || y < 0 || y > _righe - 1)
                return false;
            return true;
        }

        // PROTECTED OVERRIDE DI "RIQUADRO"
        // relativi all'input del mouse
            // calcola il tile su cui sta il mouse
        protected override void MouseOver(object sender, MouseEvntArgs args)
        {
            if (Compreso(args.Posizione.X, args.Posizione.Y))
            {
                int tempX = args.Posizione.X;
                int tempY = args.Posizione.Y;

                if (coordinatePixel2Casella(ref tempX, ref tempY) &&
                    _listaCaselle[_idMouseOver = (tempX + tempY * _colonne)].Esistente)
                {
                    id2xy(_idMouseOver, out tempX, out tempY);
                    _SelezPixCoord.X = tempX + Offset.X; _SelezPixCoord.Y = tempY + Offset.Y;
                    return;
                }
            }
            _idMouseOver = -1; // mouse fuori dal tabellone o mouse su casella Vuoto

        }

        
        protected override void ClickSinistro(object sender, MouseEvntArgs args)
        {
            _idSelezioneDestro = -1;      // annulliamo la selezione destra per prima cosa
            if (_idMouseOver >= 0)              // se il mouse sta sopra una casella valida prendiamo 
                _IdSelezione = _idMouseOver;
            else
                _IdSelezione = -1;              // sennò annulliamo la selezione attuale
        }

        protected override void ClickDestro(object sender, MouseEvntArgs args)
        {

            if (_idMouseOver >= 0)              // se il mouse sta sopra una casella valida prendiamo 
            {
                _idSelezioneDestro = _idMouseOver;
                _IdSelezione = -1;
            }
            else
                _idSelezioneDestro = -1;              // sennò annulliamo la selezione attuale
        }

        // CAMPI DELLA CLASSE

            // gli elementi costitutivi del tabellone
        private static List<Tile> _listaCaselle;
        private int _righe, _colonne;
        private int _latoCasella;

            //utility per disegnare
        private Rectangle _source, _target;

            // MEMBRI RELATIVI ALLE SELEZIONI FATTE CON CLICK DEL MOUSE
        private int _IdSelezione;
        private int _idSelezioneDestro;
        private int _idMouseOver;
        private Point _SelezPixCoord; //coordinate in pixel della casella selezionata col clk sinistro
       
        //inutile?
        private bool _partitaIniziata;
    }
 }

