using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game.Animazioni
{
    public interface IAnimazione
    {
        void Esegui();
        bool Completata { get; }
        Microsoft.Xna.Framework.Vector2 Posizione { get; }
        float Rotazione { get; }
    }
}
