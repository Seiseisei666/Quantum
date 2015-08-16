using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Quantum_Game
{
    public sealed class GUI
    {
        // Roba per implementazione Singleton
        // Può esistere un solo oggetto GUI: GUI.Istanza
        private bool _inizializzato;

        private static readonly GUI _istanza = new GUI();
        public static GUI Istanza { get { return _istanza; } }

        private GUI() { _inizializzato = false; }

        public void Init(Game game) //inizializzazione, da chiamare all'inizio
        {
            if (graphicsDevice != null)
            {
                this.game = game;
                _elementi = new List<Riquadro>();
                graphicsDevice = (GraphicsDevice) game.Services.GetService(typeof(GraphicsDevice));
            }
        }


        // Proprietà pubbliche
        public bottone BottonePremuto   // Da controllare ogni update, fornisce il bottone che è stato premuto
        {
            get
            {
                foreach (IBottone bot in _elementi)
                {
                    if (bot.Check)
                        return bot.TipoBottone;
                }
                return bottone.nessuno;
            }
        }
        public Tabellone tabellone
        {
            get
            {
                return _elementi.Find(tab => tab.GetType() == typeof(Tabellone)) as Tabellone;
            }
        }

        // METODI IMPORTANTI
        public void Draw ()
        {
            foreach (Riquadro r in _elementi)
                r.Draw(spriteBatch, texture);
        }

       


        // Metodi scemi

        public void AddElement (Riquadro riquadro)
        {
            _elementi.Add(riquadro);
        }

        // Campi privati
        private List<Riquadro> _elementi;
        private Game game;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;
        private Texture2D texture;

        // Proprietà private
        private int _numElementi { get { return _elementi.Count; } }


    }
}
