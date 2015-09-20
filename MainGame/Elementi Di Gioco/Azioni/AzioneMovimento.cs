using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game.Azioni
{
    public class AzioneMovimento: Azione
    {
        Casella casellaPartenza, casellaTarget;
        Nave naveMossa;
        Quantum quantum;
        PathFinder pathFinder;

        public AzioneMovimento (Quantum quantum, Casella casellaPartenza)
        {
            this.casellaPartenza = casellaPartenza;
            naveMossa = casellaPartenza.Occupante;
            this.quantum = quantum;

            // Faccio partire il pathfinder
            pathFinder = new PathFinder();
            pathFinder.Start(this.casellaPartenza, naveMossa.MuoveInDiagonale);

            // Illumino le caselle raggiungibili
            var caselleRaggiungibili =

                Tile.Tiles(t =>
                {
                    int d = pathFinder.DistanzaCasella(t);
                    return (d <= naveMossa.Pwr && d > 0);
                }
                ).Select(t => t.ID).ToArray();
            
            quantum.getGUI().Tabellone.IlluminaCaselle(caselleRaggiungibili);
        }


        protected override void Esegui()
        {
            // Variabili locali
            Giocatore giocatoreDiTurno = quantum.getGestoreDiGiocatori().getGiocatoreDiTurno();
            Casella casellaCliccata = quantum.getGUI().Tabellone.TileClick as Casella;
            var ultimoClick = quantum.getGUI().Tabellone.UltimoClick;
            Nave naveTarget = casellaCliccata?.Occupante;
            casellaTarget = casellaCliccata;

            int dist = pathFinder.DistanzaCasella(casellaTarget);

            // Controllo la deselezione
            if (ultimoClick == TipoEventoMouse.ClkDx || (ultimoClick == TipoEventoMouse.ClkSin && casellaCliccata == null) || dist > naveMossa.Pwr || (dist == 0 && casellaCliccata?.Equals(casellaPartenza) == false))
            {    
                Cleanup();
                return;
            }

            if (naveTarget != null && !naveTarget.Alleato(giocatoreDiTurno))

            {
                // Combattimento

                Interfaccia.ConsoleMessaggi.NuovoMessaggio("Una nave" + naveMossa.Tipo + " di colore" + naveMossa.Colore + " ha attaccato una nave" + naveTarget.Tipo + " di colore" + naveTarget.Colore + ".", giocatoreDiTurno.SpriteColor);

                bool risultatoAttacco = naveMossa.Attacco(casellaTarget, true);

                if (risultatoAttacco)
                {
                    Interfaccia.ConsoleMessaggi.NuovoMessaggio("Attacco riuscito! Piazza l'astronave sulla casella desiderata");

                }
                else
                {
                    Interfaccia.ConsoleMessaggi.NuovoMessaggio("Attacco fallito! Piazza l'astronave sulla casella desiderata");
                }

                casellaPartenza.Occupante = null;  // rimuovo temporaneamente dal gioco la nave attaccante

                giocatoreDiTurno.Azione();

                quantum.getGUI().Tabellone.ResetSelezioneMouse();

                Tile[] adiacenti = casellaTarget.TileAdiacenti(true, naveMossa.MuoveInDiagonale);

                Casella[] disponibili = adiacenti.Where(t =>
                {
                    Casella c;
                    if (t?.EunaCasella == true) c = (Casella)t; else return false;

                    int d = pathFinder.DistanzaCasella(c);
                    return
                    (
                     c.Occupante == null
                     && (d < naveMossa.Pwr || c.Equals(casellaTarget))
                     && (d > 0 || c.Equals(casellaPartenza))
                    );

                }).Select(t => (Casella)t).ToArray();

                Cleanup();

                quantum.getGestoreDiAzioni().MettiAzioneInTesta(new AzionePiazzaNave(quantum, naveMossa, disponibili));

                //faseAttuale = indietreggia;     // nuova fase
            }

            else if (casellaTarget != null && naveTarget == null)
            {
                // Movimento
                naveMossa.Muovi(casellaPartenza, casellaTarget);
                giocatoreDiTurno.Azione();
                Cleanup();
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
