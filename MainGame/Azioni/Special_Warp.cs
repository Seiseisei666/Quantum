using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Quantum_Game.Interfaccia;

namespace Quantum_Game.Azioni
{
    public class Special_Warp: AzioneDiGioco
    {
        public Special_Warp (Game game, Casella CasellaDiPartenza): base (game)
        {
            _partenza = CasellaDiPartenza;
            _naveMossa = CasellaDiPartenza.Occupante;

            var disponibili = 
                Tile.Tiles(t => t.PresenzaAlleata(giocatoreDiTurno)).Select(t => t.ID).ToArray();

            gui.Tabellone.IlluminaCaselle(disponibili);
            gui.Tabellone.ResetSelezioneMouse();
            ConsoleMessaggi.NuovoMessaggio("Scegli l'alleato con cui fare il warp:");

        }

        public override void Esegui()
        {
            if (ultimoClick == TipoEventoMouse.ClkSin && casellaCliccata?.PresenzaAlleata(giocatoreDiTurno) == true)
            {
                completaAzione();
            }
            else if (ultimoClick != TipoEventoMouse.nessuno)
                Cleanup();
        }

        void completaAzione()
        {
            _destinazioneWarp = casellaCliccata;
            _naveWarp = _destinazioneWarp.Occupante;
            _naveMossa.Piazza(_destinazioneWarp);
            _naveWarp.Piazza(_partenza);
            _naveMossa.UsaSpecial();
            Cleanup();
        }
        public override bool Abort() { Cleanup(); return true; }
        protected override void Cleanup()
        {
            gui.Tabellone.ResetSelezioneMouse();
            gui.Tabellone.SpegniCaselle();
            AzioneSuccessiva = null;
        }


        Casella _partenza, _destinazioneWarp;
        Nave _naveMossa, _naveWarp;
    }
}
