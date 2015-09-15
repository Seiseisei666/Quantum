using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quantum_Game.Interfaccia
{
    public interface IElementoAnimato
    {
        void Update();
    }

    public enum widget
    {
        Riconfigura,
        UsaSpecial,
    }


    public class Widget: ElementoGrafico, IElementoAnimato
    {
        public Widget (Point posizione, widget tipo): base (Riquadro.Main)
        {
            _posizione = new Vector2 (posizione.X, posizione.Y);
        }

        public override void CaricaContenuti(GuiManager gui)
        {
            _spriteSheet = gui.SpriteSheet;
        }

        public void Update ()
        {
            if (_mouseOver)
            {
                _scala *=1.2f;
                if (_scala.X > 0.55f) _scala = MAX_ESPANSIONE;
            }
            else
            {
                _scala *= 0.8f;
                if (_scala.X < 0.25f) _scala = MIN_ESPANSIONE;
            }
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw
                (_spriteSheet, 
                _posizione - _scala*50,
                sourceRectangle: new Rectangle(0, 0, 100, 100),
                scale: _scala,
                color: Color.White);
        }

        protected override void MouseOver(object sender, MouseEvntArgs args)
        {
            double x =  Math.Pow( (args.Posizione.X - _posizione.X),2);
            double y = Math.Pow((args.Posizione.Y - _posizione.Y),2);

            if (x + y < RAGGIO_QUADRATO)

            {
                _mouseOver = true;
            }

            else _mouseOver = false;
        }

        protected override void ClickSinistro(object sender, MouseEvntArgs args)
        {

        }

        bool _mouseOver;
        int _frame;
        Vector2 _posizione;
        Vector2 _scala = new Vector2(1, 1);
        Texture2D _spriteSheet;
        const float RAGGIO_QUADRATO = 400;
        readonly Vector2 MAX_ESPANSIONE = new Vector2(0.55f, 0.55f);
        readonly Vector2 MIN_ESPANSIONE = new Vector2(0.25f, 0.25f);
    }
}
