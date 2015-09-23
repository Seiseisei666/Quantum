using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game.Azioni
{
    /// <summary>
    /// Freeza il gioco in attesa che la nave argomento finisca la sua animazione
    /// dopodiché riprende il corso dell'azione di gioco che l'ha creata
    /// </summary>
    public class AttesaAnimazione : AzioneDiGioco
    {
        Nave _naveAnimata;
        AzioneDiGioco _azione;

        public AttesaAnimazione (Nave naveAnimata, AzioneDiGioco azione)
        {
            _naveAnimata = naveAnimata;
            _azione = azione;
        }

        public override bool Abort() { return false; }

        public override void Esegui()
        {
            if (_naveAnimata.Animazione == null)
                Cleanup();
        }

        protected override void Cleanup() { AzioneSuccessiva = _azione; }
    }
}
