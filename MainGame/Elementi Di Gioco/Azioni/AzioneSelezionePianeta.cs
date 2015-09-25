using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Quantum_Game.Interfaccia;

namespace Quantum_Game.Azioni
{
    public class AzioneSelezionePianeta: Azione
    {
        Quantum quantum;
        Pianeta pianeta;
        Widget colonizza;

        public AzioneSelezionePianeta (Quantum quantum, Pianeta pianeta)
        {
            this.quantum = quantum;
            this.pianeta = pianeta;

            GuiManager gui = quantum.getGUI();

            Vector2 pos = gui.Tabellone.Tile2Pixel(pianeta);

            pos += new Vector2(gui.Tabellone.LatoCasella / 2, - gui.Tabellone.LatoCasella / 3);

            colonizza = new Widget(pos, widget.Colonizza, pianeta.Colonizzabile(quantum.getGestoreDiGiocatori().getGiocatoreDiTurno()));

            colonizza.Click += Colonizza;
            gui.Iscrivi(colonizza);
            gui.Tabellone.ResetSelezioneMouse();
            gui.Tabellone.MostraSelezione = false;
        }
        public override bool Abort()
        {
            Cleanup();
            return true;
        }
        protected override void Esegui()
        {
            if (quantum.getGUI().Tabellone.UltimoClick != TipoEventoMouse.nessuno)
                Cleanup();
        }

        void Colonizza(object sender, EventArgs e)
        {
            pianeta.Colonizza(quantum.getGestoreDiGiocatori().getGiocatoreDiTurno());
            ConsoleMessaggi.NuovoMessaggio("Il giocatore " + quantum.getGestoreDiGiocatori().getGiocatoreDiTurno().Colore + " ha fondato una colonia sul pianeta!");
            Cleanup();
        }

        protected override void Cleanup()
        {
            ConsoleMessaggi.NuovoMessaggio("Deselezionato Pianeta");

            colonizza.Click -= Colonizza;
            quantum.getGUI().RimuoviWidget();
            quantum.getGUI().Tabellone.ResetSelezioneMouse();
            Terminata = true;
        }
    }
}
