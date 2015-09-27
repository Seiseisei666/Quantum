using System;
using Quantum_Game.Interfaccia;

namespace Quantum_Game.Azioni
{
    class AzioneSetupPartitaOffLine : Azione
    {

        private Quantum quantum;

        private int numeroGiocatori;

        public AzioneSetupPartitaOffLine(Quantum quantum, int numeroGiocatori)
        {
            this.quantum = quantum;
            this.numeroGiocatori = numeroGiocatori;
        }

        /*Viene eseguito il setum della partita */
        protected override void Esegui()
        {
            ConsoleMessaggi.NuovoMessaggio("Setup partita in corso...");

            quantum.getGestoreDiGiocatori().creaGiocatori(numeroGiocatori);

            //inizializza il giocatore e gli piazza tre navi
            foreach (Giocatore giocatore in quantum.getGestoreDiGiocatori().getGiocatori())
            {
                giocatore.GlobalInit();

                foreach (Nave nave in giocatore.NaviDaPiazzare)
                {
                    quantum.getGestoreDiAzioni().ImpilaAzione(AzionePiazzaNave.SetupPartita(quantum, nave));
                    quantum.getGUI().Tabellone.AggiungiNave(nave);
                }

                quantum.getGestoreDiAzioni().ImpilaAzione(new AzioneStampaAConsole(giocatore.ToString()+" piazza le navi..."));

            }
            
            //alla fine della fase di Setup lancio un'azione di inizio partita.
            //alternativamente posso farla lanciare tramite bottone
            quantum.getGestoreDiAzioni().IncodaAzione(new AzioneInizioPartitaOffLine(quantum)); //rimpiazza con crea bottone inizia partita
            Terminata = true;  
        }

        public override bool Abort() { return false; }

        /*  */
        protected override void Cleanup()
        {

        }
    }
}

