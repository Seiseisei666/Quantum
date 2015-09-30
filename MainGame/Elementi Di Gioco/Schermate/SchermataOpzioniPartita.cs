using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Quantum_Game.Interfaccia;

namespace Quantum_Game.Schermate
{
    public class SchermataOpzioniPartita: Schermata
    {
        public SchermataOpzioniPartita (Quantum quantum): base (quantum)
        {
            GuiManager gui = quantum.getGUI();
            Riquadro.Main.Reset();
            Riquadro spazioMenu = Riquadro.Main.Colonna(55, PaddingTopBottom: 20);
            Riquadro spazioMappa = Riquadro.Main.Colonna(45, 20, 10);
            Riquadro barraLogo = spazioMenu.Riga(18);

            // Riquadro _vuoto = spazioMenu.Colonna(10);
            Riquadro vociMenu = spazioMenu.Colonna(100);
            Tabellone anteprimaMappa = new Tabellone(quantum, spazioMappa) { MostraSelezione = false };

            //
            // Pulsanti x selezione del numero di giocatori
            //

            Riquadro giocatori = vociMenu.Riga(15, 70, 5);
            var riquadroNumGioc = giocatori.Colonna(90, 5);
            var posPiuMeno = giocatori.Colonna(10, 5);
            var tastoPiù = posPiuMeno.Riga(50, 0, 5);
            var tastoMeno = posPiuMeno.Riga(50, 0, 5);

            int numeroDiGiocatori = 2; // per motivi difficilmente comprensibili questa variabile locale resta "intrappolata" in memoria finché non cambia la schermata

            Label labelNumGiocatori = new Label(riquadroNumGioc, "Numero Giocatori: " + numeroDiGiocatori);

            // Bottoni + e - con associati i metodi anonimi per farli funzionare
            Bottone più = new Bottone(bottone.più, tastoPiù);
            Bottone meno = new Bottone(bottone.meno, tastoMeno);

            più.Click += (s, e) =>
            {
                if (++numeroDiGiocatori > 6) numeroDiGiocatori = 2;
                labelNumGiocatori.Caption = "Numero Giocatori: " + numeroDiGiocatori;
            };

            meno.Click += (s, e) =>
            {
                if (--numeroDiGiocatori < 2) numeroDiGiocatori = 6;
                labelNumGiocatori.Caption = "Numero Giocatori: " + numeroDiGiocatori;
            };

            //
            // Pulsanti x selezione del file della mappa
            //

            Riquadro posizioneMappe = vociMenu.Riga(15, 70, 5);
            var riquadroFilename = posizioneMappe.Colonna(90, 5);
            var posPiuMeno2 = posizioneMappe.Colonna(10, 5);
            var tastoPiù2 = posPiuMeno2.Riga(50, 0, 5);
            var tastoMeno2 = posPiuMeno2.Riga(50, 0, 5);

            // variabili locali intrappolate
            string[] mappeTrovate = Directory.GetFiles(@"Data Content\Mappe", "*.txt", SearchOption.TopDirectoryOnly);
            if (!mappeTrovate.Any()) throw new Exception("mappeTrovate");
            int IDmappa = 0;
            int IDmappaMax = mappeTrovate.Length;

            // inizializzo la mappa
            var generator = new Mappa.MapGenerator(mappeTrovate[IDmappa]);
            Tile.CreaMappa(generator.GeneraMappa(), generator.Righe, generator.Colonne);

            Label labelFilenameMappe = new Label(riquadroFilename, mappeTrovate[0]);

            // Bottoni + e - con associati i metodi anonimi per farli funzionare

            Bottone più2 = new Bottone(bottone.più, tastoPiù2);
            Bottone meno2 = new Bottone(bottone.meno, tastoMeno2);

            più2.Click += (s, e) =>
            {
                labelFilenameMappe.Caption = mappeTrovate[++IDmappa >= IDmappaMax ? IDmappa = 0 : IDmappa];
                var mapGenerator = new Mappa.MapGenerator(mappeTrovate[IDmappa]);
                Tile.CreaMappa(mapGenerator.GeneraMappa(), mapGenerator.Righe, mapGenerator.Colonne);
            };
            meno2.Click += (s, e) =>
            {
                labelFilenameMappe.Caption = mappeTrovate[--IDmappa < 0 ? IDmappa = IDmappaMax - 1 : IDmappa];
                var mapGenerator = new Mappa.MapGenerator(mappeTrovate[IDmappa]);
                Tile.CreaMappa(mapGenerator.GeneraMappa(), mapGenerator.Righe, mapGenerator.Colonne);
            };


            // Bottone per iniziare la partita, con metodo anonimo per farlo funzionare
            Riquadro posIniziaPartita = vociMenu.Riga(15, 70, 5);
            Bottone NewGame = new Bottone(bottone.IniziaPartita, posIniziaPartita);
            NewGame.Click += (s, e) =>
            {
                quantum.schermateDiGioco.CaricaSchermata(new SchermataPartitaInCorso(quantum));
                quantum.getGestoreDiAzioni().ImpilaAzione(new Azioni.AzioneSetupPartitaOffLine(quantum, numeroDiGiocatori));
            };
            Textbox txt = new Textbox(spazioMenu); gui.Iscrivi(txt);
            // Ho fatto un nuovo overload di ManagerGui.Iscrivi che permette di condensare le iscrizioni in una sola riga :)
            gui.Iscrivi(più, meno, labelNumGiocatori, più2, meno2, labelFilenameMappe, NewGame);
            gui.Iscrivi(anteprimaMappa);
        }
    }
}
