using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quantum_Game.Azioni;

namespace Quantum_Game.Animazioni
{
    public class Movimento: Azione
    {
        Quantum quantum;
        Nave naveMossa;
        Vector2[] _percorso;
        float pos = 0;
        int lungh;
        //float pos = 0f;
        //Curve percorsoX;
        //Curve percorsoY;

        public Movimento (Quantum quantum, Nave naveMossa, Casella[] percorso)
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
            this.quantum = quantum;
            this.naveMossa = naveMossa;
            _percorso = percorso.Select(casella => quantum.getGUI().Tabellone.Tile2Pixel(casella)).ToArray();
            lungh = percorso.Length;
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
            // Calcolo la rotazione
            Vector2 differenza = _percorso[n + 1] - _percorso[n];

            // Applico i valori alla nave
            var posizione = new Vector2(x, y);
            float rotazione = (float) Math.Atan2(differenza.X, - differenza.Y);
            naveMossa.Anima(posizione, rotazione);

            // Aggiorno la percentuale di posizione completata
            pos += INCREMENTO;
        }
        protected override void Cleanup() {Terminata = true; }
        public override bool Abort() { return false; }

        const float INCREMENTO = 0.09f;

    }
}
