using Quantum_Game.Azioni;
using Quantum_Game.Interfaccia;
using System.Linq;
using System.Collections.Generic;

namespace Quantum_Game
{
    public class GestoreDiAzioni
    {
        /* coda di azioni da eseguire che viene gestita da questa classe */
        private Queue<Azione> azioniDaEseguire;

        public GestoreDiAzioni()
        {
            azioniDaEseguire = new Queue<Azione>();
        }

        /* Eseguo la prima azione, e se è terminata la cancello dalla coda*/
        private void EseguiAzione()
        {
            if (azioniDaEseguire.Any())
            {
                var azione = azioniDaEseguire.Peek();
                azione.Start();
                if (azione.Terminata)
                    azioniDaEseguire.Dequeue();
            }
        }

        /* aggiunge una azione da eseguire alla coda */
        public void IncodaAzione(Azione azione)
        {
            
            azioniDaEseguire.Enqueue(azione);
        }

        public void MettiAzioneInTesta(Azione azione)
        {
            var azioni = azioniDaEseguire;
            azioniDaEseguire = new Queue<Azione> (new Azione[] { azione }.Concat(azioni));
        }

        //metodo che controlla se ci sono nuove azioni ed, eventualmente, le esegue 
        public void Update()
        {
            EseguiAzione();
        }

        public int Count { get { return azioniDaEseguire.Count; } }
    }
}

