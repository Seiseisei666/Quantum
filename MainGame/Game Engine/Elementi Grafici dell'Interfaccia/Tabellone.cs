using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Quantum_Game.Interfaccia;

namespace Quantum_Game
{   
    public class Tabellone: ElementoGrafico,IElementoAnimato
    {
        readonly Pianeta[] pianeti;
        readonly Casella[] caselle;
        readonly Vector2 scala;

        public Tabellone (Game game, Riquadro contenitore): base (contenitore)
        {
            _IdSelezione = -1;
            _idMouseOver = -1;
            _coordIlluminazione = new Vector2[0];
            MostraSelezione = true;

            pianeti = Tile.Tiles(t => t is Pianeta).Select (p => (Pianeta) p ).ToArray();
            caselle = Tile.Tiles(t => t.EunaCasella).Select(c => (Casella) c ).ToArray();

            //Calcolo il lato delle caselle:
            float h = contenitore.Superficie.Height / (float)Tile.Righe; float w = contenitore.Superficie.Width / (float)Tile.Colonne;
            _latoCasella = (w <= h) ? (int)w : (int)h;
            //Calcolo la scala:
            float s = _latoCasella / 100f;
            scala = new Vector2(s, s);

            navi = new ManagerNavi(Tile2Pixel, scala);
        }

        public override void CaricaContenuti(GuiManager gui)
        {
            // Questa inizializzazione è chiamata dal gui;
            // serve per prendere i riferimenti del tileset
            tileset = gui.SpriteSheet;
            pennello = gui.Pennello;
            font = gui.Font;

            // Calcolo l'offset rispetto al riquadro, in modo da posizionare il tabellone esattamente al centro
            offset = new Vector2(
                (contenitore.Superficie.Width - (_latoCasella * Tile.Colonne)) / 2,
                (contenitore.Superficie.Height - (_latoCasella * Tile.Righe) ) / 2
                );

            //rettangoli per lo spritebatch:
            _target = new Rectangle(0, 0, _latoCasella, _latoCasella);
            _source = new Rectangle(0, 0, 100, 100);  // TODO: 100 è il lato del nostro tileset di prova!! 
        }

        // PROPRIETA' PUBBLICHE
        /// <summary>Restituisce l'ultimo tile su cui si è cliccato, o null se il click non era valido o la selezione è stata resettata</summary>
        public Tile TileClick { get { return (_IdSelezione >= 0) ? Tile.id2Tile(_IdSelezione) : null; } }
        /// <summary>Restituisce l'ultimo evento del mouse: click sinistro, click destro o nessuno (se la selezione è stata resettata)</summary>
        public TipoEventoMouse UltimoClick { get; private set; }
        /// <summary> Restituisce il lato in pixel della casella </summary>
        public int LatoCasella { get { return _latoCasella; } }

        // METODI PUBBLICI

        /// <summary>Annulla la selezione del mouse, in modo che una volta finita un'azione 
        /// non venga automaticamente riselezionata la stessa ultima casella cliccata</summary>
        public void ResetSelezioneMouse()
        {
            _IdSelezione = -1;
            UltimoClick = TipoEventoMouse.nessuno;
            MostraSelezione = true;
        }

        public bool MostraSelezione { get; set; }

        #region Metodi di Disegno
        // disegna il tabellone e le navi
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 coordDraw;
            Point coordTile;
            Vector2 scalaPianeti = scala * 2f;

            // Risistemare

            Vector2 ofst = new Vector2 (_latoCasella/2, _latoCasella/2) - scalaPianeti*50;

            foreach (var pianeta in pianeti)
            {
                coordDraw = Tile2Pixel(pianeta) + ofst;

                switch (pianeta.Tipo)
                {
                    case QuantumTile.Pianeta7:
                        coordTile = new Point(0, 100);
                        break;
                    case QuantumTile.Pianeta8:
                        coordTile = new Point(100, 100);
                        break;
                    case QuantumTile.Pianeta9:
                        coordTile = new Point(200, 100);
                        break;
                    default:
                        coordTile = new Point(300, 100);
                        break;
                }

                spriteBatch.Draw(tileset, position: coordDraw, sourceRectangle: new Rectangle (coordTile, new Point (100,100)), scale: scalaPianeti, color: Color.Gray*0.8f);
            }

            foreach (var casella in caselle)
            {
                if (casella == null)
                {
                    Debug.WriteLine("CASELLA NULLA!!!");
                    continue;
                }

                coordDraw = Tile2Pixel(casella);

                if (casella.Orbita)
                    coordTile = new Point(100, 0);
                else
                    coordTile = Point.Zero;

                spriteBatch.Draw(tileset, position: coordDraw, sourceRectangle: new Rectangle(coordTile, new Point(100, 100)), scale: scala, color: Color.White*0.7f);
            }

            DisegnaSelezione(spriteBatch);

            disegnaIlluminaCaselle(spriteBatch);

            navi.Draw(spriteBatch, tileset);
        }


            // illumina la casella su cui sta il mouse
        void DisegnaSelezione(SpriteBatch spriteBatch)
        {
            if (_idMouseOver >= 0 && MostraSelezione)
            {
                _target.X = _SelezPixCoord.X; _target.Y = _SelezPixCoord.Y;
                spriteBatch.Draw(pennello, _target, Color.IndianRed*0.5f);
            }
        }

        void disegnaIlluminaCaselle (SpriteBatch spriteBatch)
        {
            if (_coordIlluminazione.Any())
            { 
            Color colore = Color.LightGoldenrodYellow;
            foreach (var p in _coordIlluminazione)
                {
                    spriteBatch.Draw
                        (pennello, p, scale: new Vector2(_latoCasella,_latoCasella), color: colore * 0.3f);
                }
            }
        }

        #endregion

        /// <summary>Definisce le caselle che devono essere illuminate sul tabellone.
        /// null o array vuoto "spegne" l'illuminazione</summary>
        public void IlluminaCaselle (int[] idCaselle)
        {
                if (idCaselle == null || !idCaselle.Any())
                    _coordIlluminazione = new Vector2[0];
                else
                {
                    _coordIlluminazione = new Vector2[idCaselle.Length];
                    int c = 0;
                    foreach (var id in idCaselle)
                        _coordIlluminazione[c++] = id2Pixel(id);
                }
        }
        /// <summary>Spegne l'illuminazione delle caselle.</summary>
        public void SpegniCaselle()
        {
            _coordIlluminazione = new Vector2[0];
        }
        public void AggiungiNave(Nave nave) { navi.Aggiungi(nave); }


        // METODI PRIVATI

        /// <summary>
        /// Converte una coordinata in pixel nell'Id della casella corrispondente
        /// Restituisce false se il click è fuori dai bordi del tabellone
        /// </summary>
        private bool coordinatePixel2Casella(ref int x, ref int y)
        {
            float tempX = x; float tempY = y;
            tempX -= (contenitore.Superficie.Location.X+offset.X); tempY -= (contenitore.Superficie.Location.Y + offset.Y);
            x = (int)Math.Floor(tempX / _latoCasella);
            y = (int)Math.Floor(tempY / _latoCasella);
            if (x < 0 || x > Tile.Colonne - 1 || y < 0 || y > Tile.Righe - 1)
                return false;
            return true;
        }
        private Vector2 id2Pixel(int id)
        {
            int n, m;
            Tile.id2nm(id, out n, out m);
            return new Vector2
                (n * _latoCasella + contenitore.Superficie.Location.X + offset.X, m * _latoCasella + contenitore.Superficie.Location.Y + offset.Y);
        }
      
        public Vector2 Tile2Pixel (Tile tile)
        {
            return id2Pixel(tile.ID);
        }

        #region Input Mouse
        // PROTECTED OVERRIDE DI "RIQUADRO"
        // relativi all'input del mouse
        // calcola il tile su cui sta il mouse
        protected override void MouseOver(object sender, MouseEvntArgs args)
        {
            if (contenitore.Superficie.Contains(args.Posizione.X, args.Posizione.Y))
            {
                int X = args.Posizione.X;
                int Y = args.Posizione.Y;

                if (coordinatePixel2Casella(ref X, ref Y) &&
                    Tile.id2Tile(_idMouseOver = (X + Y * Tile.Colonne)).EunaCasella)
                {
                    Tile.id2nm(_idMouseOver, out X, out Y);
                    _SelezPixCoord.X = (int) (X*_latoCasella + contenitore.Superficie.Location.X + offset.X);
                    _SelezPixCoord.Y = (int) (Y*_latoCasella + contenitore.Superficie.Location.Y + offset.Y);
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

        public void Update()
        {
            navi.Update();
        }

        #endregion
        // CAMPI DELLA CLASSE

        Vector2 offset;
        public Vector2 Posizione { get { return id2Pixel(0); } }

        // Per disegnare
        readonly int _latoCasella;
        private Vector2[] _coordIlluminazione; // Coordinate delle caselle da illuminare
        private Texture2D tileset;
        private Texture2D pennello;
        private Rectangle _source, _target;
        private SpriteFont font;
        private ManagerNavi navi;

        // MEMBRI RELATIVI ALLE SELEZIONI FATTE CON CLICK DEL MOUSE
        private int _IdSelezione;
        private int _idMouseOver;
        private Point _SelezPixCoord; //coordinate in pixel della casella selezionata col clk sinistro
    }
}

