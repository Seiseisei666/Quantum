using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Quantum_Game.Azioni;

namespace Quantum_Game
{
    public enum Azione
    {
        nessuna,
        Movimento,
        SelezioneDx,
        // da aggiungere man mano che implementiamo le azioni!!
    }

    /// <summary>
    /// FlussoDiGioco è un superoggettone che riunisce tutti i pezzi del gioco e li mette insieme.
    /// Non so se è buona prassi fare così... ma intanto ci ho provato
    /// </summary>
    public class FlussoDiGioco
    {
        // COSTRUTTORE
        public FlussoDiGioco(Game game)
        {
            _game = game;

            gameSystem = game.Services.GetService<GameSystem>();
            pathFinder = game.Components.OfType<PathFinder>().First();
            Gui = game.Components.OfType<GUI>().First();
            tabellone = Gui.tabellone;

            _prossimaAzione = null;
            stato = Azione.nessuna;
        }

        // PROPRIETA' PUBBLICHE

        public object OggettoSelezionato
        {
            get
            {
                Casella cas = tabellone.TileClkSn as Casella;
                Pianeta pian = tabellone.TileClkSn as Pianeta;

                if (cas != null && cas.Occupante != null)
                    return cas.Occupante;
                else if (pian != null)
                    return pian;
                else return null;
            }
        } // Riferimento all'oggetto selezionato, così che oggetti esterni possano "visualizzarlo"

        // METODI PUBBLICI
        public void Initialize()
        {

        }
        public void Update() // loop principale
        {
            if (gameSystem.FasePartita == FasiDiGioco.PartitaInCorso)
            {
                /* QUI C'E' LA PARTITA VERA E PROPRIA!!!
                ***************************************/

                if (!giocatoreDiTurno.PuòAgire || Gui.BottonePremuto == bottone.Passa)
                    _prossimaAzione = new FineTurno(_game);
                
                if (_prossimaAzione != null)
                {
                    _prossimaAzione.Esegui();
                    _prossimaAzione = _prossimaAzione.AzioneSuccessiva;
                }
                else
                    _prossimaAzione = new CheckSelezione(_game);
                    
           

            }

            else if (gameSystem.FasePartita == FasiDiGioco.SetupPartita)
                setupPartita();
        }

        void setupPartita() // loop della fase di setup della partita
        {
            Casella tempCas = tabellone.TileClkSn as Casella; // prova a castare il tile selezionato come casella
            Nave naveTemp = gameSystem.GiocatoreDiTurno.NaveDaPiazzare;
            if (naveTemp != null)
            {
                if (tempCas != null && tempCas.Occupante == null)
                    naveTemp.Piazza(tempCas);
            }
            else
                gameSystem.NextTurn();
        }


        // PROPRIETA' PRIVATE
        private Giocatore giocatoreDiTurno { get { return gameSystem.GiocatoreDiTurno; } }
        private Casella casellaCliccata { get { return tabellone.TileClkSn as Casella; } }
        private Casella casellaCliccataDx { get { return tabellone.TileClkDx as Casella; } }
        // distanza della casella su cui si prova a muovere /attaccare


        private AzioneDiGioco _prossimaAzione;




        // CAMPI
        // oggetti di gioco a cui dobbiamo avere accesso
        private Game _game;
        private GameSystem gameSystem;
        private Tabellone tabellone;
        private PathFinder pathFinder;
        private GUI Gui;
            // stato del flusso di gioco
        private Azione stato;
            // Qui ci salviamo le selezioni compiute dall'utente
        Nave _naveSel;  
        Casella _casellaSel; // la casella attualmente selezionata
        Casella _casellaTarget; // la casella obiettivo di attacco/movimento

       }
}

