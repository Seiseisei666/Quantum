using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Quantum_Game.Azioni;
using Quantum_Game.Interfaccia;

namespace Quantum_Game
{
    /// <summary>
    /// FlussoDiGioco è un superoggettone che riunisce tutti i pezzi del gioco e li mette insieme.
    /// Non so se è buona prassi fare così... ma intanto ci ho provato
    /// </summary>
    public class FlussoDiGioco: GameComponent
    {
        // COSTRUTTORE
        public FlussoDiGioco(Game game): base(game)
        {
            _game = game;
            _prossimaAzione = null;
        }

        public override void Initialize()
        {
            turno = _game.Services.GetService<ITurnazione>();
            gui = _game.Services.GetService<GuiManager>();
        }

        // METODI PUBBLICI

        public void Update() // loop principale
        {
            if (turno.FasePartita == FasiDiGioco.PartitaInCorso)
            {
                /* QUI C'E' LA PARTITA VERA E PROPRIA!!!
                ***************************************/

                
                if (_prossimaAzione != null)
                {
                    _prossimaAzione.Esegui();
                    _prossimaAzione = _prossimaAzione.AzioneSuccessiva;
                }
                else
                    _prossimaAzione = new CheckSelezione(_game);
                    
           

            }

            else if (turno.FasePartita == FasiDiGioco.SetupPartita)
                setupPartita();
        }

        void setupPartita() // loop della fase di setup della partita
        {
            Casella tempCas = gui.Tabellone.TileClick as Casella; // prova a castare il tile selezionato come casella
            Nave naveTemp = turno.GiocatoreDiTurno.NaveDaPiazzare;
            if (naveTemp != null)
            {
                if (tempCas != null && tempCas.Occupante == null)
                    naveTemp.Piazza(tempCas);
            }
            else
                turno.NextTurn();
        }

        // CAMPI
        // oggetti di gioco a cui dobbiamo avere accesso
        private Game _game;
        private AzioneDiGioco _prossimaAzione;
        private GuiManager gui;
        private ITurnazione turno;
       }
}

