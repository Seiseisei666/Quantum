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

    public class PathFinder
    {
        private List<Tile> _listaCaselle;
        private int _righe, _colonne;
        private int _distanzaMax;
        private int _naveSpd { get { return _nave.Pwr; } }
        private Nave _nave;
        private Tile _posPartenza;
        private int [][] _matrice; // per scrivere i percorsi per raggiungere le caselle
        private int _numCaselle { get { return _listaCaselle.Count; } }
        private bool _partito;

        /// <summary>
        /// restituisce l'array con gli id delle caselle da percorrere per raggiungere il tile target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public int[] PercorsoXCasella (Tile target)
        {
            if (target != null)
            {
                int id = _listaCaselle.FindIndex(x => x.Equals(target));
                return _matrice[id];
            }
            else return new int[0]; 
        }
        /// <summary>
        /// restituisce l'array con gli id delle caselle da percorrere per raggiungere il tile targetId
        /// </summary>
        public int[] PercorsoXCasella (int targetId)
        {
            if (targetId > 0) return _matrice[targetId];
            else return new int [0];
        }

        public PathFinder(List<Tile> ListaCaselle, int righe, int colonne)
        {
            _righe = righe; _colonne = colonne;
            _listaCaselle = ListaCaselle;
            _partito = false;
            _matrice = new int[_numCaselle][];
            for (int i = 0; i < _numCaselle; i++)
                _matrice[i] = new int[0];
        }

        public void Start(Tile Partenza, Nave nave, int DistanzaMax = 12)
        {
            if (_partito) return;
            if (Partenza != null)
            {
                _distanzaMax = DistanzaMax;
                _posPartenza = Partenza;
                _partito = true;
                _nave = nave;
                int tile;
                tile = _listaCaselle.FindIndex(x => x.Equals(Partenza));
                crawl(tile, 0, new int[0], Direzioni.nessuna);
            }
        }


        private void crawl (int IDtile, int count, int [] percorso, Direzioni DirezioneProvenienza)
        {
            if (!_listaCaselle[IDtile].EunaCasella || 
                (_listaCaselle[IDtile].PresenzaAlleata(_nave) &&
                count != 0) ||
                (_matrice[IDtile].Length > 0 && 
                _matrice[IDtile].Length <= percorso.Length))
                return;

           if (count > 0)
                //Scrittura su memoria del percorso
            {
                percorso[count-1] = IDtile;
                Array.Resize(ref _matrice[IDtile], count);
                Array.Copy(percorso, _matrice[IDtile], count);               
            }
            count++;
            if (!_listaCaselle[IDtile].Attraversabile && !_listaCaselle[IDtile].PresenzaAlleata(_nave))
                return;
            if (count < _distanzaMax)
                // Chiamate ricorsive
            {
                int pos = IDtile;
                if (DirezioneProvenienza != Direzioni.Sopra && step(ref pos, Direzioni.Sopra))
                {
                    int [] tempArray = new int[count];
                    Array.Copy(percorso,tempArray , count-1);
                    crawl(pos, count, tempArray, Direzioni.Sotto);
                }
                pos = IDtile;
                if (DirezioneProvenienza != Direzioni.Sotto && step(ref pos, Direzioni.Sotto))
                {
                    int[] tempArray = new int[count];
                    Array.Copy(percorso, tempArray, count-1);
                    crawl(pos, count, tempArray, Direzioni.Sopra);
                }
                pos = IDtile;
                if (DirezioneProvenienza != Direzioni.Sinistra && step(ref pos, Direzioni.Sinistra))
                {
                    int[] tempArray = new int[count];
                    Array.Copy(percorso, tempArray, count-1);
                    crawl(pos, count, tempArray, Direzioni.Destra);
                }
                pos = IDtile;
                if (DirezioneProvenienza != Direzioni.Destra && step(ref pos, Direzioni.Destra))
                {
                    int[] tempArray = new int[count];
                    Array.Copy(percorso, tempArray, count-1);
                    crawl(pos, count, tempArray, Direzioni.Sinistra);
                }
            }
    }

        

        private bool step (ref int pos, Direzioni dir)
        {
            if (dir == Direzioni.Sopra)
                return (pos -= _colonne) >= 0;
            else if (dir == Direzioni.Sotto)
                return (pos += _colonne) < _numCaselle;
            else if (dir == Direzioni.Destra)
                return ((++pos % _colonne) != 0) && pos <_numCaselle;
            else if (dir == Direzioni.Sinistra)
                return ((--pos % _colonne) != (_colonne - 1)) && pos >= 0;
            return false;
        }

        public void Clear ()
        {
            if (_partito == false)
                return;
               
            for (int i = 0; i < _numCaselle; i++)
            {
               // System.Diagnostics.Debug.WriteLine(PercorsoXCasella(i).Length);
                Array.Resize (ref _matrice[i],0);
            }
            _nave = null;
            _partito = false;
        }

        private void id2nm(int id, out int x, out int y)
        {
            x = id % _colonne;
            y = id / _colonne;
        }
        public void Draw (Tabellone tabellone, SpriteBatch spriteBatch, Texture2D texture)
        {
            if (_partito == false)
                return;
            int LarghezzaCasellePix = tabellone.LarghezzaTilePx;
            int x, y, lungh = 0;
            Color colore;

            foreach (int i in PercorsoXCasella(tabellone.IdMouseOver))
            {
                id2nm(i, out x, out y);
                x *= LarghezzaCasellePix; y *= LarghezzaCasellePix;
                x += tabellone.Offset.X; y += tabellone.Offset.Y;
                colore = lungh < _naveSpd ? Color.LawnGreen : Color.IndianRed; // verde se in range, sennò rosso
                spriteBatch.Draw(texture, new Rectangle(x, y, LarghezzaCasellePix, LarghezzaCasellePix), colore);
                lungh++;
            }
        }
    }
}
