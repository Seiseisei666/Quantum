using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Serve per tenere traccia delle caselle esistenti.
/// Vengono organizzate in una lista. 
/// </summary>

namespace Quantum_Game
{
    public class Casella
    {
        public int ID_riga { get; set; }
        public int ID_colonna { get; set; }
        public int Tipo { get; set; }


        public Casella(int id_riga, int id_colonna, int tipo)
        {
            ID_riga = id_riga;
            ID_colonna = id_colonna;
            Tipo = tipo;
    
        }

        //andrebbe spostato qui il metodo che crea le caselle del tabellone (ora si trova nella classe Tabellone)
    }
}
