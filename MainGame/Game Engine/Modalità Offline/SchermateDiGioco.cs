using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantum_Game.Interfaccia;
using System.IO;

namespace Quantum_Game.Schermate
{
    /// <summary>
    /// Al momento ogni metodo di questa classe rappresenta una pagina del gioco: sia il menù che il tabellone
    /// Utilizza una cosa matta di metodi anonimi per mantenere in memoria le informazioni visualizzate sulla schermata
    /// Ovviamente non è pratico, anche se funziona
    /// TODO: riorganizzare per bene questa parte (dividerla in più classi per esempio)
    /// </summary>
    public class SchermateDiGioco
    {
        Quantum quantum;
        public SchermateDiGioco(Quantum quantum)
        {
            this.quantum = quantum;
        }

        /// <summary>
        /// Crea la pagina del gioco vero e proprio, eg il tabellone e tutto l'ambaradan
        /// </summary>
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

            Bottone passaTurno = new Bottone(bottone.Passa, bott4);
            passaTurno.Click += (s, e) =>
            {
                if (quantum.getGestoreDiAzioni().AnnullaAzioneCorrente())
                    quantum.getGestoreDiAzioni().ImpilaAzione(new Azioni.AzioneFineTurno(quantum));
            };
            Bottone ricerca = new Bottone(bottone.Ricerca, bott3);
            ricerca.Click += (s,e) =>
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

        /// <summary>
        /// Crea la pagina del menu principale
        /// </summary>
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

            Bottone Opzioni = new Bottone(bottone.Opzioni, voce2);
            Bottone Credits = new Bottone(bottone.Credits, voce3);
            Bottone NewGame = new Bottone(bottone.IniziaPartita, voce1);
            NewGame.Click += (s, e) => OpzioniPartita();
            // per chiarezza: (s, e) sono i parametri di questo EventHandler anonimo (object sender, EventArgs e); ma tanto non li usiamo

            gui.Iscrivi(NewGame, Opzioni, Credits);

        }

        /// <summary>
        /// Crea la pagina di selezione numero giocatori e mappa
        /// </summary>
        public void OpzioniPartita()
        {
            GuiManager gui = quantum.getGUI();
            Riquadro.Main.Reset();
            Riquadro spazioMenu = Riquadro.Main.Colonna(55, PaddingTopBottom:20);
            Riquadro spazioMappa = Riquadro.Main.Colonna(45,20,10);
            Riquadro barraLogo = spazioMenu.Riga(18);
            
               // Riquadro _vuoto = spazioMenu.Colonna(10);
                Riquadro vociMenu = spazioMenu.Colonna(100);
            anteprimaMappa = new Tabellone(quantum, spazioMappa) { MostraSelezione = false };

            //
            // Pulsanti x selezione del numero di giocatori
            //

            Riquadro giocatori = vociMenu.Riga(15,70, 5);
                    var riquadroNumGioc = giocatori.Colonna(90, 5);
                    var posPiuMeno = giocatori.Colonna(10, 5);
                    var tastoPiù = posPiuMeno.Riga(50,0,5);
                    var tastoMeno = posPiuMeno.Riga(50,0,5);

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
                SchermataPartita();
                quantum.getGestoreDiAzioni().ImpilaAzione(new Azioni.AzioneSetupPartitaOffLine(quantum, numeroDiGiocatori));
            };

            // Ho fatto un nuovo overload di ManagerGui.Iscrivi che permette di condensare le iscrizioni in una sola riga :)
            gui.Iscrivi(più, meno, labelNumGiocatori, più2, meno2, labelFilenameMappe, NewGame);
            gui.Iscrivi(anteprimaMappa);
        }
        Tabellone anteprimaMappa;
    }
}
