using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Serve per tenere traccia delle caselle esistenti.
/// Vengono organizzate in una lista. 
/// </summary>

namespace Quantum_Game
{


    /// <summary>
    /// Tutti i tipi di caselle ereditano da un modello astratto, la classe Tile,
    /// che contiene il campo pubblico Tipo e un altro paio di cosette overridabili
    /// </summary>
    public class Casella : Tile
    {
        // override di Tile
        public override bool Attraversabile { get { return (this._occupante == null); } }
        public override bool Esistente { get { return true; } }
        public override bool EunaCasella { get { return true; } }


        //campi propri
        private Nave _occupante;
        public Nave Occupante { get { return _occupante; } set { _occupante = value; } }

        private bool _orbita;
        public bool Orbita { get { return _tipo == QuantumTile.orbita; } }

        //costruttore
        public Casella(QuantumTile tipo)
        {
            _tipo = tipo;
            _occupante = null;
        }
    }
}

