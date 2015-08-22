using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Quantum_Game
{
    public enum Direzioni
    {
        nessuna,
        Sopra,
        Sotto,
        Sinistra,
        Destra
    }

    public class PathFinder: IGameComponent
    {
        // COSTRUTTORE

        public PathFinder(Game game)
        {
            map = game.Services.GetService<Mappa>();
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
        public void Draw(Tabellone tabellone, SpriteBatch spriteBatch, Texture2D texture)
            // TODO: vorrei mettere questo metodo draw da un'altra parte, in modo da semplificarlo
            // (e magari non avere bisogno del riferimento al tabellone)
        {
            if (_partito == false)
                return;
            int LarghezzaCasellePix = tabellone.LarghezzaTilePx;
            int x, y, lungh = 0;
            Color colore = Color.Green;

                // Illumina tutte le caselle raggiungibili
            for (int i = 0; i < _numCaselle; i++)
            {
                if (_matrice[i].Length > 0              // percorso esistente (almeno 1 casella)
                    && _matrice[i].Length <= _nave.Pwr  // lunghezza percorso alla portata della nave
             //     && _listaCaselle[i].EunaCasella
                                                    )
                {
                    map.id2nm(i, out x, out y);
                    x *= LarghezzaCasellePix; y *= LarghezzaCasellePix;
                    x += tabellone.Offset.X; y += tabellone.Offset.Y;
                    spriteBatch.Draw(texture, new Rectangle(x, y, LarghezzaCasellePix, LarghezzaCasellePix), colore );
                }
            }
                // Illumina il percorso
            //foreach (int i in PercorsoXCasella(tabellone.IdMouseOver))
            //{
            //    id2nm(i, out x, out y);
            //    x *= LarghezzaCasellePix; y *= LarghezzaCasellePix;
            //    x += tabellone.Offset.X; y += tabellone.Offset.Y;
            //    colore = lungh < _nave.Pwr ? Color.LawnGreen : Color.IndianRed; // verde se in range, sennò rosso
            //    spriteBatch.Draw(texture, new Rectangle(x, y, LarghezzaCasellePix, LarghezzaCasellePix), colore);
            //    lungh++;
            //}
        }
        /// <summary>
        /// restituisce l'array con gli id delle caselle da percorrere per raggiungere il tile target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
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
            if (targetId >= 0) return _matrice[targetId];
            else return new int[0];
        }

        // METODI PRIVATI

            // Algoritmo ricorsivo
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
            // memorizza i percorsi per raggiungere le caselle circostanti
        private int[][] _matrice; 
            // la nave che si sta muovendo
        private Nave _nave;
            // distanza massima che calcoliamo, sarebbe meglio farla costante
        private int _distanzaMax;

    }
}
