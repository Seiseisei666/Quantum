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

        public override void Esegui()
        {
            faseAttuale();
        }

        /// <summary> Per default, nelle Azioni di Gioco Complesse, l'inizializzazione è la prima fase </summary>
        protected override void inizializzazione()
        {
            Func<Tile, bool> filtraAlleati = (t => t.PresenzaAlleata(_naveMossa));

            int[] tiles = Tile.IdTiles(filtraAlleati);
            int[] caselle = tiles.Where(t => { return _casellaPartenza.Adiacente(t, true); }).ToArray();


            //int[] caselle = _casellaPartenza.IdTileAdiacenti(filtraAlleati, false);
            gui.Tabellone.IlluminaCaselle(caselle);

            System.Diagnostics.Debug.WriteLine("Clicca su un alleato adiacente");
            faseAttuale = new Fase(sceltaAlleato); //puntiamo il delegato sulla prima fase dell'azione special
        }

        // Metodi su cui punterà di volta in volta il nostro delegato faseAttuale();
        void sceltaAlleato()
        {
           bool? alleato = casellaCliccata?.Occupante?.Alleato(giocatoreDiTurno);
           bool? circostante = casellaCliccata?.Adiacente(_casellaPartenza, true);
           if (circostante == true && alleato == true)
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
            Casella lastclick = casellaCliccata;
            azione.Esegui();

            if (azione.AzioneSuccessiva == null)    // l'azione di movimento/attacco si è conclusa
            { 
                if (_naveMossa.Mossa)               // la nave è mossa: l'azione non è stata "abortita"
                {
                    Interfaccia.ConsoleMessaggi.NuovoMessaggio("Clicca sulla casella dove vuoi posizionare l'alleato");
                    
                    // La nave che ha usato la special si è mossa, per cui dobbiamo sapere dove sta ora
                    // Ma dato che l'azione di movimento/Attacco si conclude piazzando la propria nave,
                    // siamo sicuri che questa si trova proprio sull'ultima casella cliccata
                    gui.Tabellone.ResetSelezioneMouse();
                    _casellaPartenza = lastclick;

                    azione = null;                  // annullo il mio riferimento all'azione di movimento

                    var caselle = Tile.IdTiles(t => {   // Per illuminare le caselle disponibili
                        Casella c = t as Casella;
                        return c != null && c.Occupante == null &&
                        lastclick.Adiacente(t, true);
                    });

                    gui.Tabellone.IlluminaCaselle(caselle);

                    faseAttuale = piazzaAlleato;    // passo alla fase successiva

                }

                else
                {
                    // TODO: qui bisogna abolire la special
                    _naveTrasportata.Piazza(_casellaTarget);
                    Cleanup();
                }
            }
        }

        void piazzaAlleato()
        {
            bool libera = (casellaCliccata != null && casellaCliccata.Occupante == null);
            bool circostante = casellaCliccata!= null && casellaCliccata.Adiacente(_casellaPartenza, true);

            if (libera && circostante)
            {
                _naveTrasportata.Piazza(casellaCliccata);
                _naveMossa.UsaSpecial();
                Cleanup();
            }
        }

        protected override void Cleanup()
        {
            gui.Tabellone.ResetSelezioneMouse();
            gui.Tabellone.SpegniCaselle();
            faseAttuale = null;
            AzioneSuccessiva = null;
        }

        Casella _casellaPartenza, _casellaTarget;
        Nave _naveMossa, _naveTrasportata;
        AzioneDiGioco azione;
    }
}
