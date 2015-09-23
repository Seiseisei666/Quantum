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
        Colonizza,
    }


    public class Widget: ElementoGrafico, IElementoAnimato
    {
        public event EventHandler Click;

        public Widget (Vector2 posizione, widget tipo, bool enabled): base (Riquadro.Main)
        {
            _posizione = posizione;

            // TODO: valore impostato ad occhio
            // con le sprite definitive, in caso di bottoncino più o meno circolare, bisognerà assicurarsi che il valore sia giusto 
            raggio_al_quadrato = Riquadro.Main.Superficie.Width * 0.4f;

            _enabled = enabled;
        }

        public override void CaricaContenuti(GuiManager gui)
        {
            _spriteSheet = gui.SpriteSheet;
        }

        public void Update ()
        {
            if (!_enabled) return;

            if (_mouseOver)
            {
                _fase += INCREMENTO;
                if (_fase > 2.0) _fase = 2 - _fase;
                var seno = (float)(Math.Sin(_fase * Math.PI)) * 0.06f ;

                _scala *=1.1f;
                if (_scala.X > MAX_ESPANSIONE) _scala = new Vector2(MAX_ESPANSIONE, MAX_ESPANSIONE);
                _scala += new Vector2(seno, seno);
            }
            else
            {
                _fase = 0;
                _scala *= 0.8f;
                if (_scala.X < 0.25f) _scala = new Vector2(MIN_ESPANSIONE, MIN_ESPANSIONE);
            }
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw
                (_spriteSheet, 
                _posizione - _scala*50,
                sourceRectangle: new Rectangle(0, 0, 100, 100),
                scale: _scala,
                color: _enabled ? Color.White : Color.Gray);
        }

        protected override void MouseOver(object sender, MouseEvntArgs args)
        {
            if (!_enabled) return;
            double x =  Math.Pow( (args.Posizione.X - _posizione.X),2);
            double y = Math.Pow((args.Posizione.Y - _posizione.Y),2);

            if (x + y < raggio_al_quadrato)

            {
                _mouseOver = true;
            }

            else _mouseOver = false;
        }

        protected override void ClickSinistro(object sender, MouseEvntArgs args)
        {
            if (_mouseOver && _enabled) Click?.Invoke(this, EventArgs.Empty);
        }

        bool _mouseOver = false;
        readonly bool _enabled;
        float _fase = 0;

        Texture2D _spriteSheet;
        Vector2 _posizione;
        Vector2 _scala = new Vector2(MIN_ESPANSIONE, MIN_ESPANSIONE);

        //valori provvisori calcolati con una sprite 100x100 pixel
        readonly float raggio_al_quadrato;
        const float  MAX_ESPANSIONE = 0.45f;
        const float MIN_ESPANSIONE = 0.25f;

        const float INCREMENTO = 0.015f;
    }
}
