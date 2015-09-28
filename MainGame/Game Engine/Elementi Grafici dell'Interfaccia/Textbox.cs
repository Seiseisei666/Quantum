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
        
        private int cursore = 0;


        public Textbox (Riquadro contenitore): base (contenitore)
        {
            Stringa = "QWEQWEQWEWQE";
        }

        public string Stringa { get; private set; }

        public override void CaricaContenuti(GuiManager gui)
        {
            font = gui.Font;
            font.DefaultCharacter = '?';
            texture = gui.Pennello;
        }

        public void Update()
        {
            var tasto = Keyboard.GetState().GetPressedKeys();
            if (tasto.Any())
            Stringa = Stringa + tasto.First().ToString();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, Stringa, new Vector2 (contenitore.Superficie.X, contenitore.Superficie.Y), Color.White);
        }

        protected override void ClickSinistro(object sender, MouseEvntArgs args)
        {

        }


    }
}
