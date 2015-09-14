using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quantum_Game.Interfaccia;

namespace Quantum_Game.Interfaccia
{
    public abstract class ElementoGrafico : IDisposable
    {
        protected Riquadro contenitore;
        protected ElementoGrafico (Riquadro contenitore)
        {
            this.contenitore = contenitore;
            AssociaEventiMouse();
        }

        void AssociaEventiMouse ()
        {
            var mouse = Riquadro.Main.mouse;
            mouse.ClickSinistro += ClickSinistro;
            mouse.ClickDestro += ClickDestro;
            mouse.MouseOver += MouseOver;
        }
        void RimuoviEventi ()
        {
            var mouse = Riquadro.Main.mouse;
            mouse.ClickSinistro -= ClickSinistro;
            mouse.ClickDestro -= ClickDestro;
            mouse.MouseOver -= MouseOver;
        }

        public abstract void CaricaContenuti(GuiManager gui);

        public abstract void Draw(SpriteBatch spriteBatch);

        /// <summary>Da overrideare per rispondere al click del mouse</summary>
        protected virtual void ClickSinistro(object sender, MouseEvntArgs args) { }

        /// <summary>Da overrideare per rispondere al click del mouse</summary>
        protected virtual void ClickDestro(object sender, MouseEvntArgs args) { }

        /// <summary>Da overrideare per rispondere al movimento del mouse</summary>
        protected virtual void MouseOver(object sender, MouseEvntArgs args) { }

        public void Dispose()
        {
            RimuoviEventi();
        }
    }
}
