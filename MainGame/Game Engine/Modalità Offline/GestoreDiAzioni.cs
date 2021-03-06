﻿using Quantum_Game.Azioni;
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
                if (azione.Terminata)
                    azioniDaEseguire.RemoveFirst();
                else azione.Start();
                
            }
        }

        /* aggiunge una azione da eseguire alla coda */
        public void IncodaAzione(Azione azione)
        { 
            azioniDaEseguire.AddLast(azione);

        }

        public void ImpilaAzione(Azione azione)
        {
            azioniDaEseguire.AddFirst(azione);
            stampaAzioni();
        }

        //metodo che controlla se ci sono nuove azioni ed, eventualmente, le esegue 
        public void Update()
        {
            EseguiAzione();
        }

        public bool AnnullaAzioneCorrente()
        {
            return azioniDaEseguire.First().Abort();
        }
        public int Count { get { return azioniDaEseguire.Count; } }

        void stampaAzioni () // X Debug
        {
            System.Diagnostics.Debug.WriteLine("STACK MODIFICATA");
            foreach (var a in azioniDaEseguire)
            {
                System.Diagnostics.Debug.WriteLine(a.ToString());
            }
        }
    }
}

