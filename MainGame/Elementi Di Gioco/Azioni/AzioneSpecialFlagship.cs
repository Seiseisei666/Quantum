using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game.Azioni
{

    public class AzioneSpecialFlagship: Azione
    {
        Casella casellaFlagship, casellaAlleato;
        Nave flagship, naveTrasportata;
        Quantum quantum;
        ///<summary>il tipo Action è un puntatore ad un metodo void senza argomenti</summary> 
        Action fase;

        public AzioneSpecialFlagship (Quantum quantum, Casella casellaPartenza)
        {
            this.quantum = quantum;
            this.casellaFlagship = casellaPartenza;
            flagship = casellaPartenza.Occupante;

            // Selezione caselle da illuminare
            Func<Tile, bool> filtraAlleati = (t => t.PresenzaAlleata(flagship));

            int[] tiles = Tile.IdTiles(filtraAlleati);
            int[] caselle = tiles.Where(t => { return casellaFlagship.Adiacente(t, true); }).ToArray();
            
            quantum.getGUI().Tabellone.IlluminaCaselle(caselle);
            fase = sceltaAlleato;
            Interfaccia.ConsoleMessaggi.NuovoMessaggio("Clicca su un alleato adiacente");
        }

        protected override void Esegui()
        {
            fase.Invoke();
        }

        void sceltaAlleato()
        {
            Casella casellaCliccata = quantum.getGUI().Tabellone.TileClick as Casella;
            // Controllo se la casella cliccata è intorno alla mia nave e se c'è un alleato
            bool? alleato = casellaCliccata?.Occupante?.Alleato(quantum.getGestoreDiGiocatori().getGiocatoreDiTurno());
            bool? circostante = casellaCliccata?.Adiacente(casellaFlagship, compreseDiagonali: true);

            if (circostante == true && alleato == true)
            {
                // Selezionato alleato da trasportare
                casellaAlleato = casellaCliccata;
                naveTrasportata = casellaAlleato.Occupante;

                naveTrasportata.RimuoviDalGioco();
                fase = piazzaAlleato;
                quantum.getGestoreDiAzioni().ImpilaAzione(new AzioneMovimento(quantum, casellaFlagship, puòAttaccare: false));
            }
        }

        void piazzaAlleato()
        {
            // Mi assicuro che l'azione di movimento si sia conclusa con successo
            // Altrimento abortisco e rimetto le pedine a posto
            if (flagship.Mossa)
            {

                Casella casellaCliccata = quantum.getGUI().Tabellone.TileClick as Casella;

                bool libera = (casellaCliccata != null && casellaCliccata.Occupante == null);

                /* TODO: soluzione provvisoria in attesa di aggiungere le modifiche alle navi
                la flagship si è mossa, per cui NON SAPPIAMO DOVE SI TROVA e dobbiamo controllare tutte le caselle :(
                In futuro, forse, avremo accesso diretto alla casella su cui è posizionata la nave e ci potremo evitare questo giro */
                Tile posizione = Tile.Tiles (t => 
                {
                    Casella casella = t as Casella;
                    return casella?.Occupante?.Equals(flagship) == true;
                }
                ).First();
                /* fine parte provvisoria */

                Casella[] disponibili = 
                    Tile.Tiles ( tile =>
                        {
                            Casella casella = tile as Casella;
                            return casella != null && casella.Occupante == null && casella.Adiacente(posizione, compreseDiagonali: true);
                        })
                        .Select(tile => (Casella) tile) // converto da tile a caselle
                        .ToArray();                     // creo un array col risultato
                
                quantum.getGestoreDiAzioni().ImpilaAzione (new AzionePiazzaNave 
                    (quantum, naveTrasportata, disponibili));

                // La prossima volta che questa azione viene eseguita dalla pila, deve fare solo il cleanup
                fase = Cleanup;
            }
            else Abort();
        }

        public override bool Abort()
        {
            if (flagship.Mossa)
            {
                // HACK: per controllare se l'azione di movimento è stata compiuta, verifico se la flagship risulta mossa
                // potrebbe creare problemi con qualche carta strana, in futuro
                Interfaccia.ConsoleMessaggi.NuovoMessaggio("Impossibile annullare l'azione!!!", Microsoft.Xna.Framework.Color.Salmon);
                return false;
            }
            else
            {
                // Rimetto tutto a posto
                naveTrasportata.Piazza(casellaAlleato);
                Cleanup();
                return true;
            }
        }

        protected override void Cleanup()
        {
            quantum.getGUI().Tabellone.ResetSelezioneMouse();
            quantum.getGUI().Tabellone.SpegniCaselle();
            Terminata = true;
        }


    }
}
