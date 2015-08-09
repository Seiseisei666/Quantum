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
        // gli elementi costitutivi del tabellone
        private List<Tile> _listaCaselle;
        private int _righe, _colonne;
        private int _latoCasella;

        //utility per disegnare
        private Rectangle source, target;

        // MEMBRI RELATIVI ALLE SELEZIONI FATTE CON CLICK DEL MOUSE
        private int? _IdSelezione;
        private int? _idSelezioneDestro;
        private Point _SelezPixCoord; //coordinate in pixel della casella selezionata col clk sinistro
        public void Deseleziona(bool sinistro = true) //metodo per annullare la selezione
        {
            if (sinistro)
                _IdSelezione = null;
            else _idSelezioneDestro = null;
        } //
        // restituisce il Tile su cui si è cliccato col sinistro
        private Tile _tileSelezionato { get { return (_IdSelezione == null) ? null : _listaCaselle[_IdSelezione.Value]; } }
        public Tile TileSelezionato { get { return _tileSelezionato; }   }
        //stessa cosa col click destro
        private Tile _tileTarget { get { return (_idSelezioneDestro == null) ? null : _listaCaselle[_idSelezioneDestro.Value]; } }
        public Tile TileTarget { get { return _tileTarget; } }
        //restituisce la nave selezionata
        public Nave NaveSelezionata
        {
            get
            {
                Casella cas = _tileSelezionato as Casella;
                if (cas != null)
                    return cas.Occupante;
                else return null;
            }
        }

        //inutili?
        private bool _partitaIniziata; //inutili

        /// <summary>
        /// Costruttore dell'oggetto Tabellone, con impostazioni del riquadro grafico
        /// </summary>
        /// <param name="lista">Una lista di caselle (da sin a dx, dall'alto al basso)</param>
        /// <param name="righe">Numero di caselle per colonna</param>
        /// <param name="colonne">Numero di caselle per riga</param>
        /// <param name="TopLeft">Coordinate in pixel dell'angolo in alto a sinistra</param>
        /// <param name="Largh">Larghezza del riquadro del tabellone in pixel</param>
        /// <param name="Alt">Altezza del riquadro tabellone in pixel</param>
        public Tabellone(List<Tile> lista, int righe, int colonne, float xRel, float yRel, int LarghSchermo, int AltSchermo) : base(xRel,yRel, 1f, 0.98f, LarghSchermo, AltSchermo)
        {
            _righe = righe; _colonne = colonne;
            _listaCaselle = lista;

            //Calcolo il lato delle caselle:
            float h = Altezza / (float)_righe; float w = Larghezza / (float)_colonne;
            _latoCasella =  (w <= h) ? (int)w : (int)h;
            //rettangoli per lo spritebatch:
            target = new Rectangle(0, 0, _latoCasella, _latoCasella);
            source = new Rectangle(0, 0, 100, 100);  // 100 è il lato del nostro tileset di prova!! 
                                                     //va sostituito con quello definitivo

            _IdSelezione = null;
            _idSelezioneDestro = null;
            _partitaIniziata = false;

            Game1.Ridimensionamento += GestisciRidimensionamento;
        }
       
      
        /// <summary>
        /// funzioncina per convertire fra indice dell'array e posizione x,y della casella nel tabellone
        /// (angolo sup sin)
        /// </summary>
        private void id2xy (int id, out int x, out int y)
        {
            x = ((id % _colonne) * _latoCasella);
            y = (id / _colonne) * _latoCasella;
            
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

        
        /// <summary>
        /// Gestisce il click sinistro, selezionando la casella corrispondente
        /// </summary>
        /// <param name="sender">ignorato</param>
        /// <param name="args">status del mouse (eg posizione)</param>
        protected override void ClickSinistro (object sender, MouseEvntArgs args)
        {
            _idSelezioneDestro = null;
            if (Compreso(args.Posizione.X, args.Posizione.Y)) {
                int tempX = args.Posizione.X;
                int tempY = args.Posizione.Y;

                if (coordinatePixel2Casella(ref tempX, ref tempY))
                {
                    int tempID = tempX + tempY * _colonne;  // calcolo l'ID della casella selezionata

                    if (_listaCaselle[tempID].Esistente) // la casella esiste davvero
                    {
                        _IdSelezione = tempID;
                        id2xy(_IdSelezione.Value, out tempX, out tempY);
                        _SelezPixCoord.X = tempX + Offset.X; _SelezPixCoord.Y = tempY + Offset.Y;
                        return;
                    }
                }

                _IdSelezione = null;

                }
        }

        protected override void ClickDestro(object sender, MouseEvntArgs args)
        {
            if (Compreso(args.Posizione.X, args.Posizione.Y))
            {
                int tempX = args.Posizione.X;
                int tempY = args.Posizione.Y;

                if (coordinatePixel2Casella(ref tempX, ref tempY))
                {
                    int tempID = tempX + tempY * _colonne;  // calcolo l'ID della casella selezionata

                    if (_listaCaselle[tempID].Esistente) // la casella esiste davvero
                    {
                        _idSelezioneDestro = tempID;
                        return;
                    }
                }

                _idSelezioneDestro = null;

            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D tileset)

        {
            int x, y;
            for (int Idx = 0; Idx < _listaCaselle.Count; Idx++)
            {
                if (_listaCaselle[Idx].Esistente)
                {             // se la casella non fa parte del gioco non la disegna

                    //calcolo delle coordinate su cui disegnare:
                    id2xy(Idx, out x, out y);
                    target.X = x + Offset.X;
                    target.Y = y + Offset.Y;

                    //calcolo del tipo di tile (semplificato, manca il tileset!!!)
                    switch (_listaCaselle[Idx].Tipo)
                    {

                        case QuantumTile.casella:
                            source.X = 0;
                            source.Y = 0;
                            break;
                        case QuantumTile.orbita:
                            source.X = 100;
                            source.Y = 0;
                            break;
                        default:
                            source.X = 200;
                            source.Y = 0;
                            break;
                    }

                    // l'istruzione draw vera e propria
                    spriteBatch.Draw(tileset, target, source, Color.White);
                    if (_listaCaselle[Idx].EunaCasella)
                    {
                        Casella tempCas = _listaCaselle[Idx] as Casella;
                        if (tempCas.Occupante != null)
                        {
                            source.X = 300;
                            spriteBatch.Draw(tileset, target, source, tempCas.Occupante.SpriteColor);
                        }
                    }
                }
            }
        }

        public void DisegnaSelezione (SpriteBatch spriteBatch, Texture2D texture)
        {
            if (_IdSelezione != null && _partitaIniziata)
            {
                target.X = _SelezPixCoord.X; target.Y = _SelezPixCoord.Y; 
                spriteBatch.Draw (texture,target, Color.IndianRed);
            }
        }
        public void InizioPartita (object sender, EventArgs args)
        {
            _partitaIniziata = true;
        }

        public void SpostaNaveSelezionata ()
        {
            Casella cas = TileSelezionato as Casella;
            Casella destinazione = TileTarget as Casella;
            if (NaveSelezionata != null && cas != null)
            {
                destinazione.Occupante = NaveSelezionata;
                cas.Occupante = null;
            }
            else throw new Exception("La selezione non era valida! Movimento non effettuato!");

            Deseleziona(); // dopo aver effettuato il movimento, deseleziona automaticamente
        }


        }
    }

