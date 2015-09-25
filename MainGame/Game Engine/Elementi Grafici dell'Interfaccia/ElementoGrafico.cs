using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quantum_Game.Interfaccia;

namespace Quantum_Game.Interfaccia
{
    public abstract class ElementoGrafico
    {
        /// <summary>
        /// Riquadro che ospita l'elemento
        /// </summary>
        protected Riquadro contenitore;

        private GuiManager gui;

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="contenitore"></param>
        protected ElementoGrafico (Riquadro contenitore)
        {
            this.contenitore = contenitore;
            contenitore.Eliminazione += Eliminazione;
            AssociaEventiMouse();
        }

        /// <summary>
        /// Iscrizione agli eventi del mouse
        /// </summary>
        void AssociaEventiMouse ()
        {
            var mouse = Riquadro.Main.mouse;
            mouse.ClickSinistro += ClickSinistro;
            mouse.ClickDestro += ClickDestro;
            mouse.MouseOver += MouseOver;
        }

        /// <summary>
        /// Dissociazione dagli eventi del mouse, da chiamare quando l'oggetto va eliminato
        /// </summary>
        void RimuoviEventi ()
        {
            var mouse = Riquadro.Main.mouse;
            mouse.ClickSinistro -= ClickSinistro;
            mouse.ClickDestro -= ClickDestro;
            mouse.MouseOver -= MouseOver;
        }

        /// <summary>
        /// Chiamato dal Riquadro contenitore, quando viene distrutto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Eliminazione (object sender, EventArgs e)
        {
            Dispose();
        }

        public virtual void CaricaContenuti(GuiManager gui) { this.gui = gui; }

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
            contenitore.Eliminazione -= Eliminazione;
            gui.Rimuovi(this);
        }

        public void Dispose (bool chiamatoEsplicitamente)
        {

        }
    }
}
