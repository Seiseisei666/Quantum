using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game
{
    public enum e_sett
    {
        vuoto,
        Pianeta7 = 7,
        Pianeta8,
        Pianeta9,
        Pianeta10
    }


    public class Settore
    {
        private e_sett _tipo;
        private e_color[] _colonizzazioni;

        public Settore(e_sett tipo)
        {
            _tipo = tipo;
            _colonizzazioni = new e_color[4];
            for (int i=0; i < _colonizzazioni.Length-1; i++) {
                _colonizzazioni[i] = e_color.incolore;
            }
        }

        public e_sett Tipo { get { return this._tipo; } }

        // controlla se ci sono spazi vuoti e se nessuna mentina è del giocatore
        public bool Colonizzabile (e_color colore)
        {
            bool result;
            result = _colonizzazioni.Any(x => x == 0) && !_colonizzazioni.Any(x => x == colore);
            return result;
        }

        // Colonizza il pianeta
        public bool Colonizza (Giocatore plyr)
        {
            if (plyr.PuòAgire(true) && this.Colonizzabile (plyr.Colore)) {
                // Da togliere il check sulla condizione Colonizzabile? (lo fa il programma prima?)
                for (int i = 0; i < _colonizzazioni.Length - 1; i++)
                {
                    if (_colonizzazioni[i] == 0) { 
                        _colonizzazioni[i] = plyr.Colore;
                        Giocatore.Azione(true);                 // Toglie 2 azioni al giocatore
                        return true;
                    }
                }

            }
            return false;
        }





    }
}
