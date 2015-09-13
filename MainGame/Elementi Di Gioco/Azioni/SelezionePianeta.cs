using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Quantum_Game.Interfaccia;
namespace Quantum_Game.Azioni
{
    public class SelezionePianeta: AzioneDiGioco
    {
        public SelezionePianeta (Game game): base (game)
        {
            ConsoleMessaggi.NuovoMessaggio("Selezionato Pianeta");
            _pianeta = gui.Tabellone.TileClick as Pianeta;
            if (ultimoClick == TipoEventoMouse.ClkDx)
            {
                MenuTendina menu = new MenuTendina(gui.Tabellone.Tile2Pixel(_pianeta), new bottone[] { bottone.Colonizza }, this);
                menu.Elementi[0].Enabled = _pianeta.Colonizzabile(giocatoreDiTurno);
                gui.Iscrivi(menu);
            }
            else
            {
                Bottone colonizza = Bottone.Standard(bottone.Colonizza, 82, 45, this);
                colonizza.Enabled = _pianeta.Colonizzabile(giocatoreDiTurno);
                gui.Iscrivi(colonizza);
            }

            gui.Tabellone.ResetSelezioneMouse();
        }
        public override bool Abort()
        {
            throw new NotImplementedException();
        }
        public override void Esegui()
        {
            if (gui.BottonePremuto == bottone.Colonizza)
            {
                _pianeta.Colonizza(giocatoreDiTurno);
                ConsoleMessaggi.NuovoMessaggio("Il giocatore " + giocatoreDiTurno.Colore + " ha fondato una colonia sul pianeta!");
                Cleanup();
            }

             
            
            if (ultimoClick != TipoEventoMouse.nessuno && gui.Tabellone.TileClick?.Equals((Tile)_pianeta)== false)
                Cleanup();


        }

        protected override void Cleanup()
        {
            ConsoleMessaggi.NuovoMessaggio("Deselezionato Pianeta");

            gui.Rimuovi(this);
            gui.Tabellone.ResetSelezioneMouse();
            AzioneSuccessiva = null;
        }

        Pianeta _pianeta;
    }
}
