using System;

namespace Quantum_Game.Azioni
{
    public class FineTurno: AzioneDiGioco
    {
        /// <summary>
        /// Qui gestiamo (gestiremo...) la fine del turno, ovvero:
        /// Piazzare mentine se si raggiunge il massimo di Dominio;
        /// Scartare le carte di troppo;
        /// Varie ed eventuali
        /// </summary>
        public FineTurno (Microsoft.Xna.Framework.Game game) : base (game)
        {
        }

        public override void Esegui()
        {
            Cleanup();
        }
        public override bool Abort() { return false; }
        protected override void Cleanup()
        {
            AzioneSuccessiva = null;
            gestoreDiGiocatori.aggiornaGiocatoreDiTurno();
            Interfaccia.ConsoleMessaggi.NuovoMessaggio("Turno del giocatore" + gestoreDiGiocatori.getGiocatoreDiTurno().Colore, gestoreDiGiocatori.getGiocatoreDiTurno().SpriteColor);
            gui.Cimitero.Aggiorna(gestoreDiGiocatori.getGiocatoreDiTurno());
        }

    }
}
