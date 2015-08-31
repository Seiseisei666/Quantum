using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Azioni
{
    public class MovimentoAttacco : AzioneDiGiocoComplessa
    {
        public MovimentoAttacco(Game game) : base(game)
        {
            _casellaPartenza = casellaCliccata;
        }
        public MovimentoAttacco(Game game, Casella casellaPartenza) : base (game)
        {
            _casellaPartenza = casellaPartenza;
        }

        protected override void inizializzazione()
        {
            naveMossa = _casellaPartenza.Occupante;
            gui.tabellone.ResetSelezioneMouse();

            pathFinder = new PathFinder(game);
            pathFinder.Start(_casellaPartenza);
            
            gui.tabellone.IlluminaCaselle(pathFinder.IdCaselleValide);

            faseAttuale = movimentoAttacco; // il puntatore faseAttuale viene chiamato dal metodo AzioneDiGiocoComplessa.Esegui()
        }



        void movimentoAttacco()
        {
            if (ultimoClick == TipoEventoMouse.ClkDx || (ultimoClick == TipoEventoMouse.ClkSin && casellaCliccata == null))
            {    // Deselezione
                Cleanup();
                return;
            }
            _casellaTarget = casellaCliccata;
            int dist = pathFinder.DistanzaCasella(_casellaTarget);

            if (dist == 0 || dist > naveMossa.Pwr)
                return;

            Nave naveTarget = _casellaTarget.Occupante;
            if (naveTarget != null &&
                !naveTarget.Alleato(giocatoreDiTurno))
            {
                // Combattimento

                bool risultatoAttacco;
                Debug.WriteLine("Una nave {0} di colore {1} ha attaccato una nave {2} di colore {3}.",
                            naveMossa.Tipo, naveMossa.Colore, naveTarget.Tipo, naveTarget.Colore);

                risultatoAttacco = naveMossa.Attacco(_casellaTarget);

                if (risultatoAttacco == true)
                {
                    Debug.WriteLine("Attacco riuscito! Piazza l'astronave sulla casella desiderata");
                }
                else
                {
                    Debug.WriteLine("Attacco fallito! Piazza l'astronave sulla casella desiderata");
                }

                _casellaPartenza.Occupante = null;  // rimuovo temporaneamente dal gioco la nave attaccante

                giocatoreDiTurno.Azione();

                gui.tabellone.ResetSelezioneMouse();

                gui.tabellone.IlluminaCaselle ( pathFinder.IdCaselleAdiacenti (_casellaTarget));

                faseAttuale = indietreggia;     // nuova fase
            }

            else if (_casellaTarget != null && naveTarget == null)
            {
                // Movimento
                naveMossa.Muovi(_casellaPartenza, _casellaTarget);
                giocatoreDiTurno.Azione();
                Cleanup();
            }
        }

        /// <summary>L'attaccante viene posizionato nella casella da cui proveniva l'attacco (ovvero, una casella distante 1 in meno del massimo movimento della nave attaccante)</summary>
        void indietreggia ()
        {
            int distanza = pathFinder.DistanzaCasella(casellaCliccata);
            if ((distanza <= 0 || distanza >= naveMossa.Pwr) && 
                casellaCliccata?.Equals(_casellaPartenza) == false)
                return;
            if (casellaCliccata?.Occupante == null &&
               (casellaCliccata == _casellaTarget ||                   
               casellaCliccata?.Circostante(_casellaTarget,true) == true))
            {
                naveMossa.Piazza(casellaCliccata);
                Cleanup();
            }
        }

        protected override void Cleanup()
        {
            pathFinder.Clear();
            gui.tabellone.ResetSelezioneMouse();
            gui.tabellone.SpegniCaselle();
            faseAttuale = null;
            AzioneSuccessiva = null;
        }

        Casella _casellaPartenza, _casellaTarget;
        Nave naveMossa;
        PathFinder pathFinder;
    }
}
