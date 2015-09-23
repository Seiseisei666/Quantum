using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game.Azioni
{
    public class AzioneSpecialDestroyer: Azione
    {
        Casella _partenza, _destinazioneWarp;
        Nave _naveMossa, _naveWarp;
        Quantum quantum;

        public AzioneSpecialDestroyer (Quantum quantum, Casella casellaPartenza)
        {
            _partenza = casellaPartenza;
            this.quantum = quantum;
            _naveMossa = casellaPartenza.Occupante;

            int[] disponibili =
                Tile.Tiles(t => t.PresenzaAlleata(quantum.getGestoreDiGiocatori().getGiocatoreDiTurno())).Select(t => t.ID).ToArray();

            quantum.getGUI().Tabellone.IlluminaCaselle(disponibili);
            quantum.getGUI().Tabellone.ResetSelezioneMouse();
            Interfaccia.ConsoleMessaggi.NuovoMessaggio("Scegli l'alleato con cui fare il warp:");
        }

        protected override void Esegui()
        {
            Tabellone tab = quantum.getGUI().Tabellone;
            Casella casellaCliccata = tab.TileClick as Casella;
            var ultimoClick = tab.UltimoClick;

            if (ultimoClick == TipoEventoMouse.ClkSin && casellaCliccata?.PresenzaAlleata(quantum.getGestoreDiGiocatori().getGiocatoreDiTurno()) == true)
            {
                _destinazioneWarp = casellaCliccata;
                _naveWarp = _destinazioneWarp.Occupante;
                _naveMossa.Piazza(_destinazioneWarp);
                _naveWarp.Piazza(_partenza);
                _naveMossa.UsaSpecial();
                Cleanup();
            }
            else if (ultimoClick != TipoEventoMouse.nessuno)
                Cleanup();
        }

        public override bool Abort() { Cleanup(); return true; }

        protected override void Cleanup()
        {
            quantum.getGUI().Tabellone.ResetSelezioneMouse();
            quantum.getGUI().Tabellone.SpegniCaselle();
            Terminata = true;
        }
    }
}
