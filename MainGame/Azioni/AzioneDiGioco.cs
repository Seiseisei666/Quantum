using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Quantum_Game.Interfaccia;

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
            gui = game.Services.GetService<GuiManager>();
            AzioneSuccessiva = this;
        }
        #region Metodi Astratti
        /// <summary>Il corpo dell'azione. E' astratto, per cui è obbligatorio implementarlo.</summary>
        public abstract void Esegui();

        /// <summary>Interruzione improvvisa dell'azione. Restituisce True se l'azione può essere annullata. </summary>
        public abstract bool Abort();

        /// <summary>Da chiamare quando l'azione è finita, per definire l'Azione Successiva e eventualmente per liberare eventuali risorse utilizzate</summary>
        protected abstract void Cleanup();
        #endregion

        /// <summary>Ogni azione di gioco deve definire, alla fine del suo flusso, quale sarà l'azione successiva.
        /// Se null, l'azione successiva sarà un CheckSelezione </summary>
        public virtual AzioneDiGioco AzioneSuccessiva { get; protected set; }

        // Informazioni a disposizione che prendiamo da altri oggetti di gioco
        protected Giocatore giocatoreDiTurno { get { return gameSystem.GiocatoreDiTurno; } }
        protected Casella casellaCliccata { get { return gui.Tabellone.TileClick as Casella; } }
        protected TipoEventoMouse ultimoClick { get { return gui.Tabellone.UltimoClick; } }

        // Oggetti di gioco che ci portiamo dietro
        protected Game game;
        protected GameSystem gameSystem;    // per accedere al giocatore di turno
        protected GuiManager gui;      // per accedere alla grafica e ai click sul tabellone
    }
}
