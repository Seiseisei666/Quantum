using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Quantum_Game.Interfaccia;

namespace Quantum_Game
{
    public class MenuTendina
    {
        public MenuTendina (Point posizione, bottone[] Bottoni, object parent)
        {
            Posizione = posizione;
            _elementi = new List<Bottone>(Bottoni.Length + 1);

            int i = 0;
          //  foreach (var bottone in Bottoni) 
              //  _elementi.Add
              //      (Bottone.MenuEntry(i++, bottone, parent));
        }

        public List<Bottone> Elementi { get { return _elementi; } }
        public Point Posizione { get; }

        List<Bottone> _elementi;
    }
}
