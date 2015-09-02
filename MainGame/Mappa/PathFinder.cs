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

    public class PathFinder
    {
        // COSTRUTTORE

        public PathFinder(Game game)
        {
            map = game.Services.GetService<Mappa>();
            _numCaselle = map.NumeroCaselle;
            _colonne = map.Colonne;
            _partito = false;
            //inizializzazione matrice
            _matrice = new int[_numCaselle][];
            for (int i = 0; i < _numCaselle; i++)
                _matrice[i] = new int[0];
        }
        /// <summary> Avvia il pathfinder per calcolare tutte le caselle raggiungibili. </summary>
        /// <param name="Partenza">Casella di partenza</param>
        /// <param name="muoveInDiagonale">determina se il movimento diagonale è abilitato</param>
        public void Start(Casella Partenza, bool muoveInDiagonale)
        {
            if (_partito) return;   // ignora chiamate successive multiple del metodo Start
            _muoveInDiagonale = muoveInDiagonale;

            if (Partenza != null)
            {
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
        
        public int [] IdCaselleAdiacenti (Tile target, bool compresoTarget = false)
        {
            int c = 0;
            int[] caselle = new int[0];
            Tile[] adiacenti = target.TileAdiacenti(true);
            foreach (Tile t in adiacenti)
            {
                Casella cas = t as Casella;
                if (cas != null && cas.Occupante == null)
                {
                    int id = cas.ID;
                    int dist = DistanzaCasella(id);
                    if (dist > 0 && dist < _nave.Pwr)
                    {
                        Array.Resize(ref caselle, c + 1);
                        caselle[c++] = id;
                    }
                }
            }
            if (compresoTarget && target.EunaCasella)
            {
                Array.Resize(ref caselle, caselle.Length+1);
                caselle[caselle.Length-1] = target.ID;
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
        /// <param name="count">Lunghezza attuale del percorso di questo ramo</param>
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
            if (count < DISTANZAMAX)
            // Chiamate ricorsive
            {
                int pos = IDtile;

                // Direzioni ortogonali
                if (_muoveInDiagonale)
                {
                    pos = map.idAdiacente(IDtile, Direzioni.AltoASinistra);
                    if (DirezioneProvenienza != Direzioni.AltoASinistra && map.idValido(pos))
                    {
                        int[] tempArray = new int[count];
                        Array.Copy(percorso, tempArray, count - 1);
                        crawl(pos, count, tempArray, Direzioni.BassoADestra);
                    }

                    pos = map.idAdiacente(IDtile, Direzioni.AltoADestra);
                    if (DirezioneProvenienza != Direzioni.AltoADestra && map.idValido(pos))
                    {
                        int[] tempArray = new int[count];
                        Array.Copy(percorso, tempArray, count - 1);
                        crawl(pos, count, tempArray, Direzioni.BassoASinistra);
                    }

                    pos = map.idAdiacente(IDtile, Direzioni.BassoASinistra);
                    if (DirezioneProvenienza != Direzioni.BassoASinistra && map.idValido(pos))
                    {
                        int[] tempArray = new int[count];
                        Array.Copy(percorso, tempArray, count - 1);
                        crawl(pos, count, tempArray, Direzioni.AltoADestra);
                    }

                    pos = map.idAdiacente(IDtile, Direzioni.BassoADestra);
                    if (DirezioneProvenienza != Direzioni.BassoADestra && map.idValido(pos))
                    {
                        int[] tempArray = new int[count];
                        Array.Copy(percorso, tempArray, count - 1);
                        crawl(pos, count, tempArray, Direzioni.AltoASinistra);
                    }
                }

                //Direzioni ortogonali

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
        int _numCaselle;
        int _colonne;

        // CAMPI
            // status del pathfinder: se è già stato acceso deve essere spento prima di poterlo riutilizzare
        private bool _partito;
            // 
        private bool _muoveInDiagonale;
        // la lista caselle del tabellone... non è elegante come soluzione ma non sapevo come fare
        private Mappa map;
            // memorizza i percorsi per raggiungere le caselle circostanti
        private int[][] _matrice; 
            // la nave che si sta muovendo
        private Nave _nave;
        // distanza massima che calcoliamo, sarebbe meglio farla costante
        const int DISTANZAMAX = 12;
    }
}
