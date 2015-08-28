using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Azioni
{
    public abstract class AzioneDiGiocoComplessa: AzioneDiGioco
    {
        public AzioneDiGiocoComplessa (Game game): base (game)
        {
            faseAttuale = new Fase(inizializzazione);
        }

        /// <summary> Le azioni di gioco complesse hanno bisogno di una fase speciale di gioco, l'inizializzazione, che serve a caricare le risorse che saranno usate nel corso dell'azione</summary>
        protected abstract void inizializzazione();

        /// <summary>Nelle azioni di gioco coplesse il metodo Esegui chiama un puntatore ad un altro metodo proprio dell'azione in corso</summary>
        public override void Esegui()
        {
            faseAttuale();
        }

        protected delegate void Fase();
        protected Fase faseAttuale;

    }
}
