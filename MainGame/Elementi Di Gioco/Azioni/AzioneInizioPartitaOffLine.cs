using Quantum_Game.Interfaccia;
using System.Diagnostics;

namespace Quantum_Game.Azioni
{
    public class AzioneInizioPartitaOffLine : Azione
    {
        private Quantum quantum;
        public AzioneInizioPartitaOffLine(Quantum quantum)
        {
            this.quantum = quantum;
        }
        /*Ad inizio partita tutti i bottoni vengono attivati...*/
        protected override void Esegui()
        {
            quantum.getGestoreDiAzioni().IncodaAzione(new AzioneStampaAConsole("Partita iniziata!"));
            Debug.WriteLine("Partita iniziata!!");

            foreach (Bottone b in quantum.getGUI().Bottoni)
            {
                b.Click += b.associatedEvent;
            }
        }


        /* A fine partita i bottoni vengono disattivati */
        protected override void Cleanup()
        {
            /* implemntare un if(partitaInCorso) do the following code
            foreach (Interfaccia.Bottone b in quantum.getGUI().Bottoni)
            {
                b.Click -= b.associatedEvent;
            }
            */
        }
    }
}
