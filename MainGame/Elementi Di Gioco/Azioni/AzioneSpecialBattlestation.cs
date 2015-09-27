using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantum_Game.Interfaccia;

namespace Quantum_Game.Azioni
{
    public class AzioneSpecialBattlestation: Azione
    {
        Quantum quantum;
        Casella posizione;
        Nave nave;

        public AzioneSpecialBattlestation (Quantum quantum, Casella posizioneIniziale)
        {
            this.quantum = quantum;
            this.posizione = posizioneIniziale;
            this.nave = posizione.Occupante;

            var bersagli = Tile.Tiles(
                (Tile t) =>
                {
                    var c = t as Casella;
                    return c?.Adiacente(posizione, false) == true && c?.Occupante?.Alleato(quantum.getGestoreDiGiocatori().getGiocatoreDiTurno()) == false;

                }).Select(c => c.ID).ToArray();

            quantum.getGUI().Tabellone.IlluminaCaselle(bersagli);
            quantum.getGUI().Tabellone.ResetSelezioneMouse();
            ConsoleMessaggi.NuovoMessaggio("Scegli l'avversario da attaccare");
        }

        protected override void Esegui()
        {
            var ultimoClick = quantum.getGUI().Tabellone.UltimoClick;
            Casella casellaCliccata = quantum.getGUI().Tabellone.TileClick as Casella;

            if (ultimoClick == TipoEventoMouse.ClkSin && casellaCliccata?.Occupante?.Alleato(quantum.getGestoreDiGiocatori().getGiocatoreDiTurno()) == false)
            {
                nave.Attacco(casellaCliccata, consumaMovimento: false);
                nave.UsaSpecial();
                Cleanup();
            }
            else if (ultimoClick != TipoEventoMouse.nessuno)
                Cleanup();
        }

        public override bool Abort() { Cleanup(); return true; }

        protected override void Cleanup()
        {
            var gui = quantum.getGUI();
            gui.Tabellone.ResetSelezioneMouse();
            gui.Tabellone.SpegniCaselle();
            Terminata = true;
        }
    }
}
