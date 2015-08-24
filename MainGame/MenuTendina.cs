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
                    (new Bottone (b, 
                    Posizione.X - OFFSET, 
                    Posizione.Y + (ALT_MENU * i++) + OFFSET,
                    LARGH_MENU, 
                    ALT_MENU    )   );
            }
        }
        public List<Bottone> Elementi { get { return _elementi; } }



        List<Bottone> _elementi;
        const int LARGH_MENU = 138;
        const int ALT_MENU = 35;
        const int OFFSET = 20 ;

    }
}
