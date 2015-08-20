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
    
    public abstract class Tile
    {
        public static Mappa mappa { private get; set; }
        protected QuantumTile _tipo;
        public QuantumTile Tipo { get { return _tipo; } }

        abstract public bool Esistente { get; }
        public virtual bool Attraversabile { get { return false; } }
        public virtual bool EunPianeta { get { return false; } }
        public virtual bool EunaCasella { get { return false; } }
        public virtual bool PresenzaAlleata (Nave nave) { return false; }

        // Da oggi possiamo fare TileX + Sopra e trovare il tile che sta sopra!!!!1
        public static Tile operator+ (Tile tile, Direzioni dir)
        {
            int id = mappa.Tile2Id(tile);
            switch (dir)
            {
                case Direzioni.Sopra:
                    id -= mappa.Colonne; break;
                case Direzioni.Sotto:
                    id += mappa.Colonne; break;
                case Direzioni.Sinistra:
                    if (id % mappa.Colonne != 0)
                        id--; break;
                case Direzioni.Destra:
                    if (id % mappa.Colonne != mappa.Colonne - 1)
                        id++; break;
            }
            if (mappa.idValido(id)) return mappa.id2Tile(id);
            else return null;
        }

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

