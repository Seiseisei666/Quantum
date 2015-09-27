using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Quantum_Game.Azioni
{
    public class AzionePiazzaNave : Azione
    {
        private Quantum quantum;
        private Casella[] caselleDisponibili;
        private Nave naveDaPiazzare;
        /// <summary>
        /// Determina se la nave deve assolutamente essere piazzata, oppure se si può annullare
        /// </summary>
        bool obbligatoria = true;
        /// <summary>
        /// Determina se piazzare le nave consuma un punto azione del giocatore
        /// </summary>
        bool consumaAzione = false;

        /// <summary>
        /// Costruttore statico per il setup iniziale
        /// </summary>
        public static Azione SetupPartita(Quantum quantum, Nave nave)
        {
            return new AzionePiazzaNave(quantum, nave) { obbligatoria = true, consumaAzione = false };
        }
        /// <summary>
        /// Costruttore statico per piazzare navi dal cimitero
        /// </summary>
        public static Azione DaRiserva(Quantum quantum, Nave nave)
        {
            return new AzionePiazzaNave(quantum, nave) { obbligatoria = false, consumaAzione = true };
        }
        // Costruttore PRIVATO semplificato (le caselle disponibili sono quelle intorno ad un pianeta colonizzato)
        private AzionePiazzaNave(Quantum quantum, Nave nave) :
            this (quantum, nave, Tile.Tiles(tile =>
            {
                Casella casella = tile as Casella;
                return
                casella?.PianetaPiùVicino(compreseDiagonali: true)?.PresenzaAlleata(nave) == true
                && casella.Occupante == null;
            }).Select(t => (Casella)t).ToArray())
        { }        

        /// <summary>
        /// Azione per piazzare una nave dova clicca il giocatore
        /// </summary>
        public AzionePiazzaNave (Quantum quantum, Nave naveDaPiazzare, Casella[] caselleDisponibili)
        {
            // Piazzo una Nave in una casella arbitraria fra quelle dell'argomento
            this.quantum = quantum;
            this.caselleDisponibili = caselleDisponibili;
            this.naveDaPiazzare = naveDaPiazzare;
            quantum.getGUI().Tabellone.ResetSelezioneMouse();
            if (!caselleDisponibili.Any())
            {
                Interfaccia.ConsoleMessaggi.NuovoMessaggio("Impossibile piazzare la nave!", Color.Salmon);
                Cleanup();
            }
        }


        protected override void Esegui()
        {
            // SEMI-BUG: chiamiamo IlluminaCaselle ogni frame...
            // Soluzione rapida: spostare la riga seguente nel costruttore
            // ma col sistema dell'incodamento di azioni non funziona bene...
            // Soluzione ema: rinunciamo all'incodamento di azioni =P tanto lo usiamo solo nel setup iniziale!
            quantum.getGUI().Tabellone.IlluminaCaselle(caselleDisponibili.Select(cas => cas.ID).ToArray());
            Casella casella = quantum.getGUI().Tabellone.TileClick as Casella; // prova a castare il tile selezionato come casella
            Nave nave = naveDaPiazzare;

            if (nave != null)
            {
                if (caselleDisponibili.Contains(casella) && casella.Occupante == null)
                {
                    nave.Piazza(casella);
                    if (consumaAzione) quantum.getGestoreDiGiocatori().getGiocatoreDiTurno().Azione();
                    Cleanup();
                }
                // Se l'azione non era obbligatoria (come quando gioco dal cimitero) un click non valido annulla tutto
                else if (!obbligatoria && quantum.getGUI().Tabellone.UltimoClick != TipoEventoMouse.nessuno)
                    Cleanup();
            }
        }

        public override bool Abort()
        {
            if (obbligatoria) return false;
            else
            {
                Cleanup();
                return true;
            }
        }

        protected override void Cleanup()
        {
            // annullo la selezione sul tabellone e spengo le caselle
            quantum.getGUI().Tabellone.ResetSelezioneMouse();
            quantum.getGUI().Tabellone.SpegniCaselle();
            // aggiorno il cimitero, per sicurezza nel caso sia stata rimessa in gioco una nave
            quantum.getGUI().Cimitero.Aggiorna();
            Terminata = true;
        }
    }
}
