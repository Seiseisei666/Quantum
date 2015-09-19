using Quantum_Game.Interfaccia;
using System.Diagnostics;
using System;

namespace Quantum_Game.Azioni
{
    public class AzioneInizioPartitaOffLine : Azione
    {

        public AzioneInizioPartitaOffLine(Quantum quantum)
        {
            //EDIT: faccio tutto nel costruttore, tanto è una botta e via questa

            quantum.getGestoreDiAzioni().IncodaAzione(new AzioneStampaAConsole("Partita iniziata!"));

            foreach (Bottone b in quantum.getGUI().Bottoni)
            {
                b.Click += b.associatedEvent;
            }

        }

        /*Ad inizio partita tutti i bottoni vengono attivati...*/
        protected override void Esegui()
        {
            Terminata = true;
        }

        public override bool Abort() { return false; }

        /* A fine partita i bottoni vengono disattivati */
        protected override void Cleanup()
        {// EDIT: non ho capito, questa azione andrà distrutta subito... non possiamo dissociare gli eventi da qui, servirà un'altra azione apposita

            /* implemntare un if(partitaInCorso) do the following code
            foreach (Interfaccia.Bottone b in quantum.getGUI().Bottoni)
            {
                b.Click -= b.associatedEvent;
            }
            */
        }
    }
}
