using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game
{
    public class Pianeta : Tile
    {
        //override di Tile
        public override bool Esistente { get { return true; } }
        public override bool EunPianeta { get { return true; } }

        //campi propri
        private e_color[] _colonizzazioni;
        public e_color[] Colonizzazioni {get { return _colonizzazioni; }}

        public bool Colonizzabile(e_color colore)
        {
            return _colonizzazioni.Any(x => x == 0) && !_colonizzazioni.Any(x => x == colore);
        }
        public bool Colonizza(Giocatore plyr)
        {
            if (plyr.PuòAgire(true) && this.Colonizzabile(plyr.Colore))
            {
                // Da togliere il check sulla condizione Colonizzabile? (lo fa il programma prima?)
                for (int i = 0; i < _colonizzazioni.Length - 1; i++)
                {
                    if (_colonizzazioni[i] == 0)
                    {
                        _colonizzazioni[i] = plyr.Colore;
                        Giocatore.Azione(true);                 // Toglie 2 azioni al giocatore
                        return true;
                    }
                }

            }
            return false;
        }


        //costruttore
        public Pianeta (QuantumTile tipo)
        {
            _tipo = tipo;
            _colonizzazioni = new e_color[4];
            for (int i = 0; i < _colonizzazioni.Length - 1; i++)
            {
                _colonizzazioni[i] = e_color.incolore;
            }
        }
    }
}
