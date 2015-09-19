using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game.Interfaccia
{
    public interface IElementoAnimato
    {
        void Update();
    }

    public enum widget
    {
        SfondoWidget,
        Riconfigura,
        UsaSpecial,
        Colonizza,
    }

    public enum doveDisegnoWidget
    {
        centro,
        sinistra,
        destra,
        sopra,
        sotto
    }

    public class Widget: ElementoGrafico, IElementoAnimato
    {
        public event EventHandler Click;

        public Widget (Point posizione, doveDisegnoWidget doveW, widget tipo, bool enabled): base (Riquadro.Main)
        {
            _posizione = new Vector2 (posizione.X, posizione.Y);
            _doveWidget = doveW;
            _enabled = enabled; 

            _scala = new Vector2(MIN_ESPANSIONE, MIN_ESPANSIONE);
        }


        public override void CaricaContenuti(GuiManager gui)
        {
            //_spriteSheet = gui.SpriteSheet;
            _spritePalliniAzioni = gui.SpritePalliniAzioni;
            _lunghLatoCasella = gui.Tabellone.LatoCasella;
            raggio_al_quadrato = (float)Math.Pow(_lunghLatoCasella / 4f, 2);

            switch (_doveWidget)
            {
                case doveDisegnoWidget.centro:
                    _posizione.X += 0;
                    _posizione.Y += 0;
                    break;
                case doveDisegnoWidget.sinistra:
                    //_posizione.X += -(_lunghLatoCasella/2);
                    _posizione.X += 0;
                    _posizione.Y += 0; 
                    break;
                case doveDisegnoWidget.destra:
                    _posizione.X += 0;
                    _posizione.Y += 0; 
                    break;
                default:
                    Console.WriteLine("Non posso posizionare il widget qui!");
                    break;
            }
        }

        public void Update ()
        {
            if (!_enabled) return;

            if (_mouseOver)
            {
                _fase += INCREMENTO;
                if (_fase > 1.0) _fase = 1 - _fase;
                var seno = (float)(Math.Sin(_fase * Math.PI)) * 0.06f ;

                _scala *=1.1f;
                if (_scala.X > MAX_ESPANSIONE) _scala = new Vector2(MAX_ESPANSIONE, MAX_ESPANSIONE);
                _scala += new Vector2(seno, seno);
            }
            else
            {
                _fase = 0;
                _scala *= 0.8f;
                if (_scala.X <1f) _scala = new Vector2(MIN_ESPANSIONE, MIN_ESPANSIONE);
            }
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            //calcolo quale tile sorgente usare
            Rectangle srcRect;
            Rectangle destRect;
            if (_doveWidget == doveDisegnoWidget.centro)    srcRect = new Rectangle(0, 0, 100, 100);
            else                                            srcRect = new Rectangle(100, 0, 200, 100);

            destRect = new Rectangle((int)_posizione.X, (int)_posizione.Y, _lunghLatoCasella, _lunghLatoCasella);
            spriteBatch.Draw(_spritePalliniAzioni, destRect, srcRect, Color.White);
            //spriteBatch.Draw(_spritePalliniAzioni, destRect, srcRect, Color.White, 1.57f, new Vector2(0,0), SpriteEffects.None, 0);
        }

        protected override void MouseOver(object sender, MouseEvntArgs args)
        {
            if (!_enabled) return;


            
            double x =  Math.Pow( (args.Posizione.X - _posizione.X),2);
            double y = Math.Pow((args.Posizione.Y - _posizione.Y), 2);

            //{
            //    if (args.Posizione.Y < _posizione.Y + _lunghLatoCasella && args.Posizione.Y > _posizione.Y )
            //    _mouseOver = true;
            //}

            //   if (args.Posizione.X < contenitore.Superficie.)

            if (x + y < raggio_al_quadrato)
                _mouseOver = true;
            else
            {
                _mouseOver = false;
            }
        }

        protected override void ClickSinistro(object sender, MouseEvntArgs args)
        {
            if (_mouseOver && _enabled) Click?.Invoke(this, EventArgs.Empty);
        }

        bool _mouseOver = false;
        readonly bool _enabled;
        float _fase = 0;

        Texture2D _spritePalliniAzioni;
        Vector2 _posizione;
        int _lunghLatoCasella;
        doveDisegnoWidget _doveWidget;
        Vector2 _scala = new Vector2(MIN_ESPANSIONE, MIN_ESPANSIONE);

        // TODO: valori provvisori calcolati con una sprite 100x100 pixel
        float raggio_al_quadrato;
        const float  MAX_ESPANSIONE = 1.25f;
        const float MIN_ESPANSIONE = 1f;

        const float INCREMENTO = 0.015f;
    }
}
