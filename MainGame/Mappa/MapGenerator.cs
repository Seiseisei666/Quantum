using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Quantum_Game.Mappa
{
    public class MapGenerator
    {
        private int _righe, _colonne;
        private List<Tile> _tabellone;
        public int Righe { get { return _righe; } }
        public int Colonne { get { return _colonne; } }

        //Costruttore
        public MapGenerator(int righe, int colonne)
        {
            _righe = righe; _colonne = colonne;
            _tabellone = new List<Tile>();
            _tabellone.Capacity = righe * colonne;
        }

        //Funzione principale per generare la lista di caselle
        public List<Tile> GeneraMappa()
        {
            //legge tutte le righe del file
            string[] mapData = File.ReadAllLines(@"Content\Mappe\mappa_prova3.txt");

            //var width = mapData[0].Length;
            var height = mapData.Length;

            //concateno ad una stringa vuota le varie righe, ripulite dagli spazi (vedi metodo in fondo, forse da spostare)
            string tileData = String.Empty;
            for (int j = 0; j < height; j++)
            {
                tileData = tileData + ExceptBlanks(mapData[j]);
            }

            // ad ogni carattere dello stringone associo l'oggetto Casella appropriato
            StringBuilder sb = new StringBuilder(tileData.Length);
            for (int i = 0; i < tileData.Length; i++)
            {
                char c = tileData[i];
                if (c.Equals('#')) _tabellone.Add(new Vuoto());
                if (c.Equals('*')) _tabellone.Add(new Casella(QuantumTile.casella));
                if (c.Equals('+')) _tabellone.Add(new Casella(QuantumTile.orbita));
                if (c.Equals('7')) _tabellone.Add(new Pianeta(QuantumTile.Pianeta7));
                if (c.Equals('8')) _tabellone.Add(new Pianeta(QuantumTile.Pianeta8));
                if (c.Equals('9')) _tabellone.Add(new Pianeta(QuantumTile.Pianeta9));
                if (c.Equals('0')) _tabellone.Add(new Pianeta(QuantumTile.Pianeta10));
            }

            return _tabellone;
        }

        /* Vecchio metodo di prova randomico, per eventuali referenze future
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

        */

        // Serve a rimuovere gli spazi nella mappa letta dal file
        public string ExceptBlanks(string str)
        {
            StringBuilder sb = new StringBuilder(str.Length);
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (!char.IsWhiteSpace(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }

    }
}
