using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantum_Game.Interfaccia;

namespace Quantum_Game.Schermate
{
    public class Schermata
    {
        public Schermata (params Riquadro [] elementi)
        {
            Elementi = new List<Riquadro>(elementi.Length);

            foreach (var e in elementi) Elementi.Add(e);

            isPopUp = false;
        }


        public bool isPopUp;

        public List<Riquadro> Elementi { get; protected set; }
    }
}
