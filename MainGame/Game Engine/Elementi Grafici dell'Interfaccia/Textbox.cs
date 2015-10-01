using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Quantum_Game.Interfaccia
{
    public class Textbox: ElementoGrafico, IElementoAnimato
    {
        SpriteFont font;
        Texture2D texture;
        KeyboardState newState, oldState;
        
        private int contatoreCursoreOn;
        bool cursoreOn;
        private int delayRimozioneBkspace;
        bool èSelezionato;

        public Textbox (Riquadro contenitore): base (contenitore)
        {
            Stringa = "DAJE FORTE!!";
        }
        public Textbox(Riquadro contenitore, string predef) : base(contenitore)
        {
            Stringa = predef;
        }

        public string Stringa { get; private set; }

        public override void CaricaContenuti(GuiManager gui)
        {
            font = gui.Font;
            font.DefaultCharacter = '?';
            texture = gui.Pennello;
            xMax = font.MeasureString(MAX_STR).X;
        }

        public void Update()
        {
            if (!èSelezionato) return;
            if (contatoreCursoreOn++ > 25)
            {
                cursoreOn = !cursoreOn;
                contatoreCursoreOn = 0;
            }

            // Comparo gli stati della tastiera
            oldState = newState;
            newState = Keyboard.GetState();

            // Variabili locali
            char c;

            foreach (Keys tasto in newState.GetPressedKeys())
            {
                if (tasto == Keys.Back && Stringa.Any())
                {
                    delayRimozioneBkspace++;
                    if (oldState.IsKeyUp (Keys.Back) || ( oldState.IsKeyDown (Keys.Back) && delayRimozioneBkspace > 35))
                    Stringa = Stringa.Remove(Stringa.Length - 1, 1);
                }

                if (oldState.IsKeyUp (tasto))
                {
                    if (tasto == Keys.Space)
                        Stringa = Stringa.Insert(Stringa.Length, " ");

                    else
                    {
                        // Lettera o numero
                        c = (char)tasto;
                        if (char.IsLetterOrDigit(c))
                            Stringa += 
                                newState.IsKeyDown(Keys.LeftShift)
                                ? char.ToUpper(c)
                                : char.ToLower(c);
                    }
                }
            }
            
            if (Stringa.Length > MAX_LUNGH)
                Stringa = Stringa.Remove(MAX_LUNGH);

            if (newState.IsKeyUp(Keys.Back)) delayRimozioneBkspace = 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, Stringa, new Vector2 (contenitore.Superficie.X, contenitore.Superficie.Y), Color.White);

            if (èSelezionato && cursoreOn)
                spriteBatch.Draw
                    (
                    texture,
                    new Vector2 (font.MeasureString(Stringa).X + contenitore.Superficie.Location.X, contenitore.Superficie.Location.Y),
                    scale: new Vector2(4, font.MeasureString(Stringa).Y),
                    color: Color.White
                    );
        }

        protected override void ClickSinistro(object sender, MouseEvntArgs args)
        {
            if (contenitore.Superficie.Contains(args.Posizione) &&
                args.Posizione.X - contenitore.Superficie.X <= xMax)
                èSelezionato = true;
            else èSelezionato = false;
        }

        const int MAX_LUNGH = 18;
        const string MAX_STR = "__________________";
        static float xMax;
    }
}
