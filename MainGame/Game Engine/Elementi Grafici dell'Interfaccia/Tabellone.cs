﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Quantum_Game.Interfaccia;

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
    public class Tabellone : RiquadroGui, IGameComponent
    {
        /// <summary>
        /// Costruttore dell'oggetto Tabellone, con impostazioni del riquadro grafico
        /// </summary>
        public Tabellone(Game game, int xRel, int yRel, int larghRel, int altRel) : 
            base(xRel, yRel, larghRel, altRel, game)
        {
            _game = game;
            _IdSelezione = -1;
            _idMouseOver = -1;
            _coordIlluminazione = new Point[0];
        }

        public void Initialize()
        {
            // Iscrizione al Gui
            GuiManager gui = _game.Services.GetService<GuiManager>();
            gui.Iscrivi(this);
        }

        public override void Inizializzazione(GuiManager gui)
        {
            // Questa inizializzazione è chiamata dal gui;
            // serve per regolare le misure del riquadro rispetto alle dimensioni dello schermo
            // e per prendere i riferimenti del tileset
            tileset = gui.SpriteSheet;
            pennello = gui.Pennello;
            font = gui.Font;

            base.Inizializzazione(gui);


            //Calcolo il lato delle caselle:
            float h = Altezza / (float)Tile.Righe; float w = Larghezza / (float)Tile.Colonne;
            _latoCasella = (w <= h) ? (int)w : (int)h;

            //rettangoli per lo spritebatch:
            _target = new Rectangle(0, 0, _latoCasella, _latoCasella);
            _source = new Rectangle(0, 0, 100, 100);  // TODO: 100 è il lato del nostro tileset di prova!! 
        }

        // PROPRIETA' PUBBLICHE
        /// <summary>Restituisce l'ultimo tile su cui si è cliccato, o null se il click non era valido o la selezione è stata resettata</summary>
        public Tile TileClick { get { return (_IdSelezione >= 0) ? Tile.id2Tile(_IdSelezione) : null; } }
        /// <summary>Restituisce l'ultimo evento del mouse: click sinistro, click destro o nessuno (se la selezione è stata resettata)</summary>
        public TipoEventoMouse UltimoClick { get; private set; }

        // METODI PUBBLICI

        /// <summary>Annulla la selezione del mouse, in modo che una volta finita un'azione 
        /// non venga automaticamente riselezionata la stessa ultima casella cliccata</summary>
        public void ResetSelezioneMouse()
        {
            _IdSelezione = -1;
            UltimoClick = TipoEventoMouse.nessuno;
        }

        #region Metodi di Disegno
        // disegna il tabellone e le navi
        public override void Draw(SpriteBatch spriteBatch)
        {

            int x, y;
            for (int Idx = 0; Idx < Tile.Righe*Tile.Colonne; Idx++)
            {
                Tile tile = Tile.id2Tile(Idx); 
                if (tile.Esistente)
                {             // se la casella non fa parte del gioco non la disegna

                    //calcolo delle coordinate su cui disegnare:
                    id2nm(Idx, out x, out y);
                    _target.X = x * _latoCasella + Posizione.X;
                    _target.Y = y * _latoCasella + Posizione.Y;

                    //calcolo del tipo di tile (semplificato, manca il tileset!!!)
                    // TODO: qua è ancora tutto provvisorio
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
                        case QuantumTile.Pianeta7:
                            _source.X = 0;
                            _source.Y = 100;
                            break;
                        case QuantumTile.Pianeta8:
                            _source.X = 100;
                            _source.Y = 100;
                            break;
                        case QuantumTile.Pianeta9:
                            _source.X = 200;
                            _source.Y = 100;
                            break;
                        case QuantumTile.Pianeta10:
                            _source.X = 300;
                            _source.Y = 100;
                            break;
                    }

                    // l'istruzione draw vera e propria
                    spriteBatch.Draw(tileset, _target, _source, Color.White);
                    if (tile.EunaCasella)
                    {
                        Casella tempCas = (Casella)tile;
                        if (tempCas.Occupante != null)
                        {
                            _source.X = 300;
                            spriteBatch.Draw(tileset, _target, _source, tempCas.Occupante.SpriteColor);
                            spriteBatch.DrawString(font, tempCas.Occupante.Tipo.ToString(), new Vector2(_target.X, _target.Y + _latoCasella*0.75f), Color.White);
                        }
                    }
                }
            }

            DisegnaSelezione(spriteBatch);

            disegnaIlluminaCaselle(spriteBatch);
        }
            // illumina la casella su cui sta il mouse
        void DisegnaSelezione(SpriteBatch spriteBatch)
        {
            if (_idMouseOver >= 0)
            {
                _target.X = _SelezPixCoord.X; _target.Y = _SelezPixCoord.Y;
                spriteBatch.Draw(pennello, _target, Color.IndianRed*0.5f);
            }
        }

        void disegnaIlluminaCaselle (SpriteBatch spriteBatch)
        {
            if (_coordIlluminazione.Any())
            { 
            Color colore = Color.LightGreen;
            foreach (var p in _coordIlluminazione)
                {
                    spriteBatch.Draw
                        (pennello, new Rectangle(p.X, p.Y, _latoCasella, _latoCasella), colore * 0.5f);
                }
            }
        }

        #endregion

        /// <summary>Definisce le caselle che devono essere illuminate sul tabellone.
        /// null o array vuoto "spegne" l'illuminazione</summary>
        public void IlluminaCaselle (int[] idCaselle)
        {
                if (idCaselle == null || !idCaselle.Any())
                    _coordIlluminazione = new Point[0];
                else
                {
                    _coordIlluminazione = new Point[idCaselle.Length];
                    int c = 0;
                    foreach (var id in idCaselle)
                        _coordIlluminazione[c++] = id2Pixel(id);
                }
        }
        /// <summary>Spegne l'illuminazione delle caselle.</summary>
        public void SpegniCaselle()
        {
            _coordIlluminazione = new Point[0];
        }

        // METODI PRIVATI

        /// <summary>
        /// Converte una coordinata in pixel nell'Id della casella corrispondente
        /// Restituisce false se il click è fuori dai bordi del tabellone
        /// </summary>
        private bool coordinatePixel2Casella(ref int x, ref int y)
        {
            float tempX = x; float tempY = y;
            tempX -= Posizione.X; tempY -= Posizione.Y;
            x = (int)Math.Floor(tempX / _latoCasella);
            y = (int)Math.Floor(tempY / _latoCasella);
            if (x < 0 || x > Tile.Colonne - 1 || y < 0 || y > Tile.Righe - 1)
                return false;
            return true;
        }
        private Point id2Pixel(int id)
        {
            int n, m;
            id2nm(id, out n, out m);
            return new Point
                (n * _latoCasella + Posizione.X, m * _latoCasella + Posizione.Y);
        }
        public Point Tile2Pixel(Tile tile)
        {
            return id2Pixel(tile.ID);
        }

        #region Input Mouse
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
                    Tile.id2Tile(_idMouseOver = (tempX + tempY * Tile.Colonne)).Esistente)
                {
                    id2nm(_idMouseOver, out tempX, out tempY);
                    _SelezPixCoord.X = tempX*_latoCasella + Posizione.X; _SelezPixCoord.Y = tempY*_latoCasella + Posizione.Y;
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
        #endregion
        // CAMPI DELLA CLASSE
        void id2nm(int idCasella, out int n, out int m)
        {
            if (!Tile.idValido(idCasella))
                throw new IndexOutOfRangeException("Indice non esistente");

            n = idCasella % Tile.Colonne;
            m = idCasella / Tile.Colonne;
        }

        private Game _game;

        // Per disegnare
        private int _latoCasella;
        private Point[] _coordIlluminazione; // Coordinate delle caselle da illuminare
        private Texture2D tileset;
        private Texture2D pennello;
        private Rectangle _source, _target;
        private SpriteFont font;

        // MEMBRI RELATIVI ALLE SELEZIONI FATTE CON CLICK DEL MOUSE
        private int _IdSelezione;
        private int _idMouseOver;
        private Point _SelezPixCoord; //coordinate in pixel della casella selezionata col clk sinistro
    }
}
