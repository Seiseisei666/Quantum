using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Azioni
{
    public class GiocaDaRiserva: AzioneDiGioco
    {
        public GiocaDaRiserva(Game game, Nave nave) : base(game)
        {
            _nave = nave;
            _caselleDisponibili = Tile.Tiles(t =>
            {
            Casella c = t as Casella;
            return t.PianetaPiùVicino(false)?.PresenzaGiocatore(giocatoreDiTurno) == true && c?.Occupante == null;
            });
            gui.Tabellone.IlluminaCaselle(_caselleDisponibili.Select(t => t.ID).ToArray());
            Interfaccia.ConsoleMessaggi.NuovoMessaggio("Scegli dove piazzare la nave");
            gui.Tabellone.ResetSelezioneMouse();
        }

        public override void Esegui()
        {
            if (ultimoClick == TipoEventoMouse.ClkSin && casellaCliccata != null)
            {
                if (_caselleDisponibili.Contains(casellaCliccata)) completaAzione();
            }

            else if (ultimoClick != TipoEventoMouse.nessuno) Cleanup();
        }

        void completaAzione()
        {
            _nave.Piazza(casellaCliccata);
            gui.Cimitero.Aggiorna(giocatoreDiTurno);
            giocatoreDiTurno.Azione();
            Cleanup();
        }

        protected override void Cleanup()
        {
            gui.Tabellone.SpegniCaselle();
            gui.Tabellone.ResetSelezioneMouse();
            AzioneSuccessiva = null;
        }

        Tile[] _caselleDisponibili;
        Nave _nave;
    }
}
