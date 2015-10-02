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

        private int _posCursore;
        private int posCursore {
            get
            {
                return _posCursore;
            }
            set
            {
                _posCursore = MathHelper.Clamp(value, 0, Stringa.Length);
            }
        }
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
        public bool Enabled { private get; set; }

        public override void CaricaContenuti(GuiManager gui)
        {
            font = gui.Font;
            font.DefaultCharacter = '?';
            texture = gui.Pennello;
            vMax = font.MeasureString(MAX_STR);
            xMax = vMax.X;
            offsetY = contenitore.Superficie.Y + ((contenitore.Superficie.Height - vMax.Y) / 2f);
        }

        public void Update()
        {
            if (! ( èSelezionato && Enabled)) return;
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
                if (tasto == Keys.Enter)
                {
                    èSelezionato = false;
                    return;
                }

                if (tasto == Keys.Back && Stringa.Substring(0,posCursore).Any())
                {
                    delayRimozioneBkspace++;
                    if (oldState.IsKeyUp (Keys.Back) || ( oldState.IsKeyDown (Keys.Back) && delayRimozioneBkspace > 35))
                        Stringa = Stringa.Remove(--posCursore, 1);
                }

                if (oldState.IsKeyUp (tasto))
                {
                    if (tasto == Keys.Space)
                        Stringa = Stringa.Insert(posCursore++, " ");

                    else
                    {
                        // Lettera o numero
                        c = (char)tasto;
                        if (char.IsLetterOrDigit(c))
                        {
                            Stringa = Stringa.Insert(posCursore,
                                newState.IsKeyDown(Keys.LeftShift)
                                ? c.ToString()
                                : char.ToLower(c).ToString()
                                );
                            posCursore++;
                            if (posCursore > MAX_LUNGH) posCursore = MAX_LUNGH;
                        }
                    }
                }
            }
            if (Stringa.Length > MAX_LUNGH)
                Stringa = Stringa.Remove(MAX_LUNGH);

            if (newState.IsKeyUp(Keys.Back)) delayRimozioneBkspace = 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, Stringa, new Vector2 (contenitore.Superficie.X, offsetY), Enabled ? Color.White : new Color(0x22,0x22,0x22));

            if (èSelezionato && cursoreOn)
                spriteBatch.Draw
                    (
                    texture,
                    new Vector2 (font.MeasureString(Stringa.Substring(0, posCursore)).X + contenitore.Superficie.X, offsetY),
                    scale: new Vector2(4, font.MeasureString(Stringa).Y),
                    color: Color.White
                    );
            //if (!Enabled)
            //    spriteBatch.Draw
            //        (
            //        texture,
            //        new Vector2(contenitore.Superficie.X, contenitore.Superficie.Y),
            //        scale: vMax,
            //        color: Color.Gray);
        }

        protected override void ClickSinistro(object sender, MouseEvntArgs args)
        {
            if (contenitore.Superficie.Contains(args.Posizione) &&
                args.Posizione.X - contenitore.Superficie.X <= xMax)
            {
                èSelezionato = true;
                string txt; int i = 0;

                while (i <= Stringa.Length)
                {
                    txt = Stringa.Substring(0, i);
                    if (font.MeasureString(txt).X <
                        args.Posizione.X - contenitore.Superficie.X)
                        i++;
                    else break;
                }
                posCursore = i;
            }
            else èSelezionato = false;

        }

        const int MAX_LUNGH = 18;
        const string MAX_STR = "__________________";
        static Vector2 vMax;
        static float xMax;
        /// <summary>
        /// Offset verticale per centrare il textbox all'interno del riquadro contenitore
        /// </summary>
        float offsetY;
    }
}
