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
        Widget colonizza;

        public SelezionePianeta (Game game): base (game)
        {
            ConsoleMessaggi.NuovoMessaggio("Selezionato Pianeta");
            _pianeta = (Pianeta)gui.Tabellone.TileClick;

            Vector2 pos = gui.Tabellone.Tile2Pixel(_pianeta);

            pos += new Vector2 (0, -gui.Tabellone.LatoCasella / 2);
            colonizza = new Widget(pos, widget.Colonizza, _pianeta.Colonizzabile (giocatoreDiTurno));

            colonizza.Click += completaAzione;
            gui.Iscrivi(colonizza);

            gui.Tabellone.ResetSelezioneMouse();
            gui.Tabellone.MostraSelezione = false;
        }
        public override bool Abort()
        {
            Cleanup();
            return true;
        }
        public override void Esegui()
        {
            if (ultimoClick != TipoEventoMouse.nessuno && gui.Tabellone.TileClick?.Equals(_pianeta)== false)
                Cleanup();
        }

        void completaAzione (object sender, EventArgs e)
        {
            _pianeta.Colonizza(giocatoreDiTurno);
            ConsoleMessaggi.NuovoMessaggio("Il giocatore " + giocatoreDiTurno.Colore + " ha fondato una colonia sul pianeta!");
            Cleanup();
        }

        protected override void Cleanup()
        {
            ConsoleMessaggi.NuovoMessaggio("Deselezionato Pianeta");

            colonizza.Click -= completaAzione;
            gui.RimuoviWidget();
            gui.Tabellone.ResetSelezioneMouse();
            AzioneSuccessiva = null;
        }

        Pianeta _pianeta;
    }
}
