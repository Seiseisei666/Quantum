using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quantum_Game.Interfaccia
{
    
    public class ConsoleMessaggi : RiquadroGui
    {
        public ConsoleMessaggi(int x, int y, int larg, int alt) : base(x, y, larg, alt)
        {
            _messaggi = new List<MessaggioDiGioco>(CAPACITA);
            for (int i = 0; i < CAPACITA; i++)
                _messaggi.Add(new MessaggioDiGioco("", Color.White));
        }
        /// <summary>Aggiunge un messaggio colorato alla console</summary>
        public static void NuovoMessaggio(string messaggio, Color colore)
        {
            _messaggi[N] = new MessaggioDiGioco(messaggio, colore); N++;
        }
        /// <summary>Aggiunge un messaggio bianco alla console </summary>
        public static void NuovoMessaggio(string messaggio)
        {
            NuovoMessaggio(messaggio, Color.White);
        }
        /// <summary> Inutile</summary>
        public static void Aggiunta(string messaggio, Color colore)
        {
            string s = _messaggi[N].Messaggio;
            s = s + ' ' + messaggio;
            _messaggi[N] = new MessaggioDiGioco(s, colore);
        }

        
        public override void Inizializzazione(GuiManager gui)
        {
            base.Inizializzazione(gui);
            font = gui.Font;
            pennello = gui.Pennello;

            _altezzaRiga = font.MeasureString("A").Y* 1.25f;

            _righe = (int)(Altezza / _altezzaRiga);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pennello, new Rectangle(Posizione.X, Posizione.Y, Larghezza, Altezza), Color.White);
            spriteBatch.Draw(pennello, new Rectangle(Posizione.X+5, Posizione.Y+5, Larghezza-10, Altezza-10), Color.Black);

            for (int i = 0; i < _righe; i++)
            {
                int n = wrap(N - _righe + i);
                string messaggio = _messaggi[n].Messaggio;
                Color colore = _messaggi[n].Colore;
                spriteBatch.DrawString (font, messaggio, new Vector2(Posizione.X + 10, Posizione.Y + _altezzaRiga * i), colore);

            }
        }
        /// <summary> Funzione privata per wrappare gli indici fra 0...CAPACITA-1</summary>
        int wrap (int x)
        {
            if (x < 0) return CAPACITA + x;
            else return x;
        }

        SpriteFont font;
        int _righe;
        float _altezzaRiga;
        Texture2D pennello;

        static int N { get
            {
                return _contatore;
            }

            set { if (++_contatore >= CAPACITA) _contatore = 0; } }

        static List<MessaggioDiGioco> _messaggi;

        static int _contatore = 0;
        
        const int CAPACITA = 20;
        /// <summary>Coppia string-color che rappresenta i messaggi </summary>
        private class MessaggioDiGioco
        {
            public string Messaggio { get; set; }
            public Color Colore { get; set; }

            public MessaggioDiGioco(string testo, Color colore)
            {
                Messaggio = testo;
                Colore = colore;
            }
        }

    }
}
