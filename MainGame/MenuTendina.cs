using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Quantum_Game
{
    public class MenuTendina
    {
        public MenuTendina (Point Posizione, params bottone[] Bottoni)
        {
            if (Bottoni.Length == 0)
                throw new ArgumentException("Menu' a tendina vuoto");

            int i = 0;
            _elementi = new List<Bottone>(Bottoni.Length);
            foreach (var b in Bottoni)
            {
                _elementi.Add
                    (new Bottone(b, 
                    Posizione.X - LARGH_MENU / 2, 
                    Posizione.Y + (ALT_MENU * i) + OFFSETy,
                    LARGH_MENU, 
                    ALT_MENU * ++i));
            }
        }
        public List<Bottone> Elementi { get { return _elementi; } }



        List<Bottone> _elementi;
        const int LARGH_MENU = 84;
        const int ALT_MENU = 30;
        const int OFFSETy = 10;

    }
}
