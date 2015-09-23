using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quantum_Game.Azioni;

namespace Quantum_Game.Animazioni
{
    public class Movimento: Azione, IAnimazione
    {
        Nave nave;
        Vector2[] _percorso;
        float pos = 0;
        int lungh;
        //float pos = 0f;
        //Curve percorsoX;
        //Curve percorsoY;

        public Movimento (Nave nave, Vector2 [] percorso)
        {
            // Avevo provato ad usare le curve, ma è un po' difficile (bisogna settare tutte le tangenti una per una)
            // e forse, con il tabellone quadrato, non viene nemmeno bene
            // però, con i percorsi dritti, viene un po' troppo squadrato
            // qualche idea?

            //percorsoX = new Curve();
            //percorsoY = new Curve();
            //float i = 0;
            //float n = percorso.Length;
            //foreach (var p in percorso)
            //{
            //    percorsoX.Keys.Add(new CurveKey(i / n, p.X));
            //    percorsoY.Keys.Add(new CurveKey(i / n, p.X));
            //    i++;

            //}
            _percorso = percorso;
            lungh = percorso.Length;
            this.nave = nave;
            nave.Animazione = this;
        }

        protected override void Esegui()
        {
            int n = ((int)pos % lungh);

            if (n + 1 >= lungh)
            {
                Cleanup();
                return;
            }

            float xfade = pos - n;
            float x, y;
            // Interpolazione lineare fra 2 punti consecutivi del percorso
            x = MathHelper.Lerp ( _percorso[n].X, _percorso[n+1].X, xfade);
            y = MathHelper.Lerp (_percorso[n].Y, _percorso[n + 1].Y, xfade);
            Posizione = new Vector2(x, y);
            // Calcolo la rotazione
            Vector2 differenza = _percorso[n + 1] - _percorso[n];

            Rotazione = (float) Math.Atan2(differenza.X, - differenza.Y);
            // Aggiorno la posizione della nave nel percorso
            pos += INCREMENTO;
        }
        protected override void Cleanup() { nave.Animazione = null; Terminata = true; }
        public override bool Abort() { return false; }

        const float INCREMENTO = 0.09f;
        public Vector2 Posizione { get; private set; }
        public float Rotazione { get; private set; }

    }
}
