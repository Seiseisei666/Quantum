using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Quantum_Game.Interfaccia;

namespace Quantum_Game.Azioni
{
    public class Special_Battlestation: AzioneDiGioco
    {
        public Special_Battlestation (Game game, Casella PosizioneNave) : base (game)
        {
            _posizione = PosizioneNave;
            _nave = _posizione.Occupante;

            var bersagli = Tile.Tiles(
                (Tile t) =>
                {
                    var c = t as Casella;
                    return c?.Adiacente(_posizione, false) == true && c?.Occupante?.Alleato(giocatoreDiTurno) == false;

                }).Select(c => c.ID).ToArray();

            gui.Tabellone.IlluminaCaselle(bersagli);
            gui.Tabellone.ResetSelezioneMouse();
            ConsoleMessaggi.NuovoMessaggio("Scegli l'avversario da attaccare");
        }

        public override void Esegui()
        {
            if (ultimoClick == TipoEventoMouse.ClkSin && casellaCliccata?.Occupante?.Alleato(giocatoreDiTurno) == false)
            {
                completaAzione();
            }
            else if (ultimoClick != TipoEventoMouse.nessuno)
                Cleanup();
        }

        void completaAzione()
        {
            _nave.Attacco(casellaCliccata);
            _nave.UsaSpecial();
            Cleanup();
        }

        protected override void Cleanup()
        {
            gui.Tabellone.ResetSelezioneMouse();
            gui.Tabellone.SpegniCaselle();
            AzioneSuccessiva = null;
        }

        Casella _posizione;
        Nave _nave;
    }
}
