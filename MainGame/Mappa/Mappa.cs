using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game
{
    /// <summary>
    /// La Mappa è la versione astratta del Tabellone. 
    /// Contiene tutte le proprietà posizionali dei Tile, e nessun riferimento alla parte grafica
    /// Iscrivendolo ai GameServices possiamo rendere "pubblici" questi dati e mantenere incapsulata
    /// la parte grafica e di input, gestita invece dal Tabellone
    /// </summary>
    public class Mappa
    {
        public Mappa (List<Tile> ListaCaselle, int Righe, int Colonne)
        {
            _listaCaselle = ListaCaselle;
            _righe = Righe;
            _colonne = Colonne;
            Tile.mappa = this;
        }

        //Varie uguaglianze fra id, tile e coordinate n,m

        public Tile id2Tile (int id)
        {
            if (id >= 0 && id < NumeroCaselle)
                return _listaCaselle[id];
            else return null;
        }

        public int Tile2Id (Tile tile)
        {
            return _listaCaselle.IndexOf(tile);
        }

        public int nm2id(int n, int m)
        {
            if (n >= 0 && m >= 0 && n < _colonne && m < _righe)
                return n + m * _colonne;
            else return -1;

        }
        public void id2nm(int idCasella, out int n, out int m)
        {
            if (!idValido(idCasella))
                throw new IndexOutOfRangeException("Indice non esistente");

            n = idCasella % _colonne;
            m = idCasella / _colonne;
        }
        public bool idValido (int id)
        {
            return (id >= 0 && id < NumeroCaselle);
        }

        public int idAdiacente (int idTile, Direzioni dir)
        {
            if (!idValido(idTile)) return -1;

            int id = idTile;

            switch (dir)
            {
                case Direzioni.Sopra:
                    id -= Colonne;
                    break;
                case Direzioni.Sotto:
                    id += Colonne;
                    break;
                case Direzioni.Sinistra:
                    if (id-- % Colonne == 0) return -1;
                    break;
                case Direzioni.Destra:
                    if (++id % Colonne == 0) return -1;
                    break;
                case Direzioni.AltoADestra:
                    return idAdiacente(idAdiacente(id, Direzioni.Sopra), Direzioni.Destra);

                case Direzioni.AltoASinistra:
                    return idAdiacente(idAdiacente(id, Direzioni.Sopra), Direzioni.Sinistra);

                case Direzioni.BassoASinistra:
                    return idAdiacente(idAdiacente(id, Direzioni.Sotto), Direzioni.Sinistra);

                case Direzioni.BassoADestra:
                    return idAdiacente(idAdiacente(id, Direzioni.Sotto), Direzioni.Destra);
                default:
                    return -1;
            }

            return idValido(id) ? id : -1;

        }


        public int NumeroCaselle { get { return _listaCaselle.Count; } }
        public int Righe { get { return _righe; } }
        public int Colonne { get { return _colonne; } }

        List<Tile> _listaCaselle;
        int _righe, _colonne;
    }
}
