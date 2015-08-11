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
        private int _naveSpd;
        private Tile _posPartenza;
        private int [][] _matrice;         // lunghezza tragitto per raggiungere casella N / lista tragitto
        private int _numCaselle { get { return _listaCaselle.Count; } }
        private bool _partito;

        public int[] PercorsoXCasella (Tile target)
        {
            if (target != null)
            {
                int id = _listaCaselle.FindIndex(x => x.Equals(target));
                return _matrice[id];
            }
            return new int[0];
                
        }
        public int[] PercorsoXCasella (int targetId)
        {
            if (targetId > 0) return _matrice[targetId];
            return new int [0];
        }

        public PathFinder(List<Tile> ListaCaselle, int righe, int colonne)
        {
            _righe = righe; _colonne = colonne;
            _listaCaselle = ListaCaselle;
            _matrice = new int[_numCaselle][];
            _partito = false;
        }

        public void Start(Tile Partenza, int naveSpd, int DistanzaMax = 12)
        {
            if (_partito) return;
            if (Partenza != null)
            {
                _distanzaMax = DistanzaMax;
                _posPartenza = Partenza;
                _partito = true;
                _naveSpd = naveSpd;

                for (int i = 0; i < _numCaselle; i++)
                    _matrice[i] = new int[0];
            }

            int tile;
            tile = _listaCaselle.FindIndex(x => x.Equals(Partenza));
            crawl(tile, 0, new int [0], Direzioni.nessuna);
        }


        private void crawl (int IDtile, int count, int [] percorso, Direzioni DirezioneProvenienza)
        {
            if ((!_listaCaselle[IDtile].Attraversabile && count != 0) ||
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
