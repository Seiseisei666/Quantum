using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Azioni
{
    public abstract class AzioneDiGioco
    {
        /// <summary>
        /// Il costruttore di questa classe base astratta va chiamato in tutte le classi derivate!!!
        /// Serve a copiarsi i riferimenti importanti
        /// </summary>
        /// <param name="game">Riferimento al nostro Quantum. Va passato per poter ottenere i servizi e i componenti del gioco</param>
        public AzioneDiGioco(Game game)
        {
            this.game = game;
            gameSystem = game.Services.GetService<GameSystem>();
            gui = game.Components.OfType<GUI>().First();
            AzioneSuccessiva = this;
        }
        /// <summary>
        /// Inizializzazione
        /// </summary>
        protected virtual void Inizializzazione()
        {
        }
        /// <summary>
        /// Il corpo dell'azione. E' astratto, per cui è obbligatorio implementarlo
        /// questa è l'unico metodo pubblico delle azioni
        /// </summary>
        public abstract void Esegui();

        /// <summary>
        /// Da chiamare quando l'azione è finita, per definire l'Azione Successiva e eventualmente per liberare eventuali risorse utilizzate
        /// </summary>
        protected virtual void Cleanup()
        {
            AzioneSuccessiva = null;
        }

        /// <summary>
        /// Ogni azione di gioco deve definire, alla fine del suo flusso, quale sarà l'azione successiva.
        /// Se null, l'azione successiva sarà un CheckSelezione
        /// </summary>
        public virtual AzioneDiGioco AzioneSuccessiva { get; protected set; }

        // Informazioni a disposizione che prendiamo da altri oggetti di gioco
        protected Giocatore giocatoreDiTurno { get { return gameSystem.GiocatoreDiTurno; } }
        protected Casella casellaCliccata { get { return gui.tabellone.TileClkSn as Casella; } }
        protected Casella casellaCliccataDx { get { return gui.tabellone.TileClkDx as Casella; } }
        protected bool clickDx { get { return casellaCliccataDx != null; } }
        protected bool clickSn { get { return casellaCliccata != null; } }
        protected TipoEventoMouse ultimoClick { get { return gui.tabellone.UltimoClick; } }

        // Oggetti di gioco che ci portiamo dietro
        protected Game game;
        protected GameSystem gameSystem;    // per accedere al giocatore di turno
        protected GUI gui;                  // per accedere alla grafica e ai click sul tabellone
    }
}
