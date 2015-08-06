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
    Come stabilito ieri sera il tabellone è 

    */

namespace Quantum_Game
{
    public class Tabellone : Riquadro
    {
        private List<Tile> _listaCaselle;
        private int _righe, _colonne;
        private int _latoCasella;
        private Rectangle source, target; //per disegnare

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
        public Tabellone(List<Tile> lista, int righe, int colonne, Point TopLeft, int Largh, int Alt) : base(TopLeft, 1f, 1f, Largh, Alt)
        {
            _righe = righe; _colonne = colonne;
            _listaCaselle = lista;

            //Calcolo il lato delle caselle:
            _latoCasella = (_righe >= _colonne) ?
                (int)((float)Superficie.Height / _righe) :
                (int)((float)Superficie.Width / _colonne);
            //rettangoli per lo spritebatch:
            target = new Rectangle(0, 0, _latoCasella, _latoCasella);
            source = new Rectangle(0, 0, 100, 100);  // 100 è il lato del nostro tileset di prova!! 
                                                     //va sostituito con quello definitivo



            Game1.MouseClick += ClickSinistro;
            Game1.Ridimensionamento += GestisciRidimensionamento;
        }
        /// <summary>
        /// Costruttore dell'oggetto Tabellone. 
        /// Il riquardo relativo alla parte grafica è settato automaticamente a tutto schermo
        /// </summary>
        /// <param name="lista">Una lista di caselle (da sin a dx, dall'alto al basso)</param>
        /// <param name="righe">Numero di caselle per colonna</param>
        /// <param name="colonne">Numero di caselle per riga</param>
        public Tabellone(List<Tile> lista, int righe, int colonne) : base()
        {
            _righe = righe; _colonne = colonne;
            _listaCaselle = lista;

            //Calcolo il lato delle caselle:
            _latoCasella = (_righe >= _colonne) ?
                (int)((float)Superficie.Height / _righe) :
                (int)((float)Superficie.Width / _colonne);
            //rettangoli per lo spritebatch:
            target = new Rectangle(0, 0, _latoCasella, _latoCasella);
            source = new Rectangle(0, 0, 100, 100);  // 100 è il lato del nostro tileset di prova!! 
                                                     //va sostituito con quello definitivo


            Game1.MouseClick += ClickSinistro;
            Game1.Ridimensionamento += GestisciRidimensionamento;
        }

        /// <summary>
        /// funzioncina per convertire fra indice dell'array e posizione x,y della casella nel tabellone
        /// </summary>
        private void id2xy (int id, out int x, out int y)
        {
            x = id % _colonne;
            y = (id / _righe);
        }



        public void Draw(SpriteBatch spriteBatch, Texture2D tileset)

        {
            int x, y; 
            for (int Idx = 0; Idx < _listaCaselle.Count; Idx++)
            {
                if (_listaCaselle[Idx].Esistente) {             // se la casella non fa parte del gioco non la disegna
                   
                    //calcolo delle coordinate su cui disegnare:
                    id2xy(Idx, out x, out y);
                    target.X = (x *= _latoCasella + Superficie.X);
                    target.Y = (y *= _latoCasella + Superficie.Y);

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

        }
    }

