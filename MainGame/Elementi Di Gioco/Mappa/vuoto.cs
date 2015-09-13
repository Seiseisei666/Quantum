using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game.Mappa
{

        /// <summary>
        /// Settore vuoto... non fa assolutamente niente!
        /// </summary>
        public class Vuoto : Tile
        {
            public override bool Esistente { get { return false; } }
            public Vuoto()
            {
                this._tipo = QuantumTile.vuoto;
            }
        }
}
