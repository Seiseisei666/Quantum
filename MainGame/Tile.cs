using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game
{
    /// <summary>
    /// QUESTO FILE CONTIENE:
    /// QuantumTile e QuantumDirezioni, due enum relative alle caselle;
    /// La classe astratta Tile, da cui ereditano tutte le caselle;
    /// La classe Vuoto, che rappresenta un tile non esistente messo solo come segnaposto
    /// </summary>
    public enum QuantumTile : sbyte
    {
        vuoto = -1,
        casella,
        orbita,
        Pianeta7 = 7,
        Pianeta8,
        Pianeta9,
        Pianeta10
    }
    public enum QuantumDirezioni : sbyte
    {
        AltoSx,
        Alto,
        AltoDx,
        Sx,
        seStessa = -1,
        Dx = 5,
        BassoSx,
        Basso,
        BassoDx
    }



    public abstract class Tile
    {
        protected QuantumTile _tipo;
        public QuantumTile Tipo { get { return _tipo; } }

        abstract public bool Esistente { get; }
        public virtual bool Attraversabile { get { return false; } }
        public virtual bool EunPianeta { get { return false; } }
        public virtual bool EunaCasella { get { return false; } }
    }
    
    /// <summary>
    /// Settore vuoto... non fa assolutamente niente!
    /// </summary>
    public class Vuoto : Tile
    {
        public override bool Esistente { get { return false; } }
        public Vuoto ()
        {
            this._tipo = QuantumTile.vuoto;
        }
    }
}

