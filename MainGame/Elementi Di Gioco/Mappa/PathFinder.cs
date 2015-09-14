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

        public PathFinder()
        {
            _numCaselle = Tile.Righe*Tile.Colonne;
            _colonne = Tile.Colonne;
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
                crawl(Partenza.ID, 0, new int[0], Direzioni.nessuna);
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
            // TODO: non credo che il metodo Clear serva più, dato che ora il pathfinder è una risorsa allocata
            // dinamicamente quando ne abbiamo bisogno
            if (_partito == false)
                return;
            _matrice = null;
        }
       

        /// <summary>
        /// restituisce l'array con gli id delle caselle da percorrere per raggiungere il tile targetId
        /// </summary>
        public int[] PercorsoXCasella(Tile target)
        {
            if (target != null) return PercorsoXCasella(target.ID);
            else return new int[0];
        }
        /// <summary>
        /// restituisce l'array con gli id delle caselle da percorrere per raggiungere il tile targetId
        /// </summary>
        public int[] PercorsoXCasella(int targetId)
        {
            if (Tile.idValido(targetId)) return _matrice[targetId];
            else return new int[0];
        }

        // METODI PRIVATI

        /// <summary>
        /// Algoritmo ricorsivo
        /// </summary>
        /// <param name="IDtile">Indirizzo del tile su cui sta adesso il pathfinder</param>
        /// <param name="count">Lunghezza attuale del percorso di questo ramo</param>
        /// <param name="percorso">Array che rappresenta gli indirizzi delle caselle percorse da questo ramo dell'algoritmo</param>
        /// <param name="direzioneProvenienza">La direzione da cui è stato chiamato l'algoritmo; per evitare chiamate inutili non proseguirà il percorso in questa direzione</param>
        void crawl(int IDtile, int count, int[] percorso, Direzioni direzioneProvenienza)
        {
            Casella casella = Tile.id2Tile(IDtile) as Casella;
            if (casella == null                // non è una casella valida
                    // OR c'è una nave alleata che non è la nave che stiamo muovendo
                || (casella.PresenzaAlleata(_nave) && count != 0) 
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
            if (casella.Occupante != null && !casella.PresenzaAlleata(_nave))
                return;
            if (count < DISTANZAMAX)
            // Chiamate ricorsive
            {
                int pos;

                // Direzioni ortogonali
                if (_muoveInDiagonale)
                {
                    pos = (casella + Direzioni.AltoASinistra)?.ID ?? -1;
                    if (direzioneProvenienza != Direzioni.AltoASinistra && Tile.idValido(pos))
                    {
                        int[] tempArray = new int[count];
                        Array.Copy(percorso, tempArray, count - 1);
                        crawl(pos, count, tempArray, Direzioni.BassoADestra);
                    }

                    pos = (casella + Direzioni.AltoADestra)?.ID ?? -1;
                    if (direzioneProvenienza != Direzioni.AltoADestra && Tile.idValido(pos))
                    {
                        int[] tempArray = new int[count];
                        Array.Copy(percorso, tempArray, count - 1);
                        crawl(pos, count, tempArray, Direzioni.BassoASinistra);
                    }

                    pos = (casella + Direzioni.BassoASinistra)?.ID ?? -1;
                    if (direzioneProvenienza != Direzioni.BassoASinistra && Tile.idValido(pos))
                    {
                        int[] tempArray = new int[count];
                        Array.Copy(percorso, tempArray, count - 1);
                        crawl(pos, count, tempArray, Direzioni.AltoADestra);
                    }

                    pos = (casella + Direzioni.BassoADestra)?.ID ?? -1;
                    if (direzioneProvenienza != Direzioni.BassoADestra && Tile.idValido(pos))
                    {
                        int[] tempArray = new int[count];
                        Array.Copy(percorso, tempArray, count - 1);
                        crawl(pos, count, tempArray, Direzioni.AltoASinistra);
                    }
                }

                //Direzioni ortogonali
                pos = (casella + Direzioni.Sopra)?.ID ?? -1;
                if (direzioneProvenienza != Direzioni.Sopra && Tile.idValido(pos))
                {
                    int[] tempArray = new int[count];
                    Array.Copy(percorso, tempArray, count - 1);
                    crawl(pos, count, tempArray, Direzioni.Sotto);
                }
                pos = (casella + Direzioni.Sotto)?.ID ?? -1;
                if (direzioneProvenienza != Direzioni.Sotto && Tile.idValido(pos))
                {
                    int[] tempArray = new int[count];
                    Array.Copy(percorso, tempArray, count - 1);
                    crawl(pos, count, tempArray, Direzioni.Sopra);
                }
                pos = (casella + Direzioni.Sinistra)?.ID ?? -1;
                if (direzioneProvenienza != Direzioni.Sinistra && Tile.idValido(pos))
                {
                    int[] tempArray = new int[count];
                    Array.Copy(percorso, tempArray, count - 1);
                    crawl(pos, count, tempArray, Direzioni.Destra);
                }
                pos = (casella + Direzioni.Destra)?.ID ?? -1;
                if (direzioneProvenienza != Direzioni.Destra && Tile.idValido(pos))
                {
                    int[] tempArray = new int[count];
                    Array.Copy(percorso, tempArray, count - 1);
                    crawl(pos, count, tempArray, Direzioni.Sinistra);
                }
            }
        }

        // CAMPI
        int _numCaselle;
        int _colonne;
            // status del pathfinder: se è già stato acceso deve essere spento prima di poterlo riutilizzare
        private bool _partito;
            // se true, il pathfinder si muoverà anche in diagonale
        private bool _muoveInDiagonale;
            // memorizza i percorsi per raggiungere le caselle circostanti
        private int[][] _matrice; 
            // la nave che si sta muovendo
        private Nave _nave;
            // distanza massima che calcoliamo
        const int DISTANZAMAX = 12;
    }
}
