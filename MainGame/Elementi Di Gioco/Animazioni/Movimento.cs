using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quantum_Game.Animazioni
{
    public class Movimento: IAnimazione
    {

        Vector2[] _percorso;
        float pos = 0;
        int lungh;
        //float pos = 0f;
        //Curve percorsoX;
        //Curve percorsoY;

        public Movimento (Vector2 [] percorso)
        {
            // Avevo provato ad usare le curve, ma è troppo difficile (bisogna settare tutte le tangenti una per una)
            // e probabilmente, con il tabellone quadrato, non viene nemmeno bene
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

        }

        public void Esegui()
        {
            int n = ((int)pos % lungh);

            if (n + 1 >= lungh)
            {
                Completata = true;
                return;
            }

            float xfade1 = pos - n;
            float x1, y1;
            // Interpolazione lineare fra 2 punti consecutivi del percorso
            x1 = MathHelper.Lerp ( _percorso[n].X, _percorso[n+1].X, xfade1);
            y1 = MathHelper.Lerp (_percorso[n].Y, _percorso[n + 1].Y, xfade1);
            // TODO: sistemare la rotazione!! Mi impiccio con le cose trigonometriche!
            // help wanted
            Vector2 differenza = _percorso[n + 1] - _percorso[n];
            Rotazione = (float) Math.Atan2(differenza.X, differenza.Y);
            Posizione = new Vector2 (x1,y1);

            pos += INCREMENTO;
        }

        public bool Completata { get; private set; }

        const float INCREMENTO = 0.09f;
        public Vector2 Posizione { get; private set; }
        public float Rotazione { get; private set; }

    }
}
