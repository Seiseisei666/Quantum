using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game.Azioni
{
    public class AzionePiazzaNave : Azione
    {
        private Quantum quantum;
        private Giocatore giocatore;

        public AzionePiazzaNave(Quantum quantum, Giocatore giocatore)
        {
            this.quantum = quantum;
            this.giocatore = giocatore;
        }

        protected override void Esegui()
        {
            Casella tempCas = quantum.getGUI().Tabellone.TileClick as Casella; // prova a castare il tile selezionato come casella
            
            Nave naveTemp = giocatore.NaveDaPiazzare;
            if (naveTemp != null)
            {
                if (tempCas != null && tempCas.Occupante == null)
                {
                    naveTemp.Piazza(tempCas);
                    Terminata = true;
                }//riesegue l'azione fino a che non viene piazzata davvero una nave
            }
        }

        public override bool Abort() { return false; }
        protected override void Cleanup() { }
    }
}
