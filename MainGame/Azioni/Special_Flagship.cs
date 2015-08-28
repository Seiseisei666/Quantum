using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;

namespace Quantum_Game.Azioni
{
    public class Special_Flagship: AzioneDiGiocoComplessa
    {
        public Special_Flagship (Game game, Casella casellaPartenza): base (game)
        {
            _casellaPartenza = casellaPartenza;
            _naveMossa = casellaPartenza.Occupante;
        }

        protected override void inizializzazione()
        {
            System.Diagnostics.Debug.WriteLine("Clicca su un alleato adiacente");
            faseAttuale = new Fase(sceltaAlleato); //puntiamo il delegato sulla prima fase dell'azione special
        }

        public override void Esegui()
        {
            faseAttuale();
        }

        // Metodi su cui punterà di volta in volta il nostro delegato faseAttuale();
        void sceltaAlleato()
        {
           bool? alleato = casellaCliccata?.Occupante?.Alleato(giocatoreDiTurno);
           bool? circostante = casellaCliccata?.Circostante(_casellaPartenza);
           if (circostante.HasValue && circostante == true &&
                alleato.HasValue && alleato == true)
            {
                // Selezionato alleato da trasportare
                _casellaTarget = casellaCliccata;
                _naveTrasportata = _casellaTarget.Occupante;
                _casellaTarget.Occupante = null;

                // prepara l'azione successiva
                azione = new MovimentoAttacco(game, _casellaPartenza);
                faseAttuale = movimentoAttacco;
            }
        }

        void movimentoAttacco()
        {

            azione.Esegui();

            if (azione.AzioneSuccessiva == null)    // l'azione di movimento/attacco si è conclusa
            { 
                if (_naveMossa.Mossa)               // la nave è mossa: l'azione non è stata "abortita"
                {
                    System.Diagnostics.Debug.WriteLine("Clicca sulla casella dove vuoi posizionare l'alleato");
                    
                    // La nave che ha usato la special si è mossa, per cui dobbiamo sapere dove sta ora
                    // Dato che l'azione di movimento/Attacco si conclude piazzando la propria nave,
                    // siamo sicuri che questa si trova proprio sull'ultima casella cliccata
                    _casellaPartenza = casellaCliccata;

                    azione = null;                  // annullo il mio riferimento all'azione di movimento

                    faseAttuale = piazzaAlleato;    // passo alla fase successiva
                }

                else
                {
                    // Abortiamo la special?
                }
            }
        }

        void piazzaAlleato()
        {
            bool libera = (casellaCliccata?.Occupante == null);
            bool? circostante = casellaCliccata?.Circostante(_casellaPartenza);

            if (libera &&
                circostante == true)
            {
                _naveTrasportata.Piazza(casellaCliccata);
                Cleanup();
            }
        }

        protected override void Cleanup()
        {
            gui.tabellone.ResetSelezioneMouse();
            _naveMossa.UsaSpecial();
            faseAttuale = null;
            AzioneSuccessiva = null;
        }

        Casella _casellaPartenza, _casellaTarget;
        Nave _naveMossa, _naveTrasportata;
        AzioneDiGioco azione;
    }
}
