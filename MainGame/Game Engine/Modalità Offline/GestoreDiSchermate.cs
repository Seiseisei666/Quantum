using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantum_Game.Interfaccia;
using System.IO;

namespace Quantum_Game.Schermate
{

    public class GestoreDiSchermate
    {
        Quantum quantum;
        Schermata schermataCorrente;
        Stack<Schermata> popup;

        public GestoreDiSchermate(Quantum quantum)
        {
            this.quantum = quantum;
            popup = new Stack<Schermata>();
        }

        public void CaricaSchermata (Schermata s)
        {
            schermataCorrente = s;
        }

      
    }
}
