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

            if (_caselleDisponibili.Any())
            {
                gui.Tabellone.IlluminaCaselle(_caselleDisponibili.Select(t => t.ID).ToArray());
                Interfaccia.ConsoleMessaggi.NuovoMessaggio("Scegli dove piazzare la nave");
                gui.Tabellone.ResetSelezioneMouse();
            }
            else
            {
                // abortiamo l'azione immediatamente, perché non ci sono caselle su cui piazzare la nave
                Interfaccia.ConsoleMessaggi.NuovoMessaggio("Impossibile piazzare la nave!", Color.Salmon);
                Cleanup();
            }
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
            gui.Cimitero.Aggiorna(giocatoreDiTurno); // Aggiornare il cimitero ne annulla la selezione e evita che si inneschi un loop infinito
            AzioneSuccessiva = null;
        }

        Tile[] _caselleDisponibili;
        Nave _nave;
    }
}
