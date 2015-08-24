﻿using System;
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
        public Tabellone(Game game, float xRel, float yRel) : base(game, xRel, yRel, 1f, 0.98f)
        {
            mappa = game.Services.GetService<Mappa>();
            _righe = mappa.Righe; _colonne = mappa.Colonne;

            //Calcolo il lato delle caselle:
            float h = Altezza / (float)_righe; float w = Larghezza / (float)_colonne;
            _latoCasella = (w <= h) ? (int)w : (int)h;
            //rettangoli per lo spritebatch:
            _target = new Rectangle(0, 0, _latoCasella, _latoCasella);
            _source = new Rectangle(0, 0, 100, 100);  // 100 è il lato del nostro tileset di prova!! 
                                                      //va sostituito con quello definitivo

            _IdSelezione = -1;
            _idMouseOver = -1;
            SelezTileVisibile = true;
            Quantum.Ridimensionamento += GestisciRidimensionamento;
        }

        // PROPRIETA' PUBBLICHE

        public bool SelezTileVisibile { set { _selezTileVisibile = value; } }

        public Tile TileClick { get { return (_IdSelezione >= 0) ? mappa.id2Tile(_IdSelezione) : null; } }
        public TipoEventoMouse UltimoClick { get; private set; }

        public int LarghezzaTilePx { get { return _latoCasella; } } // TODO: soluzione poco elegante

        // METODI PUBBLICI
        public void ResetSelezioneMouse()
        {
            // Annulla la selezione del mouse, in modo che una volta finita un'azione
            // non venga automaticamente riselezionata la stessa ultima casella cliccata
            _IdSelezione = -1;
            UltimoClick = TipoEventoMouse.nessuno;
        }

        public Point id2Pixel (int id)
        {
            int n, m;
            mappa.id2nm(id, out n, out m);
            return new Point
                (n * _latoCasella + Offset.X, m * _latoCasella + Offset.Y);
        }
        public Point Tile2Pixel(Tile tile)
        {
            int n, m, id;
            id = mappa.Tile2Id(tile);
            mappa.id2nm(id, out n, out m);
            return new Point
                (n * _latoCasella + Offset.X, m * _latoCasella + Offset.Y);
        }

        // restituisce il pianeta più vicino alla casella argomento
        public Pianeta PianetaPiùVicino(Casella casella)
        {
            int Id, n, m;
            Id = mappa.Tile2Id(casella);
            mappa.id2nm(Id, out n, out m);
            n = (n / 3) * 3; m /= 3 * 3; n++; m++;
            Pianeta tempPlan = mappa.id2Tile(mappa.nm2id(n,m)) as Pianeta;
            return tempPlan;
        }
            // disegna il tabellone e le navi
        public override void Draw(SpriteBatch spriteBatch, Texture2D tileset)
        {
            int x, y;
            for (int Idx = 0; Idx < mappa.NumeroCaselle; Idx++)
            {
                Tile tile = mappa.id2Tile(Idx); 
                if (tile.Esistente)
                {             // se la casella non fa parte del gioco non la disegna

                    //calcolo delle coordinate su cui disegnare:
                    mappa.id2nm(Idx, out x, out y);
                    _target.X = x * _latoCasella + Offset.X;
                    _target.Y = y * _latoCasella + Offset.Y;

                    //calcolo del tipo di tile (semplificato, manca il tileset!!!)
                    switch (tile.Tipo)
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
                    if (tile.EunaCasella)
                    {
                        Casella tempCas = tile as Casella;
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
            if (_idMouseOver >= 0 && _selezTileVisibile)
            {
                _target.X = _SelezPixCoord.X; _target.Y = _SelezPixCoord.Y;
                spriteBatch.Draw(texture, _target, Color.IndianRed*0.5f);
            }
        }
            // Al momento, del tutto inutile. Viene chiamata una volta per partita
        public void InizioPartita(object sender, EventArgs args)
        {
        }


        // METODI PRIVATI
       
        /// <summary>
        /// Converte una coordinata in pixel nell'Id della casella corrispondente
        /// Restituisce false se il click è fuori dai bordi del tabellone
        /// </summary>
        public bool coordinatePixel2Casella(ref int x, ref int y)
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
                    mappa.id2Tile(_idMouseOver = (tempX + tempY * _colonne)).Esistente)
                {
                    mappa.id2nm(_idMouseOver, out tempX, out tempY);
                    _SelezPixCoord.X = tempX*_latoCasella + Offset.X; _SelezPixCoord.Y = tempY*_latoCasella + Offset.Y;
                    return;
                }
            }
            _idMouseOver = -1; // mouse fuori dal tabellone o mouse su casella Vuoto

        }

        
        protected override void ClickSinistro(object sender, MouseEvntArgs args)
        {
            UltimoClick = TipoEventoMouse.ClkSin;
            if (_idMouseOver >= 0)              // se il mouse sta sopra una casella valida prendiamo 
            {
                _IdSelezione = _idMouseOver;
            }
            else
                _IdSelezione = -1;              // sennò annulliamo la selezione attuale
        }

        protected override void ClickDestro(object sender, MouseEvntArgs args)
        {
            UltimoClick = TipoEventoMouse.ClkDx;
            if (_idMouseOver >= 0)              // se il mouse sta sopra una casella valida prendiamo 
            {
                _IdSelezione = _idMouseOver;
            }
            else
                _IdSelezione = -1;              // sennò annulliamo la selezione attuale
        }

        // CAMPI DELLA CLASSE

            // gli elementi costitutivi del tabellone
      //  private static List<Tile> _listaCaselle;
        private int _righe, _colonne;

        private Mappa mappa;

        private int _latoCasella;

            //utility per disegnare
        private Rectangle _source, _target;

        // MEMBRI RELATIVI ALLE SELEZIONI FATTE CON CLICK DEL MOUSE
        bool _selezTileVisibile;
        private int _IdSelezione;
        private int _idMouseOver;
        private Point _SelezPixCoord; //coordinate in pixel della casella selezionata col clk sinistro
    }
 }

