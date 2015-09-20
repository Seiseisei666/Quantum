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
            //REPLY: OK! Se il problema è che ti sta scomodo passare quantum, questa azione necessita
            // solo di GestoreDiAzioni e GestoreDiGiocatori per funzionare (in termini di memoria sono solo due liste, molto ragionevole).
            //Quindi si puo' costruirla come public AzioneInizioPartitaOffLine(GestoreDiAzioni a, GestoreDiGiocatori b).
            //Probabilmente vale anche per le altre azioni, avevo messo quantum in via provvisoria
            //EDIT2: serve anche la GUI a meno di non attivare i bottoni in qualche altro modo

            quantum.getGestoreDiAzioni().IncodaAzione(new AzioneStampaAConsole("Partita iniziata!"));

            foreach (Bottone b in quantum.getGUI().Bottoni)
            {
                b.Click += b.associatedEvent;
            }
            //probabilmente ha senso creare un'azione di inizio turno (che verrà usata anche dal bottone passa turno)
            quantum.getGestoreDiGiocatori().getGiocatoreDiTurno().Init();
            quantum.getGestoreDiAzioni().IncodaAzione(new AzioneSelezione(quantum));
            Cleanup();
        }

        /*Ad inizio partita tutti i bottoni vengono attivati...*/
        protected override void Esegui()
        {
        }

        public override bool Abort() { return false; }

        /* A fine partita i bottoni vengono disattivati */
        protected override void Cleanup()
        {// EDIT: non ho capito, questa azione andrà distrutta subito... non possiamo dissociare gli eventi da qui, servirà un'altra azione apposita
         //REPLY: si, bisognerebbe fare una AzioneFinePartita in cui mettere il codice qui sotto 
        
            /* implemntare un if(partitaInCorso) do the following code
            foreach (Interfaccia.Bottone b in quantum.getGUI().Bottoni)
            {
                b.Click -= b.associatedEvent;
            }
            */
            Terminata = true;

        }
    }
}
