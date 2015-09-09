using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game
{

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
    /// <summary> Classe astratta da cui ereditano tutti i tile. Contiene metodi statici per gestire le caselle </summary>
    public abstract class Tile
    {
        public Tile() { ID = _id++; }

        static List<Tile> _caselle;
        static int _colonne;
        static int _righe;
        static int _id = 0;
        public static int Righe {get {return _righe;} }
        public static int Colonne { get { return _colonne; } }

        public static void CreaMappa (List<Tile> Lista, int r, int c)
        {
            _caselle = Lista; _righe = r; _colonne = c;
        }

        public int ID { get; private set; }

        protected QuantumTile _tipo;
        public QuantumTile Tipo { get { return _tipo; } }

        abstract public bool Esistente { get; }
        public virtual bool EunaCasella { get { return false; } }
        public virtual bool PresenzaAlleata (Nave nave) { return false; }
        public virtual bool PresenzaAlleata (Giocatore giocatore) { return false; }


        /// <summary>Overload dell'operatore + , permette di accedere ad un tile adiacente utilizzando la formula Tile + Direzione</summary>
        public static Tile operator+ (Tile tile, Direzioni dir)
        {
            if (tile == null) return null;
            int id = tile.ID;
            switch (dir)
            {   
                // ortogonali
                case Direzioni.Sopra:
                    id -= _colonne; break;
                case Direzioni.Sotto:
                    id += _colonne; break;
                case Direzioni.Sinistra:
                    if (id % _colonne != 0)
                        id--;
                    else return null;
                    break;
                case Direzioni.Destra:
                    if (id % _colonne != _colonne - 1)
                        id++;
                    else return null;
                    break;

                // diagonali
                case Direzioni.AltoADestra:
                    return (tile + Direzioni.Sopra) + Direzioni.Destra;
                case Direzioni.AltoASinistra:
                    return (tile + Direzioni.Sopra) + Direzioni.Sinistra;
                case Direzioni.BassoADestra:
                    return (tile + Direzioni.Sotto) + Direzioni.Destra;
                case Direzioni.BassoASinistra:
                    return (tile + Direzioni.Sotto) + Direzioni.Sinistra;
            }

            if (idValido(id)) return _caselle[id];
            else return null;
        }

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
        /// <summary>Calcola se questo ID fa riferimento ad uno dei tile vicini al tile argomento</summary><param name="compreseDiagonali">Se false, controlliamo solo in alto, in basso, a sin e a dx</param>
        public bool Adiacente (int idCentro, bool compreseDiagonali)
        {
            return Adiacente(id2Tile(idCentro), compreseDiagonali);
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

        /// <summary> Filtra le caselle della mappa in base ad una data condizione. Restituisce gli ID delle caselle</summary>
        public static int[] IdTiles (Func<Tile,bool> filtro)
        {
            if (filtro == null) throw new ArgumentNullException("filtro");
            var tiles = Tiles(filtro);

            int[] risultato = new int[0];
            foreach (Tile t in tiles)
            {
                    risultato = risultato.Concat(new int[] { t.ID }).ToArray();
            }
            return risultato;
        }
        // USATO SOLO DA IdTiles ^^^
        /// <summary> Filtra le caselle della mappa in base ad una data condizione </summary>
        public static Tile[] Tiles (Func<Tile,bool> filtro)
        {
            if (filtro == null) throw new ArgumentNullException("filtro");

            return _caselle.Where(t => filtro(t)).ToArray();

        }
        // NON USATO
        public static Tile[] ToTile (int[] tiles)
        {
            if (tiles == null) throw new ArgumentNullException("tiles");
            return tiles.Select(i => id2Tile(i)).ToArray();
        }

        ///<summary> Restituisce il pianeta più vicino alla casella argomento</summary>
        public Pianeta PianetaPiùVicino(bool compreseDiagonali)
        {
            foreach (Tile t in TileAdiacenti(false, compreseDiagonali))

            {
                Pianeta pianeta = t as Pianeta;
                if (pianeta != null) return pianeta;
            }

            return null;
        }

        #region Ex Mappa

        ///<summary>Conversione da ID a Tile</summary>
        public static Tile id2Tile(int id)
        {
            if (idValido (id))
                return _caselle[id];
            else return null;
        }

        public static bool idValido(int id)
        {
            return (id >= 0 && id < _righe*_colonne);
        }

        public int idAdiacente(int id, Direzioni dir)
        {
            return (id2Tile(id) + dir)?.ID ?? -1; // -1 se il tile di arrivo non è valido (vuoto/inesistente)
        }
        #endregion


    }



}

