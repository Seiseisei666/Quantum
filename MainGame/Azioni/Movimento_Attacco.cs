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
        /// <summary>Costruttore per un movimento/attacco che parte da una casella ben definita</summary>
        public MovimentoAttacco(Game game, Casella casellaPartenza) : base (game)
        {
            _casellaPartenza = casellaPartenza;
        }

        protected override void inizializzazione()
        {
            naveMossa = _casellaPartenza.Occupante;
            gui.Tabellone.ResetSelezioneMouse();

            pathFinder = new PathFinder();
            pathFinder.Start(_casellaPartenza, naveMossa.MuoveInDiagonale);
            
            gui.Tabellone.IlluminaCaselle(pathFinder.IdCaselleValide);

            faseAttuale = movimentoAttacco; // il puntatore faseAttuale viene chiamato dal metodo AzioneDiGiocoComplessa.Esegui()
        }



        void movimentoAttacco()
        {
            int dist = pathFinder.DistanzaCasella(casellaCliccata);

            if (ultimoClick == TipoEventoMouse.ClkDx || (ultimoClick == TipoEventoMouse.ClkSin && casellaCliccata == null) || dist > naveMossa.Pwr || (dist == 0 && casellaCliccata?.Equals(_casellaPartenza) == false ))
            {    // Deselezione
                Cleanup();
                return;
            }
            _casellaTarget = casellaCliccata;


            Nave naveTarget = _casellaTarget?.Occupante;
            if (naveTarget != null &&
                !naveTarget.Alleato(giocatoreDiTurno))
            {
                // Combattimento

                bool risultatoAttacco;

                Interfaccia.ConsoleMessaggi.NuovoMessaggio("Una nave" + naveMossa.Tipo + " di colore" + naveMossa.Colore + " ha attaccato una nave" + naveTarget.Tipo + " di colore" + naveTarget.Colore + ".", giocatoreDiTurno.SpriteColor);

                risultatoAttacco = naveMossa.Attacco(_casellaTarget, true);



                if (risultatoAttacco)
                {
                    Interfaccia.ConsoleMessaggi.NuovoMessaggio ("Attacco riuscito! Piazza l'astronave sulla casella desiderata");

                }
                else
                {
                    Interfaccia.ConsoleMessaggi.NuovoMessaggio ("Attacco fallito! Piazza l'astronave sulla casella desiderata");
                }

                _casellaPartenza.Occupante = null;  // rimuovo temporaneamente dal gioco la nave attaccante

                giocatoreDiTurno.Azione();

                gui.Tabellone.ResetSelezioneMouse();

                int[] caselleAdiacentiTarget = pathFinder.IdCaselleAdiacenti(_casellaTarget, naveMossa.MuoveInDiagonale, risultatoAttacco);
                if (_casellaPartenza.Adiacente(_casellaTarget, naveMossa.MuoveInDiagonale))
                {
                    caselleAdiacentiTarget = caselleAdiacentiTarget.Concat(Enumerable.Repeat(_casellaPartenza.ID, 1)).ToArray();
                }

                gui.Tabellone.IlluminaCaselle (caselleAdiacentiTarget);

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
            if ((distanza > naveMossa.Pwr) || (distanza == 0 && 
                casellaCliccata?.Equals(_casellaPartenza) == false))
                return;
            if (casellaCliccata?.Occupante == null &&
               (casellaCliccata == _casellaTarget ||                   
               casellaCliccata?.Adiacente(_casellaTarget,true) == true))
            {
                naveMossa.Piazza(casellaCliccata);
                Cleanup();
            }
        }

        protected override void Cleanup()
        {
            pathFinder.Clear();
            gui.Tabellone.ResetSelezioneMouse();
            gui.Tabellone.SpegniCaselle();
            faseAttuale = null;
            AzioneSuccessiva = null;
        }

        Casella _casellaPartenza, _casellaTarget;
        Nave naveMossa;
        PathFinder pathFinder;
    }
}
