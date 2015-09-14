using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quantum_Game.Interfaccia
{
    
    public class ConsoleMessaggi : ElementoGrafico
    {
        public ConsoleMessaggi (Riquadro contenitore): base (contenitore)
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

        
        public override void CaricaContenuti(GuiManager gui)
        {

            font = gui.Font;
            pennello = gui.Pennello;

            _altezzaRiga = font.MeasureString("A").Y* 1.25f;

            _righe = (int)(contenitore.Superficie.Height / _altezzaRiga);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pennello, new Rectangle(contenitore.Superficie.Location.X, contenitore.Superficie.Location.Y, contenitore.Superficie.Width, contenitore.Superficie.Height), Color.White);
            spriteBatch.Draw(pennello, new Rectangle(contenitore.Superficie.Location.X+5, contenitore.Superficie.Location.Y+5, contenitore.Superficie.Width - 10, contenitore.Superficie.Height - 10), Color.Black);

            for (int i = 0; i < _righe; i++)
            {
                int n = wrap(N - _righe + i);
                string messaggio = _messaggi[n].Messaggio;
                Color colore = _messaggi[n].Colore;
                spriteBatch.DrawString (font, messaggio, new Vector2(contenitore.Superficie.Location.X + 10, contenitore.Superficie.Location.Y + _altezzaRiga * i), colore);

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
