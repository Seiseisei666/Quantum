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

        int contatoreGiocatori = 2;
        int idMappa = 0;
        int idMappaMax;

        /// <summary>
        /// Tutti i file mappa trovati nella cartella del gioco
        /// </summary>
        string[] mappeTrovate;

        /// <summary>
        /// Mappa il colore di ciascun bottoncino, in modo da evitare che due giocatori possano avere lo stesso colore
        /// </summary>
        Dictionary< Bottone, e_color> coloriGiocatori;

        /// <summary>
        /// Bottoncini dei colori
        /// </summary>
        Bottone[] colori;

        /// <summary>
        /// Per inserire i nomi dei giocatori
        /// </summary>
        Textbox[] nomi;

        /// <summary>
        /// Scritta che riporta il numero di giocatori
        /// </summary>
        Label numeroGiocatori;

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

            var _parteSinistra = Riquadro.Main.Colonna(30,10,10); var _parteDestra = Riquadro.Main.Colonna(70,10,10);

            // Costanti di posizionamento
            const float Y_RIGA_GRANDE = 15.2f;
            const float Y_RIGA_ = (100f - (Y_RIGA_GRANDE * 2)) / MAX_GIOCATORI;
            const float X_BOTT_COLOR = 10f;
            const float PAD_Y_LABEL = 15f;

            // PARTE SINISTRA

            var labelTitolo = _parteSinistra.Riga(Y_RIGA_GRANDE);

            // Spazio per il selettore del num giocatori
            var _nGiocatori = _parteSinistra.Riga(Y_RIGA_GRANDE);

            var labelNumeroGiocatori = _nGiocatori.Colonna(70, 10); //var _selNGiocatori = _nGiocatori.Colonna(16);

            var bottoneMenoGiocatori = _nGiocatori.Colonna(10,10,33, forzaQuadrato: true);
            var bottonePiùGiocatori = _nGiocatori.Colonna(10,10,33, forzaQuadrato: true);

            // Spazio per selezione colore e nomi dei giocatori
            Riquadro[] _giocatori = new Riquadro[MAX_GIOCATORI];
            for (int i = 0; i < MAX_GIOCATORI; i++)
                _giocatori[i] = _parteSinistra.Riga(Y_RIGA_,5);

            // Bottoni colore
            Riquadro[] bottoniColore = new Riquadro[MAX_GIOCATORI];
            for (int i = 0; i < MAX_GIOCATORI; i++)
                bottoniColore[i] = _giocatori[i].Colonna (X_BOTT_COLOR,50, PAD_Y_LABEL, forzaQuadrato: true);

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
            var bottoneAnnulla = _okCancel.Colonna(50,15,20);
            var bottoneOk = _okCancel.Colonna(50,15,20);

            #endregion Definizione Riquadri


            #region Instanziamento componenti

            //TODO: mettere una scritta più carina, magari col logo
            Label Titolo = new Label(labelTitolo) { Caption = "Opzioni Nuova Partita" };

            /*-----------------------------------------------------------------------------------------------------*/

            numeroGiocatori = new Label(labelNumeroGiocatori, "Numero Giocatori: " + contatoreGiocatori);
            Bottone PiùGiocatori = new Bottone(bottonePiùGiocatori, "+");
            Bottone MenoGiocatori = new Bottone(bottoneMenoGiocatori, "-");
            PiùGiocatori.Click += (s, e) => ContatoreGiocatori++; //numeroGiocatori.Caption = "Numero Giocatori: " + NumeroDiGiocatori; };
            MenoGiocatori.Click += (s, e) => ContatoreGiocatori--; // numeroGiocatori.Caption = "Numero Giocatori: " + NumeroDiGiocatori; };

            /*-----------------------------------------------------------------------------------------------------*/

            // COSTRUZIONE INTERFACCIA PARTE SN - SCELTA COLORE E NOME GIOCATORE
            colori = new Bottone[MAX_GIOCATORI]; 
            nomi = new Textbox[MAX_GIOCATORI];

            Color coloreDefault;

            for (int i = 0; i < MAX_GIOCATORI; i++)
            {
                GestoreDiGiocatori.QuantumColor.TryGetValue((e_color)i+1, out coloreDefault);

                colori[i] = new Bottone(bottoniColore[i], " ") { Colore = coloreDefault, Enabled = i < 2 };
                colori[i].Click += sceltaColore;
                // Popolo un dizionario che associa ciascuno dei colori del gioco al bottone che è di quel colore
                coloriGiocatori.Add(colori[i], i < 2 ? (e_color)i + 1 : 0);

                // COSTRUZIONE CASELLE TESTO NOME
                nomi[i] = new Textbox(textboxNome[i], "Giocatore " + (i + 1)) { Enabled = i < 2 };
            }
            
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
            Ok.Click += (s, e) => InizioPartita();
            

            #endregion

            gui.Iscrivi(colori);
            foreach (Textbox n in nomi) gui.Iscrivi(n);
            gui.Iscrivi(Titolo, numeroGiocatori, FileMappa, PiùMappa, PiùGiocatori, MenoMappa, MenoGiocatori, Ok, Annulla, anteprimaMappa);

        }

        void sceltaColore (Bottone bottone)
        {
            e_color coloreAttuale;
            int numColori = Enum.GetValues(typeof(e_color)).Length;

            // trovo il colore che ha il bottone in questo momento

            coloriGiocatori.TryGetValue(bottone, out coloreAttuale);

            // Continuo a cambiarlo finché non ne trovo uno libero
            while (coloriGiocatori.ContainsValue(coloreAttuale) || coloreAttuale == e_color.incolore) 
            {
                coloreAttuale = (e_color)(((int)coloreAttuale + 1) % numColori);
            }

            // Aggiorno il dizionario
            coloriGiocatori[bottone] = coloreAttuale;
            System.Diagnostics.Debug.WriteLine(coloreAttuale.ToString());
            // Aggiorno la proprietà Colore del bottone
            Color nuovoColore;
            GestoreDiGiocatori.QuantumColor.TryGetValue(coloreAttuale, out nuovoColore);

            bottone.Colore = nuovoColore;
        }

        void sceltaColore (object sender, EventArgs e)
        {
            sceltaColore ((Bottone)sender);
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

        int ContatoreGiocatori
        {
            get { return contatoreGiocatori; }
            set
            {
                contatoreGiocatori = MathHelper.Clamp(value, 2, MAX_GIOCATORI);
                numeroGiocatori.Caption = "Numero Giocatori: " + contatoreGiocatori;
                for (int i = 0; i < MAX_GIOCATORI; i++)
                {
                    bool rigaAttiva = i < contatoreGiocatori;
                    nomi[i].Enabled = rigaAttiva;

                    Bottone b = colori[i];
                    if (rigaAttiva && !b.Enabled)
                    {
                        b.Enabled = true;
                        sceltaColore(b);
                    }

                    else if (!rigaAttiva && b.Enabled)
                    {
                        b.Enabled = false;
                        coloriGiocatori[b] = e_color.incolore;
                    }
                    
                }
            }
        }

        void InizioPartita()

        {
            quantum.schermateDiGioco.CaricaSchermata(new SchermataPartitaInCorso(quantum));
            var elencoGiocatoriDaCreare = new Dictionary<e_color, string>();

            for (int i = 0; i < contatoreGiocatori; i++)
            {
                e_color colore;
                coloriGiocatori.TryGetValue(colori[i], out colore);
                elencoGiocatoriDaCreare.Add(colore, nomi[i].Stringa);
            }

            quantum.getGestoreDiAzioni().ImpilaAzione(new Azioni.AzioneSetupPartitaOffLine(quantum, elencoGiocatoriDaCreare));

        }
    }
}
