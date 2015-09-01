using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Quantum_Game.Interfaccia;

namespace Quantum_Game.Azioni
{
    /* 
    Ereditiamo dalla classe astratta AzioneDiGioco per fare azioni che hanno un solo blocco di logica principale.
    Qua dentro, dopo l'inizializzazione, sono già disponibili le seguenti proprietà:

        giocatoreDiTurno { get { return gameSystem.GiocatoreDiTurno; } }
        casellaCliccata { get { return gui.tabellone.TileClick as Casella; } }
        ultimoClick { get { return gui.tabellone.UltimoClick; } }

        */
    public class EsempioAzione: AzioneDiGioco
    {
        public EsempioAzione (Game game): base (game)
            {

            /* 
            Qui si svolgono compiti di inizializzazione, da fare una tantum:
            scriversi su una variabile locale l'ultima casella cliccata, 
            oppure la nave o il pianeta selezionato, ecc
            Per esempio, qui creo un menù a tendina con 3 bottoni a caso 
            e lo iscrivo al manager Gui (che mi vado a pescare dai servizi del gioco):
            */

            gui = game.Services.GetService<Quantum_Game.Interfaccia.GuiManager>();

            Point puntoCliccato = gui.Tabellone.Tile2Pixel(casellaCliccata);

            MenuTendina menu = new MenuTendina (puntoCliccato, new bottone[] { bottone.UsaSpecial, bottone.Colonizza, bottone.Passa }, this);

            gui.Iscrivi(menu);

            /*
            Puntiamo inoltre AzioneSuccessiva su quest'istanza, in modo che al prossimo frame verrà chiamato il metodo Esegui
            di questa azione
            */

            AzioneSuccessiva = this;
            }


        public override void Esegui()
        {

          /*
          Loop dell'azione, chiamato ogni frame.
          Per azioni che hanno più blocchi di logica diversi c'è la classe astratta AzioneDiGiocoComplessa
          */
        }


        protected override void Cleanup()
        {
            /*
            Se abbiamo caricato risorse (bottoni, caselle illuminate, elementi gui, immaginine, testi, piselloni)
            qui dobbiamo fare in modo di eliminarle
            */

            gui.Rimuovi(this); // Rimuovo tutti i miei figli

            /*
            E' buona norma anche resettare la selezione del moutuse (per evitare che il prossimo CheckSelezione
            selezioni in automatico, di nuovo, l'ultima casella cliccata!
            */

            gui.Tabellone.ResetSelezioneMouse();

            /*
            Annullando AzioneSuccessiva, questa azione viene ufficialmente conclusa e al prossimo loop il FlussoDiGioco 
            sarà "resettato" su un'azione CheckSelezione
            */

            AzioneSuccessiva = null;

            // in alternativa possiamo chiamare un'ulteriore azione, per esempio:

            AzioneSuccessiva = new MovimentoAttacco(game);

        }


    }
}
