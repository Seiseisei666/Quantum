using Quantum_Game.Azioni;
using System;

namespace Quantum_Game.Interfaccia
{
    /* Bottone specifico e personalizzabile a piacere per l'azione inizia partita.  */
    public class BottoneInizioPartitaOffLine : Bottone
    {
        private AzioneInizioPartitaOffLine iniziaPartita;

        public BottoneInizioPartitaOffLine(AzioneInizioPartitaOffLine iniziaPartita, bottone TipoBottone, Riquadro contenitore): base(TipoBottone, contenitore)
        {
            this.iniziaPartita = iniziaPartita;
        }
        // TODO: usare override invece di new quando bottone diventa abstract
        public new void associatedEvent(object bott, EventArgs a)
        {
            iniziaPartita.Start();
        }
    }
}
