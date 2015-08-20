﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quantum_Game
{
    public abstract class Riquadro
    {
        private Rectangle _superficie; // rettangolo che rappresenta il Riquadro
        /// <summary>
    /// Offset (in pixel) del Riquadro
    /// </summary>
        public Point Offset { get { return _superficie.Location; } }
        /// <summary>
        /// dimensione assoluta(in pixel) del Riquadro
        /// </summary>
        public int Larghezza { get { return _superficie.Width; } }
        /// <summary>
        /// dimensione assoluta(in pixel) del Riquadro
        /// </summary>
        public int Altezza { get { return _superficie.Height; } }

        private float _larghRel, _altRel;

        #region Costruttori
        protected Riquadro
            (Game game, float xRel, float yRel, float LarghRelativa, float AltezzaRelativa)
        {
            GraphicsDevice graphicsDevice = game.Services.GetService<GraphicsDevice>();
            costruzioneRiquadro
                (xRel, yRel, LarghRelativa, AltezzaRelativa, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
        }
        protected Riquadro 
            (float xRel, float yRel, float LarghRelativa, float AltezzaRelativa, int LarghSchermo, int AltezzaSchermo)
        {
            costruzioneRiquadro
                (xRel, yRel, LarghRelativa, AltezzaRelativa, LarghSchermo, AltezzaSchermo);
        }
            // Metodo privato chiamato dai costruttori
        void costruzioneRiquadro(float xRel, float yRel, float LarghRelativa, float AltezzaRelativa, int LarghSchermo, int AltezzaSchermo)
        {
            float ofsX = MathHelper.Clamp(xRel, 0f, 1f);
            float ofsY = MathHelper.Clamp(yRel, 0f, 1f);
            _larghRel = MathHelper.Clamp(LarghRelativa, 0f, 1f);
            _altRel = MathHelper.Clamp(AltezzaRelativa, 0f, 1f);

            float w = (LarghRelativa - ofsX) * LarghSchermo;
            float h = (AltezzaRelativa - ofsY) * AltezzaSchermo;

            _superficie = new Rectangle(
                (int)(xRel * LarghSchermo),
                (int)(yRel * AltezzaSchermo),
                (int)w,
                (int)h
                                            );
        }

        #endregion Costruttori

        /// <summary>
        /// Ritorna true se il punto è compreso nella superficie del riquadro
        /// </summary>
        public bool Compreso (int x, int y) {return _superficie.Contains(x, y);}

        /// <summary>
        /// Ritorna true se il punto è compreso nella superficie del riquadro
        /// </summary>
        public bool Compreso (Point x)  {return _superficie.Contains(x);}

        /// <summary>
        /// Event Handler che viene chiamato in caso di ridimensionamento della finestra di gioco
        /// e che forza il ricalcolo delle dimensioni del riquadro in pixel
        /// </summary>
        /// <param name="args"></param>
        protected virtual void GestisciRidimensionamento (object sender, ResizeEvntArgs args)
        {
            _superficie.Height = (int)(_altRel * args.newScreenHeight);
            _superficie.Width = (int)(_larghRel * args.newScreenWidth);
        }

        protected virtual void ClickSinistro (object sender, MouseEvntArgs args)
        {
            // gestione click sinistro standard
            
        }
        protected virtual void ClickDestro(object sender, MouseEvntArgs args)
        {
            // gestione click dx standard
        }
        protected virtual void MouseOver (object sender, MouseEvntArgs args)
        {
            // gestione click sinistro standard
        }
        public abstract void Draw(SpriteBatch spriteBatch, Texture2D texture);
        /// <summary>
        /// Metodo per associare un evento del mouse ad un handler (metodo) a disposizione di questo oggetto
        /// (se il parametro associa è false l'associazione viene rimossa)
        /// </summary>
        /// <param name="tipo">Tipo di evento da associare/dissociare</param>
        /// <param name="Associa">True = associa, false = rimuovi associazione; predefinito true</param>
        public virtual void AssociaEvento(MouseInput mouseInput, TipoEventoMouse tipo, bool associa = true)
        {
            //TODO: capire se sta roba serve davvero

            switch (tipo)
            {
                case TipoEventoMouse.ClkSin:
                    if (associa)
                        mouseInput.ClickSinistro += ClickSinistro;
                    else mouseInput.ClickSinistro -= ClickSinistro;
                    break;
                case TipoEventoMouse.ClkDx:
                    if (associa)
                        mouseInput.ClickDestro += ClickDestro;
                    else mouseInput.ClickDestro -= ClickDestro;
                    break;
                case TipoEventoMouse.Over:
                    if (associa)
                        mouseInput.MouseOver += MouseOver;
                    else mouseInput.MouseOver -= MouseOver;

                    break;
            }
        }


    }
}
