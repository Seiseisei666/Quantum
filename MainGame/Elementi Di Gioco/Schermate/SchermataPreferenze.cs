using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantum_Game.Interfaccia;

namespace Quantum_Game.Schermate
{
    public class SchermataPreferenze: Schermata
    {
        public SchermataPreferenze (Quantum quantum): base (quantum)
        {
            Riquadro.Main.Reset();
            // var = Riquadro
            // spazio superiore per mettere un logo
            var barraLogo = Riquadro.Main.Riga(18);

        var vociMenu = Riquadro.Main.Riga(100, PaddingTopBottom: 20);

        // Pulsanti del menu

        var voce1 = vociMenu.Riga(15, 75, 5);
        var voce2 = vociMenu.Riga(15, 75, 5);
        var voce3 = vociMenu.Riga(15, 75, 5);
        var voce4 = vociMenu.Riga(15, 75, 5);

        Bottone Opzioni = new Bottone(voce2, "Preferenze");
        Bottone Credits = new Bottone(bottone.Credits, voce3);
        Bottone NewGame = new Bottone(bottone.IniziaPartita, voce1);
        Bottone Esci = new Bottone(voce4, "Esci dal gioco");
        NewGame.Click += (s, e) => quantum.schermateDiGioco.CaricaSchermata(new SchermataOpzioniPartita(quantum));
            // per chiarezza: (s, e) sono i parametri di questo EventHandler anonimo (object sender, EventArgs e); ma tanto non li usiamo

            Esci.Click += (s, e) => quantum.Exit();

            quantum.getGUI().Iscrivi(NewGame, Opzioni, Credits, Esci);

    }
}
}
