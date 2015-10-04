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
            var vociMenuDx = vociMenu.Colonna(50,20);
            var vociMenuDxLabel = vociMenuDx.Riga(20,0,15);
            var vociMenuDxRisol = vociMenuDx.Riga(40,25);

            var vociMenuSn = vociMenu.Colonna(50);


        // Pulsanti del menu

        var voce1 = vociMenuSn.Riga(15, 40, 5);
        var voce2 = vociMenuSn.Riga(15, 40, 5);
        var voce3 = vociMenuSn.Riga(15, 40, 5);
        var voce4 = vociMenuSn.Riga(15, 40, 5);


            Label Risoluzione = new Label(vociMenuDxLabel, "Risoluzione");
            GruppoBottoniRadio radioRisoluzione = new GruppoBottoniRadio(vociMenuDxRisol, true, risoluzioni.Keys.ToArray());

        Bottone Indietro = new Bottone(voce4, "Torna al menu principale");

            quantum.getGUI().Iscrivi(Risoluzione, radioRisoluzione, Indietro);

            radioRisoluzione.onValoreCambiato += (s, e) => RidimensionaSchermo(((Bottone)s).Caption, false);

            Indietro.Click += (s, e) => quantum.schermateDiGioco.CaricaSchermata(new SchermataMenu(quantum));

    }

        void RidimensionaSchermo (string risoluzione, bool fullscreen)
        {
            Tuple<int, int> ris;
            if (risoluzioni.TryGetValue(risoluzione, out ris))
                quantum.RidimensionaSchermo(ris.Item1, ris.Item2, false);
            else throw new Exception(risoluzione);
        }

        Dictionary<string, Tuple<int, int>> risoluzioni = new Dictionary<string, Tuple<int, int>>()
        {
            { "800 x 600", Tuple.Create (800,600) },
            { "1024 x 768", Tuple.Create (1024, 768) },
            { "1280 x 768", Tuple.Create (1280, 768) },
            { "1360 x 768", Tuple.Create (1360, 768) }
        };
    }
}
