using System;
using Microsoft.Xna.Framework;
using Quantum_Game.Azioni;
using Quantum_Game.Interfaccia;
using System.Threading;

namespace Quantum_Game
{
    /// FlussoDiGioco è un superoggettone che riunisce tutti i pezzi del gioco e li mette insieme.
    /// Non so se è buona prassi fare così... ma intanto ci ho provato
    public class FlussoDiGioco: GameComponent
    {

        private Quantum _game;
        private AzioneDiGioco _prossimaAzione;
        private GuiManager gui;
        private GameSystem turno;

        public FlussoDiGioco(Quantum game): base(game)
        {
            _game = game;
            _prossimaAzione = null;
        }

        public override void Initialize()
        {
            turno = _game.Services.GetService<GameSystem>();
            gui = _game.Services.GetService<GuiManager>();

            turno.InizioPartita += inizioPartita;
        }

        public void inizioPartita (object sender, EventArgs e)
        {
            foreach (Bottone b in gui.Bottoni)
            {
                b.Click += bottoneCliccato;
            }
        }

        //metodo che controlla se ci sono nuove azioni ed, eventualmente, le esegue 
        public void Update() 
        {

            if (turno.FasePartita == FasiDiGioco.PartitaInCorso)
            {
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

            else if(turno.FasePartita == FasiDiGioco.SceltaOpzioni)
            {
                foreach (Bottone b in gui.Bottoni)
                {
                    if(b.TipoBottone == (bottone.IniziaPartita))
                        b.Click += onIniziaPartita;
                }
            }
        }

        void setupPartita() 
        {
            // TODO: facciamo una classe azione a parte anche per questa roba qui please?
            Casella tempCas = gui.Tabellone.TileClick as Casella; // prova a castare il tile selezionato come casella
            Nave naveTemp = turno.GiocatoreDiTurno.NaveDaPiazzare;
            if (naveTemp != null)
            {
                if (tempCas != null && tempCas.Occupante == null)
                    naveTemp.Piazza(tempCas);
            }
            else
                //quando nextTurn() viene chiamato finisce la fase di Setup
                turno.NextTurn();
        }

        // EventHandler per gestire il click dei bottoni
        void bottoneCliccato(object bott, EventArgs a)
        {
            
            Bottone b = (Bottone) bott;
            switch (b.TipoBottone)
            {
                case bottone.Passa:
                    if (_prossimaAzione.Abort()) _prossimaAzione = new FineTurno(_game);
                    break;
                case bottone.Ricerca:
                    if (_prossimaAzione.Abort())
                    {
                        turno.GiocatoreDiTurno.Ricerca();
                        turno.GiocatoreDiTurno.Azione();
                    }
                    break;
            }
        }

        void onIniziaPartita(object bott, EventArgs a)
        {
            //BUG: il metodo viene chiamato piu volte quando si clicca sul bottone. 
            if (turno.FasePartita == FasiDiGioco.SceltaOpzioni)
            {
                Interfaccia.ConsoleMessaggi.NuovoMessaggio("Setup partita in corso...");
                turno.IniziaSetupPartita();
                foreach (Bottone b in gui.Bottoni)
                {
                    if (b.TipoBottone == (bottone.IniziaPartita))
                    {
                        b.Click -= onIniziaPartita;
                        gui.Rimuovi(b);
                    }
                }
            }

        }
    }
}

