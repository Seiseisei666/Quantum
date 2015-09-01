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
            {   // ortogonali
                case Direzioni.Sopra:
                    id -= mappa.Colonne; break;
                case Direzioni.Sotto:
                    id += mappa.Colonne; break;
                case Direzioni.Sinistra:
                    if (id % mappa.Colonne != 0)
                        id--;
                    else return null;
                    break;
                case Direzioni.Destra:
                    if (id % mappa.Colonne != mappa.Colonne - 1)
                        id++;
                    else return null;
                    break;
                // diagonali
                case Direzioni.AltoADestra:
                    if (id++ % mappa.Colonne != mappa.Colonne - 1)
                        id -= mappa.Colonne; break;
                case Direzioni.AltoASinistra:
                    if (id-- % mappa.Colonne != 0)
                        id -= mappa.Colonne; break;
                case Direzioni.BassoADestra:
                    if (id++ % mappa.Colonne != mappa.Colonne - 1)
                        id += mappa.Colonne; break;
                case Direzioni.BassoASinistra:
                    if (id-- % mappa.Colonne != 0)
                        id += mappa.Colonne; break;
            }

            if (mappa.idValido(id))
                return mappa.id2Tile(id);

            else return null;
        }

        public int ID { get { return mappa.Tile2Id(this); } }

        /// <summary>Calcola se questo tile è uno degli 8 intorno al tile argomento</summary>
        public bool Circostante(Tile centro, bool SoloOrtogonali = false)
        {
            int max = SoloOrtogonali ? (int)Direzioni.Destra : (int)Direzioni.BassoADestra;
            for (int dir = 1; dir <= max; dir++)
            {
                if (this.Equals(centro + (Direzioni)dir))
                    return true;
            }
            return false;
        }
        /// <summary>Fornisce la lista dei tile adiacenti a questo tile.</summary>
        public Tile[] TileAdiacenti (bool soloOrtogonali = false)
        {
            Tile[] tiles = new Tile[8];
            int max = soloOrtogonali ? (int)Direzioni.Destra : (int)Direzioni.BassoADestra;
            for (int dir = 1; dir <= max; dir++)
            {
                tiles[dir - 1] = this + (Direzioni)dir;
            }

            return tiles;
        }
        /// <summary>Fornisce la lista dei tile adiacenti a questo tile.</summary>
        public int[] IdTileAdiacenti (bool soloOrtogonali = false)
        {
            int[] tiles = new int[8];
            int c = 0;
            foreach (Tile t in this.TileAdiacenti(soloOrtogonali))
            {
                tiles[c++] = mappa.Tile2Id(t);
            }
            return tiles;
        }
        public int[] IdTileAdiacenti(Func<Tile, bool> predicato, bool soloOrtogonali = false)
        {
            int[] tiles = new int[0];
            int c = 0;
            foreach (Tile t in this.TileAdiacenti(soloOrtogonali))
            {
                if (predicato(t))
                {
                    Array.Resize(ref tiles, c + 1);
                    tiles[c++] = mappa.Tile2Id(t);
                }

            }
            return tiles;
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

