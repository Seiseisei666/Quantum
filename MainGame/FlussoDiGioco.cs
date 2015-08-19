using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

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
        public FlussoDiGioco(Quantum game)
        {
            gameSystem = (GameSystem)game.GetGameObject(typeof(GameSystem));
            pathFinder = (PathFinder)game.GetGameObject(typeof(PathFinder));
            Gui = (GUI)game.GetGameObject(typeof(GUI));
            tabellone = Gui.tabellone;
            mouseInput = (MouseInput)game.GetGameObject(typeof(MouseInput)); 
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
        public void Update() // loop principale
        {
            if (gameSystem.FasePartita == FasiDiGioco.PartitaInCorso)
            {
                /* QUI C'E' LA PARTITA VERA E PROPRIA!!!
                ***************************************/
                checkFineTurno(); // Controlla se il giocatore può agire; in caso contrario finisce il turno ed esce da Update

                if (stato == Azione.nessuna)
                    checkSelezione();

                else if (stato == Azione.Movimento)
                    Movimento();

                else if (stato == Azione.SelezioneDx)
                    SelezioneDx();
            }

            else if (gameSystem.FasePartita == FasiDiGioco.SetupPartita)
                setupPartita();
        }

        // METODI PRIVATI
        void checkFineTurno() // controlla se è finito il turno
        {
            if (!giocatoreDiTurno.PuòAgire | Gui.BottonePremuto == bottone.Passa)   
            {
                gameSystem.NextTurn();
                Debug.WriteLine("Turno del giocatore {0}", gameSystem.GiocatoreDiTurno.Colore);
                return;
            }
        }

        void checkSelezione()   // aspetta una selezione valida
        {
            Nave nave = casellaCliccata?.Occupante ?? casellaCliccataDx?.Occupante;
            if (nave == null) return;

            else if 
                (clickSn && nave.Alleato(giocatoreDiTurno) && !nave.Mossa)
            {
                // Selezionata nave alleata e disponibile per il movimento
                _naveSel = nave;
                _casellaSel = casellaCliccata;
                pathFinder.Start(tabellone.TileClkSn, _naveSel);
                stato = Azione.Movimento;
            }
            else if 
                (clickDx && nave.Alleato(giocatoreDiTurno) &&
                (!nave.SpecialUsata || !nave.Riconfigurata))
            {
                _naveSel = nave;
                _casellaSel = casellaCliccataDx;
                stato = Azione.SelezioneDx;
            }

        }

        void Movimento()    // gestisce Attacco/Movimento una volta che è stata fatta una selezione valida
        {
            if (clickDx || !clickSn)
            {    // Click dx valido o click sn NON valido: deselezione
                Deseleziona();
                return;
            }
            
            _casellaTarget = casellaCliccata;
            int dist = _distanzaCasella;

            if (dist == 0 || dist > _naveSel.Pwr)
                return;

            Nave nave = _casellaTarget.Occupante;
            if (nave != null && 
                nave.Alleato(giocatoreDiTurno))
            {   
                // Combattimento

                bool RisultatoAttacco;
                Debug.WriteLine("Una nave {0} di colore {1} ha attaccato una nave {2} di colore {3}.",
                            _naveSel.Tipo, _naveSel.Colore, nave.Tipo, nave.Colore);

                RisultatoAttacco = _naveSel.Attacco(nave);
                Debug.WriteLine("risultato: {0}", RisultatoAttacco);

                if (RisultatoAttacco == true)
                    _naveSel.Muovi(_casellaSel, _casellaTarget);
            }
            else if (_casellaTarget != null)

            {
                // Movimento

                _naveSel.Muovi(_casellaSel, _casellaTarget);
            }

            // Fine procedura Movimento/Attacco
            Deseleziona();
            
        }
        void SelezioneDx()
        {
            if (!clickDx || clickSn)
            {    // Click dx valido o click sn NON valido: deselezione
                Deseleziona();
                return;
            }
            /* TODO:
            Mostrare il menù di selezione fra riconfigurazione e special;
            riconfigurazione -> si chiama il metodo e stop
            Special -> si entra in un altro blocco di codice per la gestione dello special
    */
        }

        void Deseleziona () // esce dalla routine di Attacco/Movimento
        {
            pathFinder.Clear();
            stato = Azione.nessuna;
            _naveSel = null;
            _casellaTarget = _casellaSel = null;
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
        private int _distanzaCasella
        {
            get
            {
                if (_casellaTarget != null)
                    return pathFinder.PercorsoXCasella(_casellaTarget).Length;
                else return 0;
            }
        }
            // bool che ci dicono se c'è stato un click destro o sinistro sul tabellone
        private bool clickDx { get { return casellaCliccataDx != null; } }
        private bool clickSn { get { return casellaCliccata != null; } }


        // CAMPI
        // oggetti di gioco a cui dobbiamo avere accesso
        private GameSystem gameSystem;
        private Tabellone tabellone;
        private MouseInput mouseInput;
        private PathFinder pathFinder;
        private GUI Gui;
            // stato del flusso di gioco
        private Azione stato;
            // Qui ci salviamo le selezioni compiute dall'utente
        private Nave _naveSel;  
        private Casella _casellaSel; // la casella attualmente selezionata
        private Casella _casellaTarget; // la casella obiettivo di attacco/movimento

       }
}

