using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quantum_Game.Interfaccia
{
    public enum Allineamento
    {
        BassoDx,
        BassoSx,
        AltoDx,
        AltoSx
    }

    public class RiquadroGui
    {

        public Point Posizione { get { return new Point(superficie.X, superficie.Y); } }
        public int Altezza { get { return superficie.Height; } }
        public int Larghezza { get { return superficie.Width; } }

        /// <summary>Tiene traccia dell'oggetto "padre", che ha istanziato dinamicamente questo riquardo</summary>
        public object Parent { get { return _parent; } } 

        #region Costruzione
        /// <summary>Costruttore di un riquadro Gui di cui è importante tenere traccia dell'oggetto Parent</summary>
        /// <param name="parent">Istanza dell'oggetto "proprietario" del RiquadroGui</param>
        protected RiquadroGui(int xRel, int yRel, int larghRel, int altRel, object parent): 
            this (xRel, yRel, larghRel, altRel)
        {
            _parent = parent;
        }
        /// <summary>Costruttore di un RiquadroGui senza oggetto "parent"</summary>
        protected RiquadroGui(int xRel, int yRel, int larghRel, int altRel)
        {
            int x, y, w, h;
            x = MathHelper.Clamp(xRel, 1, 100);
            y = MathHelper.Clamp(yRel, 1, 100);
            w = MathHelper.Clamp(larghRel, 1, 100 - x);
            h = MathHelper.Clamp(altRel, 1, 100 - y);
            _superficieRel = new Rectangle(x, y, w, h);
        }

        protected RiquadroGui (Riquadro r)
        {
            // COSDTRUTTORE NUOVO SERATA MALGIOGLIO
            superficie = new Rectangle(r.Xabs, r.Yabs, r.Largh, r.Alt);
        }


        protected RiquadroGui (RiquadroGui vicino, Allineamento allineamento, int larghRel, int altRel)
        {
            int x, y, w, h;
            
            x = MathHelper.Clamp(vicino._superficieRel.X + vicino._superficieRel.Width, 1, 100);
            y = MathHelper.Clamp((vicino._superficieRel.Y + vicino._superficieRel.Height) -altRel, 1, 100);
            w = MathHelper.Clamp(larghRel,1,100);
            h = MathHelper.Clamp(altRel,1,100);
            _superficieRel = new Rectangle(x,y,w,h);
        }
        #endregion Costruzione

        /// <summary>Questo metodo va chiamato dal GuiManager; calcola la misura in pixel del componente grafico in base alle sue dimensioni relative in centesimi</summary>
        public virtual void Inizializzazione (GuiManager gui)
        {
            int larghSchermo = gui.GraphicsDevice.Viewport.Width;
            int altSchermo = gui.GraphicsDevice.Viewport.Height;

            float rapporto = 1;// (float)larghSchermo / altSchermo ;

            float fx = larghSchermo / (100f * rapporto);
            //fx /= rapporto;
            float fy = altSchermo / (100f );
            int x, y, w, h;
            x = (int)(_superficieRel.X * fx);
            y = (int)(_superficieRel.Y * fy);
            w = (int)(_superficieRel.Width * fx);
            h = (int)(_superficieRel.Height *fy);

            //superficie = new Rectangle(x, y, w,h );
        }
        
        public void AssociaEventiMouse(MouseInput m)
        {
            m.MouseOver += MouseOver;
            m.ClickDestro += ClickDestro;
            m.ClickSinistro += ClickSinistro;
        }

        public void DissociaEventiMouse (MouseInput m)
        {
            m.MouseOver -= MouseOver;
            m.ClickDestro -= ClickDestro;
            m.ClickSinistro -= ClickSinistro;
        }

        /// <summary>Metodo pecionata per spostare un bottone</summary>
        public virtual void Riposiziona (Point posizione)
        {
            superficie.X += posizione.X;
            superficie.Y += posizione.Y;
        }
        /// <summary>Ritorna true se il punto è compreso nella superficie del riquadro</summary>
        public bool Compreso(int x, int y) { return superficie.Contains(x, y); }

        /// <summary>Ritorna true se il punto è compreso nella superficie del riquadro</summary>
        public bool Compreso(Point x) { return superficie.Contains(x); }

        /// <summary>Da overrideare per rispondere al click del mouse</summary>
        protected virtual void ClickSinistro(object sender, MouseEvntArgs args) {   }

        /// <summary>Da overrideare per rispondere al click del mouse</summary>
        protected virtual void ClickDestro(object sender, MouseEvntArgs args) {   }

        /// <summary>Da overrideare per rispondere al movimento del mouse</summary>
        protected virtual void MouseOver(object sender, MouseEvntArgs args) {   }

        /// <summary>Da overrideare per implementare la routine di disegno dell'oggetto</summary>
        public virtual void Draw (SpriteBatch spriteBatch) {    }

        #region campiPrivati
        private Rectangle superficie;       //Dimensioni 
        protected Rectangle _superficieRel;
        protected object _parent;
        #endregion campiPrivati
    }
}
