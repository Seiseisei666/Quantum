using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantum_Game.Interfaccia;

namespace Quantum_Game.Schermate
{
    public abstract class Schermata
    {
        protected Quantum quantum;

        public Schermata (Quantum quantum)
        {
            this.quantum = quantum;
        }
    }
}
