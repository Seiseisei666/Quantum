﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quantum_Game.Interfaccia
{
    public class GuiManager : DrawableGameComponent
    {
        const int MAX_ELEMENTI = 10;
        /// <summary> Nel loro Costruttore i GameComponent iscrivono i loro propri servizi ai servizidel gioco.
        /// In questo caso il GuiManager si iscrive in quanto servizio grafico unico di quantum.</summary>
        public GuiManager (Game game) : base(game)
        {
            _game = game;
            elementi = new List<RiquadroGui> (MAX_ELEMENTI);
            // Iscrivo il managerGui ai servizi del gioco
            _game.Services.AddService(this);
        }

        public override void Initialize()
        {
            // Qua ci prendiamo dai servizi del gioco quello che ci interessa
            _mouseInput = _game.Services.GetService<MouseInput>();

            base.Initialize();
        }

        public bottone BottonePremuto { get; private set; }
        //public event EventHandler<EventArgs> BottonePassa;
        //public event EventHandler<EventArgs> BottoneRicerca;
        //public event EventHandler<EventArgs> BottoneColonizza;
        //public event EventHandler<EventArgs> BottoneRiconfigura;
        //public event EventHandler<EventArgs> BottoneUsaSpecial;

        public Texture2D Pennello { get { return _texture; } }
        public Texture2D SpriteSheet { get { return _spriteSheet; } }
        public SpriteFont Font { get { return font; } }
        public Tabellone Tabellone { get { return _tabellone; } }
        public Cimitero Cimitero { get { return _cimitero; } }
        /// <summary>Lista dei Bottoni iscritti alla GUI</summary>
        public Bottone[] Bottoni { get
            {
                Bottone[] bott = new Bottone[0];
                foreach (var e in elementi)
                {
                    var b = e as Bottone;
                    if (b != null)
                        bott = bott.Concat(Enumerable.Repeat(b, 1)).ToArray();
                }
                return bott;
            }

        }
        /// <summary>Iscrive un RiquadroGui all'interfaccia </summary>
        public void Iscrivi(RiquadroGui elemento)
        {
            elemento.Inizializzazione(this);
            elemento.AssociaEventiMouse(_mouseInput);
            elementi.Add(elemento);
        }
        /// <summary>Rimuove un RiquadroGui dall'interfaccia </summary>
        public void Rimuovi(RiquadroGui elemento)
        {
            elemento.DissociaEventiMouse(_mouseInput);
            elementi.Remove(elemento);
        }

        /// <summary> Iscrizione del tabellone al GUI</summary>
        public void Iscrivi(Tabellone tab)
        {
            Iscrivi(tab as RiquadroGui);
            _tabellone = tab;
        }
        public void Iscrivi (Cimitero cim)
        {
            Iscrivi ((RiquadroGui)cim);
            _cimitero = cim;

        }
        /// <summary>Iscrive un Menu a tendina all'interfaccia </summary>
        public void Iscrivi(MenuTendina menu)
        {
            foreach (var voce in menu.Elementi)
            {
                Iscrivi(voce as RiquadroGui);
                voce.Riposiziona(menu.Posizione);
            }
        }
        /// <summary>Rimuove dall'interfaccia tutti i "figli" dell'oggetto argomento </summary>
        public void Rimuovi(object parent)
        {
            var lista = elementi.FindAll(x => x.Parent?.Equals(parent) == true);
            foreach (var e in lista)
                if (e!= null)
                    Rimuovi(e as RiquadroGui);
        }



        #region Override di Game Component
        protected override void LoadContent()
        {
            _spriteSheet = _game.Content.Load<Texture2D>(@"Graphica\TileSet_prova3");
            font = _game.Content.Load<SpriteFont>("Font\\Font");

            _texture = new Texture2D(GraphicsDevice, 1, 1);
            _texture.SetData(new[] { (Color.White) });
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _game.Services.AddService(_spriteBatch);
        }
        /// <summary>Controlla ad ogni frame se è stato premuto un bottone. </summary>
        public override void Update(GameTime gameTime)
        {
            /* TODO:

Il sistema di bottoni attuale è molto basic: in pratica, se in un certo frame è stato premuto un bottone, la proprietà GuiManager.BottonePremuto verrà settata sul tipo di bottone.
Problema: se in un pezzo di codice non controlliamo esplicitamente un dato bottone, questo, anche se viene premuto, non produce risultati.
Ad esempio, se durante un'azione di movimento premo "passa turno" non succede niente, perché quel bottone viene osservato solo durante la fase di attesa della selezione.

L'unica soluzione che mi viene in mente è: sostituire questo meccanismo con uno ad eventi... (work in progress)

            */
            foreach (var elemento in elementi)
            {
                Bottone bott = elemento as Bottone;
                if (bott?.Cliccato == true)
                {
                    bott.Reset();
                    BottonePremuto = bott.TipoBottone;

                    return;
                }
                BottonePremuto = bottone.nessuno;
            }
        }


        public override void Draw(GameTime gameTime)
        {

            foreach (RiquadroGui elemento in elementi)
            {
                elemento.Draw(_spriteBatch);
            }

        }
        #endregion Override di Game Component


        /// <summary>Elimina tutti gli elementi della Gui</summary>
        public void ResettoneGlobale()
        {
            elementi.Clear();
        }

        /// <summary>Libera le risorse dell'oggetto </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var elemento in elementi)
                {
                    elemento.DissociaEventiMouse(_mouseInput);
                    var d = elemento as IDisposable;
                    d?.Dispose();
                }
            ResettoneGlobale();
            }
        }

        // Campi privati
        private List<RiquadroGui> elementi;

        private Game _game;
        private Tabellone _tabellone;
        Cimitero _cimitero;
        private MouseInput _mouseInput;
        private SpriteBatch _spriteBatch;
        private Texture2D _spriteSheet;
        private Texture2D _texture;
        private SpriteFont font;

    }

}