using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Quantum_Game.Interfaccia;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Schermate
{
    public class SchermataOpzioniPartita: Schermata
    {
        const int MAX_GIOCATORI = 6;

        int numeroDiGiocatori = 2;
        int idMappa = 0;
        int idMappaMax;
        string[] mappeTrovate;

        Dictionary< Bottone, e_color> coloriGiocatori;

        public SchermataOpzioniPartita (Quantum quantum): base (quantum)
        {
            // Cerco le mappe su disco
            mappeTrovate = Directory.GetFiles(@"Data Content\Mappe", "*.txt", SearchOption.TopDirectoryOnly);
            if (!mappeTrovate.Any()) throw new Exception("mappeTrovate");
            idMappaMax = mappeTrovate.Length;
            IdMappa = 0;

            // Resetto l'interfaccia
            GuiManager gui = quantum.getGUI();
            Riquadro.Main.Reset();

            coloriGiocatori = new Dictionary<Bottone, e_color>(MAX_GIOCATORI);

            #region Definizione Riquadri

            var _parteSinistra = Riquadro.Main.Colonna(60); var _parteDestra = Riquadro.Main.Colonna(40);

            // Costanti di posizionamento
            const float Y_RIGA_GRANDE = 15.2f;
            const float Y_RIGA_ = (100f - (Y_RIGA_GRANDE * 2)) / MAX_GIOCATORI;
            const float X_BOTT_COLOR = 100 / 6f;
            const float PAD_Y_LABEL = 2.5f;

            // PARTE SINISTRA

            var labelTitolo = _parteSinistra.Riga(Y_RIGA_GRANDE);

            // Spazio per il selettore del num giocatori
            var _nGiocatori = _parteSinistra.Riga(Y_RIGA_GRANDE);

            var labelNumeroGiocatori = _nGiocatori.Colonna(100 - X_BOTT_COLOR); var _selNGiocatori = _nGiocatori.Colonna(100);

            var bottonePiùGiocatori = _selNGiocatori.Riga(50);
            var bottoneMenoGiocatori = _selNGiocatori.Riga(50);

            // Spazio per selezione colore e nomi dei giocatori
            Riquadro[] _giocatori = new Riquadro[MAX_GIOCATORI];
            for (int i = 0; i < MAX_GIOCATORI; i++)
                _giocatori[i] = _parteSinistra.Riga(Y_RIGA_);

            // Bottoni colore
            Riquadro[] bottoniColore = new Riquadro[MAX_GIOCATORI];
            for (int i = 0; i < MAX_GIOCATORI; i++)
                bottoniColore[i] = _giocatori[i].Colonna (X_BOTT_COLOR, 2.5f, PAD_Y_LABEL);

            // textbox nomi dei giocatori
            Riquadro[] textboxNome = new Riquadro[MAX_GIOCATORI];
            for (int i = 0; i < MAX_GIOCATORI; i++)
                textboxNome[i] = _giocatori[i].Colonna(100, 5f, PAD_Y_LABEL);

            // PARTE DESTRA

            var tabellone = _parteDestra.Riga (100 - (Y_RIGA_GRANDE * 2) - Y_RIGA_);

            var _fileMappa = _parteDestra.Riga(Y_RIGA_GRANDE);
            var labelFileMappa = _fileMappa.Colonna(100 - X_BOTT_COLOR); var _selFileMappa = _fileMappa.Colonna(100);

            var bottonePiùMappa = _selFileMappa.Riga(50);
            var bottoneMenoMappa = _selFileMappa.Riga(50);

            var __ = _parteDestra.Riga(10f); // spazio vuoto intenzionale

            var _okCancel = _parteDestra.Riga(100);
            var bottoneAnnulla = _okCancel.Colonna(50);
            var bottoneOk = _okCancel.Colonna(50);

            #endregion Definizione Riquadri


            #region Instanziamento componenti

            Label Titolo = new Label(labelTitolo) { Caption = "Opzioni Nuova Partita" };

            /*-----------------------------------------------------------------------------------------------------*/

            Label NumeroGiocatori = new Label(labelNumeroGiocatori, "Numero Giocatori: " + numeroDiGiocatori);
            Bottone PiùGiocatori = new Bottone(bottone.più, bottonePiùGiocatori);
            Bottone MenoGiocatori = new Bottone(bottone.meno, bottoneMenoGiocatori);
            PiùGiocatori.Click += (s, e) => NumeroGiocatori.Caption = "Numero Giocatori: " + (++NumeroDiGiocatori);
            MenoGiocatori.Click += (s, e) => NumeroGiocatori.Caption = "Numero Giocatori: " + (--NumeroDiGiocatori);

            /*-----------------------------------------------------------------------------------------------------*/

            // COSTRUZIONE BOTTONCINI SCELTA COLORE
            Bottone[] colori = new Bottone[MAX_GIOCATORI]; Color coloreDefault;
            GestoreDiGiocatori.QuantumColor.TryGetValue(e_color.incolore, out coloreDefault);
            for (int i = 0; i < MAX_GIOCATORI; i++)
            {
                colori[i] = new Bottone(bottone.nessuno, bottoniColore[i]) { Colore = coloreDefault };
                colori[i].Click += sceltaColore;
                // Popolo un dizionario che associa ciascuno dei colori del gioco al bottone che è di quel colore
                coloriGiocatori.Add(colori[i], 0);
            }

            // COSTRUZIONE CASELLE TESTO NOME
            Textbox[] nomi = new Textbox[MAX_GIOCATORI];
            for (int i = 0; i < MAX_GIOCATORI; i++)
                nomi[i] = new Textbox(textboxNome[i], "Giocatore " + (i + 1));

            /*-----------------------------PARTE DESTRA -----------------------------------------------------------*/

            Tabellone anteprimaMappa = new Tabellone(quantum, tabellone);

            /*-----------------------------------------------------------------------------------------------------*/

            Label FileMappa = new Label(labelFileMappa, "Scegli la mappa");
            Bottone PiùMappa = new Bottone(bottone.più, bottonePiùMappa);
            Bottone MenoMappa = new Bottone(bottone.meno, bottoneMenoMappa);
            PiùMappa.Click += (s, e) => IdMappa++;
            MenoMappa.Click += (s, e) => IdMappa--;

            Bottone Annulla = new Bottone(bottone.Annulla, bottoneAnnulla);
            Bottone Ok = new Bottone(bottone.IniziaPartita, bottoneOk);
            Annulla.Click += (s, e) => quantum.schermateDiGioco.CaricaSchermata(new SchermataMenu(quantum));
            Ok.Click += (s, e) =>
            {
                quantum.schermateDiGioco.CaricaSchermata(new SchermataPartitaInCorso(quantum));
                quantum.getGestoreDiAzioni().ImpilaAzione(new Azioni.AzioneSetupPartitaOffLine(quantum, numeroDiGiocatori));
            };


            #endregion

            gui.Iscrivi(colori);
            gui.Iscrivi(nomi);
            gui.Iscrivi(Titolo, NumeroGiocatori, FileMappa, PiùMappa, PiùGiocatori, MenoMappa, MenoGiocatori, Ok, Annulla, anteprimaMappa);

        }


        void sceltaColore (object sender, EventArgs e)
        {
            var bottone = (Bottone)sender;
            e_color coloreAttuale;
            int numColori = Enum.GetValues(typeof(e_color)).Length;

            // trovo il colore che ha il bottone in questo momento

            coloriGiocatori.TryGetValue(bottone, out coloreAttuale);
            coloriGiocatori.Remove(bottone);

            do
            {
                coloreAttuale = (e_color)(((int)coloreAttuale + 1) % numColori);
            }
            while (coloriGiocatori.ContainsValue(coloreAttuale));

            coloriGiocatori.Add(bottone, coloreAttuale);
            Color nuovoColore;
            GestoreDiGiocatori.QuantumColor.TryGetValue(coloreAttuale, out nuovoColore);

            bottone.Colore = nuovoColore;
        }

        int IdMappa { get { return idMappa; }
        set
            {
                if (value >= idMappaMax) idMappa = 0;
                else if (value < 0) idMappa = idMappaMax - 1;
                else idMappa = value;
                var mapGenerator = new Mappa.MapGenerator(mappeTrovate[idMappa]);
                Tile.CreaMappa(mapGenerator.GeneraMappa(), mapGenerator.Righe, mapGenerator.Colonne);
            }
        }

        int NumeroDiGiocatori
        {
            get { return numeroDiGiocatori; }
            set
            {
                if (value > MAX_GIOCATORI) numeroDiGiocatori = 6;
                else if (value < 2) numeroDiGiocatori = 2;
                else numeroDiGiocatori = value;
            }
        }

        // Vecchio metodo
        void old ()
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
