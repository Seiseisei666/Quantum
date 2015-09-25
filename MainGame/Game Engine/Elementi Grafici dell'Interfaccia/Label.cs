using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Quantum_Game.Interfaccia;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Interfaccia
{
    /// <summary>
    /// Visualizza un testo arbitrario
    /// </summary>
    public class Label : ElementoGrafico
    {
        SpriteFont font;
        Texture2D pennello;
        Vector2 dimensioni;
        string _caption;

        public Label(Riquadro contenitore) : base(contenitore) { }
        public Label (Riquadro contenitore, string testo): this (contenitore) { _caption = testo; }

        public string Caption { get { return _caption; } set { _caption = value; if (font != null) dimensioni = font.MeasureString(_caption); } }

        public override void CaricaContenuti(GuiManager gui)
        {
            font = gui.Font;
            dimensioni = font.MeasureString(_caption);
            pennello = gui.Pennello;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(pennello, new Rectangle(contenitore.Superficie.X, contenitore.Superficie.Y, contenitore.Superficie.Width, contenitore.Superficie.Height), Color.Black);
            spriteBatch.DrawString(font, Caption, new Vector2(contenitore.Superficie.X, contenitore.Superficie.Y + dimensioni.Y), Color.White );
        }
    }
}
