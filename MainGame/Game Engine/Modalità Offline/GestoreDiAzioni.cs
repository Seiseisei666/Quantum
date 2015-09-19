using Quantum_Game.Azioni;
using Quantum_Game.Interfaccia;
using System.Linq;
using System.Collections.Generic;

namespace Quantum_Game
{
    public class GestoreDiAzioni
    {
        /* coda di azioni da eseguire che viene gestita da questa classe */
        private LinkedList<Azione> azioniDaEseguire;


        public GestoreDiAzioni()
        {
            azioniDaEseguire = new LinkedList<Azione>();
        }

        /* Eseguo la prima azione, e se è terminata la cancello dalla coda*/
        private void EseguiAzione()
        {
            if (azioniDaEseguire.Any())
            {
                var azione = azioniDaEseguire.First();
                azione.Start();
                if (azione.Terminata)
                    azioniDaEseguire.RemoveFirst();
            }
        }

        /* aggiunge una azione da eseguire alla coda */
        public void IncodaAzione(Azione azione)
        { 
            azioniDaEseguire.AddLast(azione);
        }

        public void MettiAzioneInTesta(Azione azione)
        {
            azioniDaEseguire.AddFirst(azione);
        }

        //metodo che controlla se ci sono nuove azioni ed, eventualmente, le esegue 
        public void Update()
        {
            EseguiAzione();
        }

        public int Count { get { return azioniDaEseguire.Count; } }
    }
}

