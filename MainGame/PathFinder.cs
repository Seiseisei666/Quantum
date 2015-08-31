using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quantum_Game.Interfaccia;


namespace Quantum_Game
{
    public enum Direzioni
    {
        nessuna,
        Sopra,
        Sotto,
        Sinistra,
        Destra,
        AltoASinistra,
        AltoADestra,
        BassoASinistra,
        BassoADestra
    }

    public class PathFinder: IGameComponent
    {
        // COSTRUTTORE

        public PathFinder(Game game)
        {
            map = game.Services.GetService<Mappa>();
            tabellone = game.Services.GetService<GuiManager>().tabellone;
            Initialize();
        }

        // METODI PUBBLICI
        public void Initialize()
        {
            _partito = false;
            //inizializzazione matrice
            _matrice = new int[_numCaselle][];
            for (int i = 0; i < _numCaselle; i++)
                _matrice[i] = new int[0];
        }

        public void Start(Casella Partenza, int DistanzaMax = 12)
        {
            if (_partito) return;   // ignora chiamate successive multiple del metodo Start
            if (Partenza != null)
            {
                _distanzaMax = DistanzaMax;
                _partito = true;
                _nave = Partenza.Occupante;
                if (_nave == null)
                    throw new ArgumentNullException("Pathfinder chiamato con una casella di partenza su cui non c'è nessuna nave!!!");
                int tile;
                tile = map.Tile2Id(Partenza);
                crawl(tile, 0, new int[0], Direzioni.nessuna);
            }
        }

        public int DistanzaCasella (int IdCasella)
        {
            return PercorsoXCasella(IdCasella).Length;
        }
        public int DistanzaCasella(Tile tile)
        {
            return PercorsoXCasella(tile).Length;
        }

        public void Clear()
        {
            if (_partito == false)
                return;

            for (int i = 0; i < _numCaselle; i++)
            {
                // System.Diagnostics.Debug.WriteLine(PercorsoXCasella(i).Length);
                Array.Resize(ref _matrice[i], 0);
            }
            _nave = null;
            _partito = false;
        }

        public int[] IdCaselleValide
        {
            get
            {
                int c = 0;
                int[] caselle = new int[0];
                for (int id = 0; id < _numCaselle; id++)
                {
                    if (_matrice[id].Length > 0              // percorso esistente (almeno 1 casella)
                        && _matrice[id].Length <= _nave.Pwr) // lunghezza percorso alla portata della nave
                    {
                        Array.Resize(ref caselle, c + 1);
                        caselle[c++] = id;
                    }
                }
                return caselle;
            }
        }
        
        public int [] IdCaselleAdiacenti (Tile target)
        {
            int c = 0;
            int[] caselle = new int[0];
            Tile[] adiacenti = target.TileAdiacenti(true);
            foreach (Tile t in adiacenti)
            {
                Casella cas = t as Casella;
                if (cas != null && cas.Occupante == null)
                {
                    int id = map.Tile2Id(cas);
                    int dist = DistanzaCasella(id);
                    if (dist > 0 && dist < _nave.Pwr)
                    {
                        Array.Resize(ref caselle, c + 1);
                        caselle[c++] = id;
                    }
                }
            }
            return caselle;

        }


        public int[] PercorsoXCasella(Tile target)
        {
            if (target != null)
            {
                int id = map.Tile2Id(target);
                return _matrice[id];
            }
            else return new int[0];
        }
        /// <summary>
        /// restituisce l'array con gli id delle caselle da percorrere per raggiungere il tile targetId
        /// </summary>
        public int[] PercorsoXCasella(int targetId)
        {
            if (map.idValido(targetId)) return _matrice[targetId];
            else return new int[0];
        }

        // METODI PRIVATI

        /// <summary>
        /// Algoritmo ricorsivo
        /// </summary>
        /// <param name="IDtile">Indirizzo del tile su cui sta adesso il pathfinder</param>
        /// <param name="count">Lunghezza del percorso</param>
        /// <param name="percorso">Array che rappresenta gli indirizzi delle caselle percorse da questo ramo dell'algoritmo</param>
        /// <param name="DirezioneProvenienza">La direzione da cui è stato chiamato l'algoritmo; per evitare chiamate inutili non proseguirà il percorso in questa direzione</param>
        void crawl(int IDtile, int count, int[] percorso, Direzioni DirezioneProvenienza)
        {
            Tile tile = map.id2Tile(IDtile);
            if (!tile.EunaCasella                // non è una casella valida
                    // OR c'è una nave alleata che non è la nave che stiamo muovendo
                || (tile.PresenzaAlleata(_nave) && count != 0) 
                    // OR percorso già esistente e più breve di quello che ha trovato questo ramo della ricorsione
                || (_matrice[IDtile].Length > 0 && _matrice[IDtile].Length <= percorso.Length))
                return;

            if (count > 0)
            //Scrittura su memoria del percorso
            {
                percorso[count - 1] = IDtile;
                Array.Resize(ref _matrice[IDtile], count);
                Array.Copy(percorso, _matrice[IDtile], count);
            }
            count++;
            if (!tile.Attraversabile && !tile.PresenzaAlleata(_nave))
                return;
            if (count < _distanzaMax)
            // Chiamate ricorsive
            {
                int pos = IDtile;
                if (DirezioneProvenienza != Direzioni.Sopra && step(ref pos, Direzioni.Sopra))
                {
                    int[] tempArray = new int[count];
                    Array.Copy(percorso, tempArray, count - 1);
                    crawl(pos, count, tempArray, Direzioni.Sotto);
                }
                pos = IDtile;
                if (DirezioneProvenienza != Direzioni.Sotto && step(ref pos, Direzioni.Sotto))
                {
                    int[] tempArray = new int[count];
                    Array.Copy(percorso, tempArray, count - 1);
                    crawl(pos, count, tempArray, Direzioni.Sopra);
                }
                pos = IDtile;
                if (DirezioneProvenienza != Direzioni.Sinistra && step(ref pos, Direzioni.Sinistra))
                {
                    int[] tempArray = new int[count];
                    Array.Copy(percorso, tempArray, count - 1);
                    crawl(pos, count, tempArray, Direzioni.Destra);
                }
                pos = IDtile;
                if (DirezioneProvenienza != Direzioni.Destra && step(ref pos, Direzioni.Destra))
                {
                    int[] tempArray = new int[count];
                    Array.Copy(percorso, tempArray, count - 1);
                    crawl(pos, count, tempArray, Direzioni.Sinistra);
                }
            }
        }
            // Per trovare l'indice di una casella adiacente
        bool step(ref int pos, Direzioni dir)
        {
            if (dir == Direzioni.Sopra)
                return (pos -= _colonne) >= 0;
            else if (dir == Direzioni.Sotto)
                return (pos += _colonne) < _numCaselle;
            else if (dir == Direzioni.Destra)
                return ((++pos % _colonne) != 0) && pos < _numCaselle;
            else if (dir == Direzioni.Sinistra)
                return ((--pos % _colonne) != (_colonne - 1)) && pos >= 0;
            return false;
        }   

        // PROPRIETA' PRIVATE
        int _numCaselle { get { return map.NumeroCaselle; } }
        int _colonne { get { return map.Colonne; } }

        // CAMPI
            // status del pathfinder: se è già stato acceso deve essere spento prima di poterlo riutilizzare
        private bool _partito;
        // la lista caselle del tabellone... non è elegante come soluzione ma non sapevo come fare
        private Mappa map;
        private Tabellone tabellone;
            // memorizza i percorsi per raggiungere le caselle circostanti
        private int[][] _matrice; 
            // la nave che si sta muovendo
        private Nave _nave;
            // distanza massima che calcoliamo, sarebbe meglio farla costante
        private int _distanzaMax;

    }
}
