using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Azioni
{
    public abstract class AzioneDiGioco
    {
        // Costruttore
        public AzioneDiGioco (Game game)
        {
            Completata = false;
            _game = game;
            gameSystem = game.Services.GetService<GameSystem>();
            gui = game.Components.OfType<GUI>().First();
            tabellone = gui.tabellone;
        }

        protected virtual void Inizializzazione()
        {
            AzioneSuccessiva = this;
        }

        public abstract void Esegui();

        protected virtual void Cleanup()
        {
            Completata = true;
        }

        public virtual AzioneDiGioco AzioneSuccessiva { get; protected set; }
        public bool Completata { get; protected set; }


        // Informazioni che otteniamo da altri oggetti di gioco
        protected Giocatore giocatoreDiTurno { get { return gameSystem.GiocatoreDiTurno; } }
        protected Casella casellaCliccata { get { return tabellone.TileClkSn as Casella; } }
        protected Casella casellaCliccataDx { get { return tabellone.TileClkDx as Casella; } }
        protected bool clickDx { get { return casellaCliccataDx != null; } }
        protected bool clickSn { get { return casellaCliccata != null; } }

        // Oggetti di gioco che ci interessano
        protected Game _game;
        protected GameSystem gameSystem;
        protected GUI gui;
        protected Tabellone tabellone;


    }
}
