using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Quantum_Game.Mappa
{
    public class MapGenerator
    {
        // dimensioni in Settori
        private int _righe = 0, _colonne = 0;  
        private string _filemappa;
        //private List<Tile> _tabellone; //Non dovrebbe servire
        // restituisce il numero di caselle
        public int Righe { get { return _righe*3; } } 
        public int Colonne { get { return _colonne*3; } }

        //Costruttore
        public MapGenerator(string filemappa)
        {
            _filemappa = filemappa;
            var dimensioni = File.ReadAllLines(filemappa);
            _righe = dimensioni.Length;
            _colonne = dimensioni[0].Length;

            // TODO: dobbiamo fare in modo di assegnare sempre alle colonne la dimensione più grande
            // per evitare di generare mappe lunghe e strette che sfanculano lo schermo
        }

        //Funzione principale per generare la lista di caselle
        public List<Tile> GeneraMappa()
        {
            var tabellone = new List<Tile>(_righe * _colonne);

            //legge tutte le righe del file in un array di stringhe
            string[] mapData = File.ReadAllLines(_filemappa);

            //concateno ad una stringa vuota le varie righe
            string tileData = String.Empty;
            for (int j = 0; j < _righe; j++)
                tileData = tileData + mapData[j];

            //nuovo algoritmo per generare la mappa da txt con solo pianeti!
            //sfrutto divisione intera per 3 (int/3=int) e resto modulo 3 (8%3=2)
            // è snello grazie alle simmetrie che ci sono nella mappa sulle colonne/righe senza pianeti!
            for (int x = 0; x < _righe * 3; x++)
                for (int y = 0; y < _colonne * 3; y++)
                {
                    char c = tileData[y / 3 + x / 3 * _colonne];
                    if (c.Equals('*')) tabellone.Add(new Vuoto());
                    else if (y % 3 == 1)
                    {
                        if (x % 3 == 1)
                        {
                            if (c.Equals('7')) tabellone.Add(new Pianeta(QuantumTile.Pianeta7));
                            if (c.Equals('8')) tabellone.Add(new Pianeta(QuantumTile.Pianeta8));
                            if (c.Equals('9')) tabellone.Add(new Pianeta(QuantumTile.Pianeta9));
                            if (c.Equals('0')) tabellone.Add(new Pianeta(QuantumTile.Pianeta10));
                        }
                        else tabellone.Add(new Casella(QuantumTile.orbita));
                    }
                    else
                    {
                        if (x % 3 == 1) tabellone.Add(new Casella(QuantumTile.orbita));
                        else tabellone.Add(new Casella(QuantumTile.casella));
                    }
                }

            return tabellone;
        }

        ////Vecchio generatore di map da txt, per usarlo in caso di bug
        //public List<Tile> GeneraMappa()
        //{
        //    //legge tutte le righe del file
        //    string[] mapData = File.ReadAllLines(@"Content\Mappe\mappa_prova3.txt");

        //    //var width = mapData[0].Length;
        //    var height = mapData.Length;

        //    //concateno ad una stringa vuota le varie righe, ripulite dagli spazi (vedi metodo in fondo, forse da spostare)
        //    string tileData = String.Empty;
        //    for (int j = 0; j < height; j++)
        //    {
        //        tileData = tileData + ExceptBlanks(mapData[j]);
        //    }

        //    // ad ogni carattere dello stringone associo l'oggetto Casella appropriato
        //    StringBuilder sb = new StringBuilder(tileData.Length);
        //    for (int i = 0; i < tileData.Length; i++)
        //    {
        //        char c = tileData[i];
        //        if (c.Equals('#')) _tabellone.Add(new Vuoto());
        //        if (c.Equals('*')) _tabellone.Add(new Casella(QuantumTile.casella));
        //        if (c.Equals('+')) _tabellone.Add(new Casella(QuantumTile.orbita));
        //        if (c.Equals('7')) _tabellone.Add(new Pianeta(QuantumTile.Pianeta7));
        //        if (c.Equals('8')) _tabellone.Add(new Pianeta(QuantumTile.Pianeta8));
        //        if (c.Equals('9')) _tabellone.Add(new Pianeta(QuantumTile.Pianeta9));
        //        if (c.Equals('0')) _tabellone.Add(new Pianeta(QuantumTile.Pianeta10));
        //    }

        //    return _tabellone;
        //}


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
        private string ExceptBlanks(string str)
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
