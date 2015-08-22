using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Azioni
{
    public abstract class AzioneDiGioco
    {
        // Costruttore
        public AzioneDiGioco (Game game, FlussoDiGioco parent)
        {
            _game = game;
            _parent = parent;
            Completato = false;
            Inizializzazione();
        }
        public virtual bool Completato { get; protected set; }

        public abstract void Corpo();
        public virtual bool Cleanup()
        {
            return Completato;
        }
        protected abstract void Inizializzazione();

        protected FlussoDiGioco _parent;
        protected Game _game;

    }
}
