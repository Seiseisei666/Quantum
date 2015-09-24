using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantum_Game.Animazioni;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Azioni
{
    public class AzioneMovimento: Azione
    {
        Casella casellaPartenza, casellaTarget;
        Nave naveMossa;
        Quantum quantum;
        PathFinder pathFinder;
        bool puòAttaccare;

        public AzioneMovimento (Quantum quantum, Casella casellaPartenza, bool puòAttaccare = true)
        {
            this.casellaPartenza = casellaPartenza;
            naveMossa = casellaPartenza.Occupante;
            this.quantum = quantum;
            this.puòAttaccare = puòAttaccare;

            // Faccio partire il pathfinder
            pathFinder = new PathFinder();
            pathFinder.Start(this.casellaPartenza, naveMossa.MuoveInDiagonale);

            // Illumino le caselle raggiungibili
            int[] caselleRaggiungibili =

                Tile.Tiles(t =>
                {
                    Casella casella = t as Casella;
                    int d = pathFinder.DistanzaCasella(t);
                    return (d <= naveMossa.Pwr && d > 0 && (puòAttaccare || (casella != null && casella.Occupante == null) ));
                }
                ).Select(t => t.ID).ToArray();

            quantum.getGUI().Tabellone.ResetSelezioneMouse();
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

            if (naveTarget != null && !naveTarget.Alleato(giocatoreDiTurno) && puòAttaccare)

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


                quantum.getGestoreDiAzioni().ImpilaAzione(new AzionePiazzaNave(quantum, naveMossa, disponibili));
                lancioAnimazione();
                //faseAttuale = indietreggia;     // nuova fase
            }

            else if (casellaTarget != null && naveTarget == null)
            {
                // Movimento
                naveMossa.Muovi(casellaPartenza, casellaTarget);
                giocatoreDiTurno.Azione();
                lancioAnimazione();
            }

        }

        protected override void Cleanup()
        {
            quantum.getGUI().Tabellone.ResetSelezioneMouse();
            quantum.getGUI().Tabellone.SpegniCaselle();
            Terminata = true;
        }

        void lancioAnimazione()
        {
            // Prendo tutti i punti del percorso della nave
            var enumerablePunti =
                pathFinder.PercorsoXCasella(casellaTarget).Select
                (
                    idCasella => quantum.getGUI().Tabellone.Tile2Pixel(Tile.id2Tile(idCasella))
                );
            //Aggiungo la casella di partenza della nave
            Vector2 posizionePartenza = quantum.getGUI().Tabellone.Tile2Pixel(casellaPartenza);
            Vector2[] punti = new Vector2[] { posizionePartenza }.Concat(enumerablePunti).ToArray();

            quantum.getGestoreDiAzioni().ImpilaAzione(new Movimento(naveMossa, punti));
            Cleanup();
        }
    }
}
