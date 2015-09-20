using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game.Azioni
{
    public class AzioneFineTurno : Azione
    {
        Quantum quantum;

        public AzioneFineTurno (Quantum quantum)
        {
            this.quantum = quantum;
        }

        protected override void Esegui()
        {
            Cleanup();
        }

        public override bool Abort() { return false; }

        protected override void Cleanup()
        {
            var gestoreDiGiocatori = quantum.getGestoreDiGiocatori();

            gestoreDiGiocatori.aggiornaGiocatoreDiTurno();
            Interfaccia.ConsoleMessaggi.NuovoMessaggio("Turno del giocatore" + gestoreDiGiocatori.getGiocatoreDiTurno().Colore, gestoreDiGiocatori.getGiocatoreDiTurno().SpriteColor);
            quantum.getGUI().Cimitero.Aggiorna(gestoreDiGiocatori.getGiocatoreDiTurno());

            Terminata = true;
        }


      
    }
}
