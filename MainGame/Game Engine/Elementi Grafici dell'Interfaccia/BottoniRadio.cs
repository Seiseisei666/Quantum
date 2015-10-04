using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Interfaccia
{
    internal class GruppoBottoniRadio<T>
    {
        List<BottoneRadio<T>> Bottoni { get; set; }

        public GruppoBottoniRadio (Riquadro contenitore, bool incolonnati, params Tuple<string, T> [] bottoni)
        {
            Bottoni = new List<BottoneRadio<T>>(bottoni.Length);
            float span = 100f / bottoni.Length;
            Riquadro r; BottoneRadio<T> b;

            foreach (var bottone in bottoni)
            {
                if (incolonnati)
                    r = contenitore.Riga(span, 0, 2.5f, forzaQuadrato: true);
                else
                    r = contenitore.Colonna(span, 2.5f, 0, forzaQuadrato: true);

                b = new BottoneRadio<T>(r, bottone.Item1, bottone.Item2);
                Bottoni.Add(b);
                b.Click += (s, e) => BottoneCliccato((BottoneRadio<T>) s);
            }
        }

        public event EventHandler<RadioBottEventArgs<T>> Click;

        void BottoneCliccato(BottoneRadio<T> bottone)
        {
            foreach (var b in Bottoni)
                b.Selezionato = b.Equals(bottone);
        }



        private class BottoneRadio<T> : Bottone
        {
            public BottoneRadio(Riquadro contenitore, string testo, T valoreAssociato, bool selezionato = false) : base(contenitore, testo) { this.ValoreAssociato = valoreAssociato; }

            public T ValoreAssociato { get; private set; }

            public bool Selezionato { get; set; }

            public override event EventHandler Click;

            protected override void ClickSinistro(object sender, MouseEvntArgs args)
            {
                if (Enabled && contenitore.Superficie.Contains(args.Posizione.X, args.Posizione.Y))
                {
                    Click?.Invoke(this, EventArgs.Empty);
                }
            }
        }

    }



    internal class RadioBottEventArgs<T> : EventArgs
    {
        public T Valore;
        public RadioBottEventArgs (T Valore)
            {
            this.Valore = Valore;
            }
    }


}
