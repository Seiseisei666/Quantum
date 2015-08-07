using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game
{
    public class MapGenerator
    {
        private int _righe, _colonne;
        private List<Tile> _tabellone;

        public MapGenerator (int righe, int colonne)
        {
            _righe = righe; _colonne = colonne;
            _tabellone = new List<Tile>();
           _tabellone.Capacity = righe * colonne;
        }

        public List<Tile> GeneraMappa ()
        {
            // DA RIEMPIRE ************************
            return Test();
        }
        
        private List<Tile> Test ()
        {
            int ris;
         //   System.Diagnostics.Debug.WriteLine("Numero oggetti: {0}", _righe * _colonne);
            for (int i = 0; i < _righe * _colonne; i++)
            {
                ris = util.Dadi(2);
                
                if (ris < 3)
                    _tabellone.Add(new Vuoto());
                else if (ris < 7)
                    _tabellone.Add(new Casella(QuantumTile.casella));
                else if (ris < 9)
                    _tabellone.Add(new Casella(QuantumTile.orbita));
                else
                    _tabellone.Add(new Pianeta(QuantumTile.Pianeta10));
       //         System.Diagnostics.Debug.Write(_tabellone[i].Tipo + " ");

            }
      //      System.Diagnostics.Debug.WriteLine("Numero oggetti: {0}", _tabellone.Count);

            return _tabellone;
        }


    }
}
