using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantum_Game.Interfaccia;

namespace Quantum_Game.Schermate
{
    public class SchermataMenu: Schermata
    {
        public SchermataMenu (Quantum quantum): base (quantum)
        {
            Riquadro.Main.Reset();
            // var = Riquadro
            // spazio superiore per mettere un logo
            var barraLogo = Riquadro.Main.Riga(18);

            var vociMenu = Riquadro.Main.Riga(100, PaddingTopBottom: 20);

            // Pulsanti del menu

            var voce1 = vociMenu.Riga(15, 70, 5);
            var voce2 = vociMenu.Riga(15, 70, 5);
            var voce3 = vociMenu.Riga(15, 70, 5);

            Bottone Opzioni = new Bottone(bottone.Opzioni, voce2);
            Bottone Credits = new Bottone(bottone.Credits, voce3);
            Bottone NewGame = new Bottone(bottone.IniziaPartita, voce1);
            NewGame.Click += (s, e) => quantum.schermateDiGioco.CaricaSchermata(new SchermataOpzioniPartita(quantum));
            // per chiarezza: (s, e) sono i parametri di questo EventHandler anonimo (object sender, EventArgs e); ma tanto non li usiamo

            quantum.getGUI().Iscrivi(NewGame, Opzioni, Credits);

        }
    }
}
