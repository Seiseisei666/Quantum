using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantum_Game.Interfaccia;

namespace Quantum_Game.Schermate
{
    public class SchermataPartitaInCorso: Schermata
    {
        public SchermataPartitaInCorso (Quantum quantum): base (quantum)
        {
            GuiManager gui = quantum.getGUI();
            Riquadro.Main.Reset();
            var barraSuperiore = Riquadro.Main.Riga(5);

            var riquadroTabellone = Riquadro.Main.Colonna(70);
            var tabellone = riquadroTabellone.Riga(100, 5, 5);

            var laterale = Riquadro.Main.Colonna(100, 5);

            var riquadroCimitero = laterale.Riga(50, 0, 10);
            var bott1 = laterale.Riga(10, 35, 5);
            var bott3 = laterale.Riga(10, 35, 5);
            var bott4 = laterale.Riga(10, 35, 5);
            var msg = laterale.Riga(100, 0, 15);

            Sfondo sfondo = new Sfondo(quantum);
            sfondo.DrawOrder = -1;
            quantum.Components.Add(sfondo);

            Bottone passaTurno = new Bottone(bottone.Passa, bott4);
            passaTurno.Click += (s, e) =>
            {
                if (quantum.getGestoreDiAzioni().AnnullaAzioneCorrente())
                    quantum.getGestoreDiAzioni().ImpilaAzione(new Azioni.AzioneFineTurno(quantum));
            };

            Bottone ricerca = new Bottone(bottone.Ricerca, bott3);
            ricerca.Click += (s, e) =>
            {
                if (quantum.getGestoreDiAzioni().AnnullaAzioneCorrente())
                {
                    Giocatore giocatore = quantum.getGestoreDiGiocatori().getGiocatoreDiTurno();
                    giocatore.Ricerca(); giocatore.Azione();
                }
            };

            ConsoleMessaggi console = new ConsoleMessaggi(msg);
            Tabellone tab2 = new Tabellone(quantum, tabellone);
            Cimitero cim = new Cimitero(quantum, riquadroCimitero);

            gui.Iscrivi(tab2);
            gui.Iscrivi(passaTurno, ricerca, console);
            gui.Iscrivi(cim);
        }
    }
}
