using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Azioni
{
    public class MovimentoAttacco : AzioneDiGioco
    {
        public MovimentoAttacco(Game game) : base(game)
        {
            if (casellaCliccata == null)
                throw new ArgumentNullException("La casella era null");
            _casellaPartenza = casellaCliccata;
            pathFinder = game.Components.OfType<PathFinder>().First();
            pathFinder.Start(_casellaPartenza);
        }

        public override void Esegui()
        {

            if (ultimoClick == TipoEventoMouse.ClkDx || casellaCliccata == null)
            {    // Deselezione
                Cleanup();
                return;
            }

            int dist = pathFinder.DistanzaCasella(casellaCliccata);

            if (dist == 0 || dist > naveMossa.Pwr)
                return;

            Nave naveTarget = casellaCliccata.Occupante;
            if (naveTarget != null &&
                !naveTarget.Alleato(giocatoreDiTurno))
            {
                // Combattimento

                bool RisultatoAttacco;
                Debug.WriteLine("Una nave {0} di colore {1} ha attaccato una nave {2} di colore {3}.",
                            naveMossa.Tipo, naveMossa.Colore, naveTarget.Tipo, naveTarget.Colore);

                RisultatoAttacco = naveMossa.Attacco(naveTarget);
                Debug.WriteLine("risultato: {0}", RisultatoAttacco);

                if (RisultatoAttacco == true)
                    naveMossa.Muovi(_casellaPartenza, casellaCliccata);
            }
            else if (casellaCliccata != null && naveTarget == null)
            {
                // Movimento
                naveMossa.Muovi(_casellaPartenza, casellaCliccata);
            }
            // Fine procedura Movimento/Attacco
            Cleanup();
        }

        protected override void Cleanup()
        {
            pathFinder.Clear();
            AzioneSuccessiva = null;
        }

        Casella _casellaPartenza;
        private Nave naveMossa {get {return _casellaPartenza.Occupante;} }
        PathFinder pathFinder;
    }
}
