using Quantum_Game.Azioni;
using Quantum_Game.Interfaccia;
using System.Collections.Generic;

namespace Quantum_Game
{
    public class GestoreDiAzioni
    {
        /* coda di azioni da eseguire che viene gestita da questa classe */
        private List<Azione> azioniDaEseguire;

        public GestoreDiAzioni()
        {
            azioniDaEseguire = new List<Azione>();
        }
        /* se c'è almeno una azione in coda da eseguire, viene eseguita e cancellata dalla coda*/
        private void EseguiAzione()
        {
            if (azioniDaEseguire.Count > 0)
            {
                azioniDaEseguire[0].Start();
                azioniDaEseguire.RemoveAt(0);
            }
        }

        /* aggiunge una azione da eseguire alla coda */
        public void IncodaAzione(Azione azione)
        {
            
            azioniDaEseguire.Add(azione);
        }

        public void MettiAzioneInTesta(Azione azione)
        {
            azioniDaEseguire.Insert(0, azione);
        }

        //metodo che controlla se ci sono nuove azioni ed, eventualmente, le esegue 
        public void Update()
        {
            EseguiAzione();
        }

        public int Count()
        {
            return azioniDaEseguire.Count;
        }
    }
}

