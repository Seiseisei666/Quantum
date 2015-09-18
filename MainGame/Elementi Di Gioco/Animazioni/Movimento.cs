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
            if (pos >= lungh || lungh <2) return;
            //Posizione = new Vector2(percorsoX.Evaluate(pos), percorsoY.Evaluate(pos));
            int n = (int)Math.Floor (pos / lungh);
            float xfade1 = pos - n;
            float xfade2 = 1 - xfade1;

            float x1, y1, x2, y2;

            x1 = _percorso[n].X * xfade1;
            y1 = _percorso[n].Y * xfade1;

            x2 = _percorso[n + 1].X * xfade2;
            y2 = _percorso[n + 1].Y * xfade2;

            Posizione = new Vector2(x1 + x2, y1 + y2);
            System.Diagnostics.Debug.WriteLine("{0}, {1}", Posizione.X, Posizione.Y);

            pos += INCREMENTO;
            if (n == lungh) pos = lungh;
        }

        public bool Completata { get { return pos >= lungh; } }

        const float INCREMENTO = 0.01f;
        public Vector2 Posizione { get; private set; }
        public float Rotazione { get; private set; }

    }
}
