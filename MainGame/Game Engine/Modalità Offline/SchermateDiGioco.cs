using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantum_Game.Interfaccia;

namespace Quantum_Game.Schermate
{
    public class SchermateDiGioco
    {
        Quantum quantum;
        public SchermateDiGioco(Quantum quantum)
        {
            this.quantum = quantum;
        }

        public void SchermataPartita()

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

            Tabellone tab2 = new Tabellone(quantum, tabellone);


            Bottone passaTurno = new Bottone(bottone.Passa, bott4);
            Bottone ricerca = new Bottone(bottone.Ricerca, bott3);

            gui.Iscrivi(passaTurno);
            gui.Iscrivi(ricerca);

            gui.Iscrivi(tab2);


            ConsoleMessaggi console = new ConsoleMessaggi(msg);
            gui.Iscrivi(console);
            Cimitero cim = new Cimitero(quantum, riquadroCimitero);
            gui.Iscrivi(cim);

        }

        public void MenuPrincipale()
        {

            GuiManager gui = quantum.getGUI();
            Riquadro.Main.Reset();
            // var = Riquadro
            // spazio superiore per mettere un logo
            var barraLogo = Riquadro.Main.Riga(18);

            var vociMenu = Riquadro.Main.Riga(100, PaddingTopBottom: 20);

            // Pulsanti del menu

            var voce1 = vociMenu.Riga(15, 70, 5);
            var voce2 = vociMenu.Riga(15, 70, 5);
            var voce3 = vociMenu.Riga(15, 70, 5);

            Bottone NewGame = new Bottone(bottone.IniziaPartita, voce1);
            Bottone Opzioni = new Bottone(bottone.Opzioni, voce2);
            Bottone Credits = new Bottone(bottone.Credits, voce3);

            gui.Iscrivi(NewGame, Opzioni, Credits);

            EventHandler iniziaPartita = null;
            iniziaPartita = (object sender, EventArgs e) =>
            {
                SchermataPartita();
                quantum.getGestoreDiAzioni().ImpilaAzione(new Azioni.AzioneSetupPartitaOffLine(quantum, 2));
                NewGame.Click -= iniziaPartita;
            };
            NewGame.Click += iniziaPartita;


        }
    }
}
