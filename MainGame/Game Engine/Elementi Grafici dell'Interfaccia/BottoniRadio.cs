using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Interfaccia
{
    internal class GruppoBottoniRadio: ElementoGrafico
    {
        List<BottoneCheck> Bottoni;
        BottoneCheck bottoneSelezionato;
        public BottoneCheck BottoneSelezionato { get { return bottoneSelezionato; }
        set
            {
                if (!value.Equals(bottoneSelezionato)) onValoreCambiato?.Invoke(value, EventArgs.Empty);
                bottoneSelezionato = value;
            }
        }

        public GruppoBottoniRadio (Riquadro contenitore, bool incolonnati, params string [] bottoni): base (contenitore)
        {
            Bottoni = new List<BottoneCheck>(bottoni.Length);
            float span = 100f / bottoni.Length;
            Riquadro r; BottoneCheck b;

            foreach (string bottone in bottoni)
            {
                if (incolonnati)
                    r = contenitore.Riga(span, 0, 10f, forzaQuadrato: false);
                else
                    r = contenitore.Colonna(span, 10f, 0, forzaQuadrato: false);

                b = new BottoneCheck(r, bottone);
                Bottoni.Add(b);
                b.Click += (s, e) => BottoneCliccato((BottoneCheck) s);
            }
        }

        public event EventHandler onValoreCambiato;

        void BottoneCliccato(BottoneCheck bottone)
        {
            foreach (var b in Bottoni)
                b.Selezionato = b.Equals(bottone);
            BottoneSelezionato = bottone;
        }

        public override void CaricaContenuti(GuiManager gui)
        {
            foreach (var b in Bottoni)
                b.CaricaContenuti(gui);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var b in Bottoni)
                b.Draw(spriteBatch);
        }


        internal class BottoneCheck : Bottone
        {
            public BottoneCheck(Riquadro contenitore, string testo, bool selezionato = false) : base(contenitore, testo)
            {
                this.selezionato = selezionato;
            }
            private bool selezionato;
            public bool Selezionato { get { return selezionato; } set { selezionato = value; } }

            public override event EventHandler Click;

            protected override void ClickSinistro(object sender, MouseEvntArgs args)
            {
                if (Enabled && contenitore.Superficie.Contains(args.Posizione.X, args.Posizione.Y))
                {
                    Click?.Invoke(this, EventArgs.Empty);
                }
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                {
                    Color color;

                    if (selezionato) color = Color.DarkOliveGreen;
                    else color = colSfondo;

                    if (!Enabled) color = Color.Gray;
                    float bright = mouseover ? 1 : 0.6f;
                    //bordo
                    spriteBatch.Draw(texture, new Vector2(contenitore.Superficie.X, contenitore.Superficie.Y), scale: new Vector2(contenitore.Superficie.Width, contenitore.Superficie.Height), color: colBordo);
                    //sfondo
                    spriteBatch.Draw(texture, new Rectangle(contenitore.Superficie.Location.X + 2, contenitore.Superficie.Location.Y + 2, contenitore.Superficie.Width - 4, contenitore.Superficie.Height - 4), Color.White);

                    spriteBatch.Draw(texture, new Rectangle(contenitore.Superficie.Location.X + 2, contenitore.Superficie.Location.Y + 2, contenitore.Superficie.Width - 4, contenitore.Superficie.Height - 4), color * bright);
                    //scritta
                    spriteBatch.DrawString(font, Caption, posScritta, Enabled ? Color.Black : Color.DarkGray);
                }
            }
        }

    }



}
