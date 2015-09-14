using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Quantum_Game
{
    /// <summary>
    /// Classe che si occupa della gestione dell'input del mouse
    /// e dell'inoltro degli eventi click dx, click sn e mouseover.
    /// Gli eventi vengono prodotti solo se qualche oggetto nel programma si è iscritto per riceverli
    /// </summary>
    public class MouseInput: GameComponent
    {
        private MouseState newState, oldState;

        public event EventHandler<MouseEvntArgs> ClickSinistro;
        public event EventHandler<MouseEvntArgs> ClickDestro;
        public event EventHandler<MouseEvntArgs> MouseOver;

        //costruttore vuoto
        public MouseInput(Game game) : base (game)
        {
            game.Services.AddService(this);
        }

        //chiamata dagli update del gioco
        public override void Update (GameTime gametime)
        {
            newState = Mouse.GetState();

            OnMouseOver();
            if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
                OnClickSinistro();
            else if (newState.RightButton == ButtonState.Pressed && oldState.RightButton == ButtonState.Released)
                OnClickDestro();

            oldState = newState;
        }

        private void OnClickSinistro()
        {
            if (ClickSinistro != null)
                ClickSinistro(this, new MouseEvntArgs(newState.X, newState.Y, true, false));
        }

        private void OnClickDestro()
        {
            if (ClickDestro != null)
                ClickDestro (this, new MouseEvntArgs(newState.X, newState.Y, false, true));
        }

        private void OnMouseOver ()
        {
            if (MouseOver != null)
                MouseOver(this, new MouseEvntArgs(newState.X, newState.Y, false, false));

        }

    }
}
