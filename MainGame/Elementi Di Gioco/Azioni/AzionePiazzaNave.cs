using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game.Azioni
{
    public class AzionePiazzaNave : Azione
    {
        private Quantum quantum;
        private Casella[] caselleDisponibili;
        private Nave naveDaPiazzare;

        // COSTRUTTORE PER LA FASE DI SETUP
        public AzionePiazzaNave(Quantum quantum, Nave nave) :
            this(quantum, nave, Tile.Tiles(t => t is Casella).Select(t => (Casella)t).ToArray())
        { }        // Le navi non possono essere piazzate in qualsiasi casella! Dev'esserci una mentina nel pianeta
                    // TODO: quando ci sono le mentine va modificata la selezione delle caselle disponibili


        public AzionePiazzaNave (Quantum quantum, Nave naveDaPiazzare, Casella[] caselleDisponibili)
        {
            // Piazzo una Nave in una casella arbitraria fra quelle dell'argomento
            this.quantum = quantum;
            this.caselleDisponibili = caselleDisponibili;
            this.naveDaPiazzare = naveDaPiazzare;
        }

        protected override void Esegui()
        {
            // SEMI-BUG: chiamiamo IlluminaCaselle ogni frame...
            // Soluzione rapida: spostare la riga seguente nel costruttore
            // ma col sistema dell'incodamento di azioni non funziona bene...
            // Soluzione ema: rinunciamo all'incodamento di azioni =P tanto lo usiamo solo nel setup iniziale!
            quantum.getGUI().Tabellone.IlluminaCaselle(caselleDisponibili.Select(cas => cas.ID).ToArray());
            Casella casella = quantum.getGUI().Tabellone.TileClick as Casella; // prova a castare il tile selezionato come casella
            Nave nave = naveDaPiazzare;

            if (nave != null)
            {
                if (caselleDisponibili.Contains(casella) && casella.Occupante == null)
                {
                    nave.Piazza(casella);
                    Cleanup();
                }
                // L'azione non termina finché la nave non viene piazzata
            }
        }

        public override bool Abort() { return false; }

        protected override void Cleanup()
        {
            quantum.getGUI().Tabellone.ResetSelezioneMouse();
            quantum.getGUI().Tabellone.SpegniCaselle();
            Terminata = true;
        }
    }
}
