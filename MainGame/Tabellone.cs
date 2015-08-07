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
        private List<Tile> _listaCaselle;
        private int _righe, _colonne;
        private int _latoCasella;
        private Rectangle source, target; //per disegnare

        private int? _IdSelezione;
        private Point _SelezPixCoord; //coordinate in pixel della casella selezionata

        public Nave NaveSelezionata { get
                {
                Casella cas = _listaCaselle[_IdSelezione.Value] as Casella;
                if (cas != null)
                    return cas.Occupante;
                else return null;
                }
            } 

        public int Righe { get { return _righe; } }
        public int Colonne { get { return _colonne; } }

        /// <summary>
        /// Costruttore dell'oggetto Tabellone, con impostazioni del riquadro grafico
        /// </summary>
        /// <param name="lista">Una lista di caselle (da sin a dx, dall'alto al basso)</param>
        /// <param name="righe">Numero di caselle per colonna</param>
        /// <param name="colonne">Numero di caselle per riga</param>
        /// <param name="TopLeft">Coordinate in pixel dell'angolo in alto a sinistra</param>
        /// <param name="Largh">Larghezza del riquadro del tabellone in pixel</param>
        /// <param name="Alt">Altezza del riquadro tabellone in pixel</param>
        public Tabellone(List<Tile> lista, int righe, int colonne, float xRel, float yRel, int LarghSchermo, int AltSchermo) : base(xRel,yRel, 1f, 1f, LarghSchermo, AltSchermo)
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

            Game1.MouseClick += ClickSinistro;
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
        /// </summary>
        public void coordinatePixel2Casella(ref int x, ref int y)
        {
            float tempX = x; float tempY = y;
            tempX -= Offset.X; tempY -= Offset.Y;

            x = (int)Math.Floor(tempX / _latoCasella);
            y = (int)Math.Floor(tempY / _latoCasella);
            return;
        }

        protected override void ClickSinistro (object sender, MouseEvntArgs args)
        {
            int tempX = args.Posizione.X;
            int tempY = args.Posizione.Y;

            if (Compreso(tempX, tempY)) {       // Click sul tabellone
                coordinatePixel2Casella(ref tempX, ref tempY);
                int tempID = tempX + tempY * _colonne;
             
                if (tempID >= 0)                // la condizione dovrebbe essere sempre soddisfatta in realtà
                {
                    _IdSelezione = tempID;
                    id2xy(_IdSelezione.Value, out tempX, out tempY);
                    _SelezPixCoord.X = tempX + Offset.X; _SelezPixCoord.Y = tempY +Offset.Y;
                }
                else
                    _IdSelezione = null;
                Debug.WriteLine(_IdSelezione.Value);
                Debug.WriteLine(_latoCasella);



            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D tileset)

        {
            int x, y; 
            for (int Idx = 0; Idx < _listaCaselle.Count; Idx++)
            {
                if (_listaCaselle[Idx].Esistente) {             // se la casella non fa parte del gioco non la disegna
                   
                    //calcolo delle coordinate su cui disegnare:
                    id2xy(Idx, out x, out y);
                    target.X = x  + Offset.X;
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
                    }
                }
            }

        public void DisegnaSelezione (SpriteBatch spriteBatch, Texture2D texture)
        {
            if (_IdSelezione != null)
            {
                target.X = _SelezPixCoord.X; target.Y = _SelezPixCoord.Y; 
                spriteBatch.Draw(texture,target, Color.Beige);

            }
        }

        }
    }

