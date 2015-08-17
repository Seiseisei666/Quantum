﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game
{
    public class Pianeta : Tile
    {
        //costruttore
        public Pianeta(QuantumTile tipo)
        {
            _tipo = tipo;
            _colonizzazioni = new e_color[4];
            for (int i = 0; i < _colonizzazioni.Length; i++)
            {
                _colonizzazioni[i] = e_color.incolore;
            }
        }

        // PROPRIETA' PUBBLICHE

            //override di Tile
        public override bool Esistente { get { return true; } }
        public override bool EunPianeta { get { return true; } }
            // l'array delle mentine
        public e_color[] Colonizzazioni { get { return _colonizzazioni; } }

        // METODI PUBBLICI
            // Per controllare se il pianeta è colonizzabile
        public bool Colonizzabile(e_color colore)
        {
            return _colonizzazioni.Any(x => x == 0) && !_colonizzazioni.Any(x => x == colore);
        }
        public bool Colonizzabile(Giocatore player)
        {
            e_color colore = player.Colore;
            return _colonizzazioni.Any(x => x == 0) && !_colonizzazioni.Any(x => x == colore);
        }
            // L'azione di colonizzazione vera e propria
        public bool Colonizza(Giocatore plyr)
        {
            if (plyr.PuòColonizzare && this.Colonizzabile(plyr.Colore))
            {
                // Da togliere il check sulla condizione Colonizzabile? (lo fa il programma prima?)
                for (int i = 0; i < _colonizzazioni.Length - 1; i++)
                {
                    if (_colonizzazioni[i] == 0)
                    {
                        _colonizzazioni[i] = plyr.Colore;
                        plyr.Azione(true);                 // Toglie 2 azioni al giocatore
                        return true;
                    }
                }
            }
            return false;
        }


        //campi propri
        private e_color[] _colonizzazioni;
      
    }
}
