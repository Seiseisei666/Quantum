using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game.Azioni
{
    public class FineTurno: AzioneDiGioco
    {
        /// <summary>
        /// Qui gestiamo (gestiremo...) la fine del turno, ovvero:
        /// Piazzare mentine se si raggiunge il massimo di Dominio;
        /// Scartare le carte di troppo;
        /// Varie ed eventuali
        /// </summary>
        public FineTurno (Microsoft.Xna.Framework.Game game) : base (game)
        {
        }

        public override void Esegui()
        {
            Cleanup();
        }

        protected override void Cleanup()
        {
            AzioneSuccessiva = null;
            gameSystem.NextTurn();
            System.Diagnostics.Debug.WriteLine("Turno del giocatore {0}", gameSystem.GiocatoreDiTurno.Colore);
        }
    }
}
