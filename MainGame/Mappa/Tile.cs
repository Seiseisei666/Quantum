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
        public virtual bool EunaCasella { get { return false; } }
        public virtual bool PresenzaAlleata (Nave nave) { return false; }
        public virtual bool PresenzaAlleata (Giocatore giocatore) { return false; }

        /// <summary>Overload dell'operatore + , permette di accedere ad un tile adiacente utilizzando la formula Tile + Direzione</summary>
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

        /// <summary>Calcola se questo tile è uno degli 8 vicini al tile argomento</summary><param name="compreseDiagonali">Se false, controlliamo solo in alto, in basso, a sin e a dx</param>
        public bool Adiacente(Tile centro, bool compreseDiagonali)
        {
            int max = compreseDiagonali ? (int)Direzioni.BassoADestra : (int)Direzioni.Destra;
            for (int dir = 1; dir <= max; dir++)
            {
                if (this.Equals(centro + (Direzioni)dir))
                    return true;
            }
            return false;
        }
        /// <summary>Calcola se questo tile è uno degli 8 vicini al tile argomento</summary><param name="compreseDiagonali">Se false, controlliamo solo in alto, in basso, a sin e a dx</param>
        public bool Adiacente (int idCentro, bool compreseDiagonali)
        {
            return Adiacente(mappa.id2Tile(idCentro), compreseDiagonali);
        }
        /// <summary>Fornisce la lista dei tile adiacenti a questo tile.</summary>
        public Tile[] TileAdiacenti (bool compresoTarget, bool compreseDiagonali)
        {
            Tile[] tiles = new Tile[8];
            int max = compreseDiagonali ? (int)Direzioni.BassoADestra : (int)Direzioni.Destra;
            for (int dir = 1; dir <= max; dir++)
            {
                tiles[dir - 1] = this + (Direzioni)dir;
            }

            return tiles;
        }
        /// <summary>Fornisce la lista dei tile adiacenti a questo tile.</summary>
        public int[] IdTileAdiacenti (bool compreseDiagonali)
        {
            int[] tiles = new int[8];
            int c = 0;
            foreach (Tile t in this.TileAdiacenti(false, compreseDiagonali))
            {
                tiles[c++] = mappa.Tile2Id(t);
            }
            return tiles;
        }
        public int[] IdTileAdiacenti(Func<Tile, bool> predicato, bool compreseDiagonali)
        {
            int[] tiles = new int[0];
            int c = 0;
            foreach (Tile t in this.TileAdiacenti(false, compreseDiagonali))
            {
                if (predicato(t))
                {
                    Array.Resize(ref tiles, c + 1);
                    tiles[c++] = mappa.Tile2Id(t);
                }

            }
            return tiles;
        }
        /// <summary> Filtra le caselle della mappa in base ad una data condizione. Restituisce gli ID delle caselle</summary>
        public static int[] IdTiles (Func<Tile,bool> filtro)
        {

            if (filtro == null) throw new ArgumentNullException("filtro");
            int[] risultato = new int[0];
            for (int i = 0; i < mappa.NumeroCaselle; i++)
            {
                if (filtro(mappa.id2Tile(i)))
                    risultato = risultato.Concat(new int[] { i }).ToArray();
            }
            System.Diagnostics.Debug.WriteLine(risultato.ToString());
            return risultato;
        }
        /// <summary> Filtra le caselle della mappa in base ad una data condizione </summary>
        public static Tile[] Tiles (Func<Tile,bool> filtro)
        {
            if (filtro == null) throw new ArgumentNullException("filtro");

            return mappa._listaCaselle.Where(t => filtro(t)).ToArray();

            Tile[] risultato = new Tile[0];
            for (int i = 0; i < mappa.NumeroCaselle; i++)
            {
                Tile t = mappa.id2Tile(i);
                if (filtro(t))
                    risultato = Enumerable.Empty<Tile>().Concat(new Tile[] { t }).ToArray();
            }

            return risultato;

        }

        public static Tile[] ToTile (int[] tiles)
        {
            if (tiles == null) throw new ArgumentNullException("tiles");
            Tile[] risultato = new Tile[0];
            foreach (int i in tiles)
            {
                risultato = Enumerable.Empty<Tile>().Concat(new Tile[] { mappa.id2Tile(i) }).ToArray();

            }
            return risultato;
        }
        // restituisce il pianeta più vicino alla casella argomento
        public Pianeta PianetaPiùVicino()
        {
            foreach (Tile t in this.TileAdiacenti(false, true))

            {
                Pianeta pianeta = t as Pianeta;
                if (pianeta != null) return pianeta;
            }

            return null;
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

