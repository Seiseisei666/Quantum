using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Quantum_Game.Azioni
{
    public class AzioneSelezione: Azione
    {
        Quantum quantum;
        public AzioneSelezione(Quantum quantum)
        {
            this.quantum = quantum;
        }

        protected override void Esegui()
        {
            Giocatore giocatoreDiTurno = quantum.getGestoreDiGiocatori().getGiocatoreDiTurno();
            Casella casellaCliccata = quantum.getGUI().Tabellone.TileClick as Casella;
            var ultimoClick = quantum.getGUI().Tabellone.UltimoClick;
            Nave nave = casellaCliccata?.Occupante;

            if (!giocatoreDiTurno.PuòAgire)
            {
                quantum.getGestoreDiAzioni().ImpilaAzione(new AzioneFineTurno(quantum));
                return;
            }

            if (quantum.getGUI().Tabellone.TileClick as Pianeta != null)
            {

                quantum.getGestoreDiAzioni().ImpilaAzione(new AzioneSelezionePianeta(quantum, (Pianeta)quantum.getGUI().Tabellone.TileClick));
                    return;
            }


            if (nave != null)
            {

                if (ultimoClick == TipoEventoMouse.ClkSin && nave.Alleato(giocatoreDiTurno) && !nave.Mossa)
                {
                    quantum.getGestoreDiAzioni().ImpilaAzione(new AzioneMovimento(quantum, casellaCliccata, puòAttaccare: true));
                    Cleanup();
                    return;
                    // Questa istanza di AzioneSelezione non viene rimossa finché non finisce la partita;
                    // mettiamo le azioni ulteriori chiamate dal giocatore in pila
                }

                else if (ultimoClick == TipoEventoMouse.ClkDx && nave.Alleato(giocatoreDiTurno) && (!nave.SpecialUsata || !nave.Riconfigurata))
                {
                    quantum.getGestoreDiAzioni().ImpilaAzione(new AzioneSelezioneDestra(quantum, casellaCliccata));
                    Cleanup();
                    return;
                }
            }

            else if
                (quantum.getGUI().Cimitero.NaveSelezionata != null)
            {
                //AzioneSuccessiva = new GiocaDaRiserva(game, gui.Cimitero.NaveSelezionata);
                return;
            }
        }

        public override bool Abort() { Cleanup(); return true; }

        protected override void Cleanup() { quantum.getGUI().Tabellone.ResetSelezioneMouse(); }

    }
}

