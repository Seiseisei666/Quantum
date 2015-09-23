using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Quantum_Game.Animazioni;
namespace Quantum_Game.Azioni
{
    public class MovimentoAttacco : AzioneDiGiocoComplessa
    {
        public MovimentoAttacco(Game game) : base(game)
        {
            _casellaPartenza = casellaCliccata;
        }
        /// <summary>Costruttore per un movimento/attacco che parte da una casella ben definita</summary>
        public MovimentoAttacco(Game game, Casella casellaPartenza) : base (game)
        {
            _casellaPartenza = casellaPartenza;
        }

        protected override void inizializzazione()
        {
            naveMossa = _casellaPartenza.Occupante;
            gui.Tabellone.ResetSelezioneMouse();

            pathFinder = new PathFinder();
            pathFinder.Start(_casellaPartenza, naveMossa.MuoveInDiagonale);

            var caselleRaggiungibili = 
                Tile.Tiles(t => 
                {
                    int d =  pathFinder.DistanzaCasella(t);
                    return (d <= naveMossa.Pwr && d > 0);
                }
                ).Select(t => t.ID).ToArray();

            //gui.Tabellone.IlluminaCaselle(pathFinder.IdCaselleValide);
            gui.Tabellone.IlluminaCaselle(caselleRaggiungibili);

            faseAttuale = movimentoAttacco; // il puntatore faseAttuale viene chiamato dal metodo AzioneDiGiocoComplessa.Esegui()
        }

        void movimentoAttacco()
        {
            int dist = pathFinder.DistanzaCasella(casellaCliccata);

            if (ultimoClick == TipoEventoMouse.ClkDx || (ultimoClick == TipoEventoMouse.ClkSin && casellaCliccata == null) || dist > naveMossa.Pwr || (dist == 0 && casellaCliccata?.Equals(_casellaPartenza) == false ))
            {    // Deselezione
                Cleanup();
                return;
            }

            _casellaTarget = casellaCliccata;

            Nave naveTarget = _casellaTarget?.Occupante;
            if (naveTarget != null && !naveTarget.Alleato(giocatoreDiTurno))
            {
                // Combattimento

                Interfaccia.ConsoleMessaggi.NuovoMessaggio
                    ("Una nave" + naveMossa.Tipo + " di colore" + naveMossa.Colore + " ha attaccato una nave" + naveTarget.Tipo + " di colore" + naveTarget.Colore + ".", giocatoreDiTurno.SpriteColor);

                bool risultatoAttacco = naveMossa.Attacco(_casellaTarget, true);

                if (risultatoAttacco)
                {
                    Interfaccia.ConsoleMessaggi.NuovoMessaggio 
                        ("Attacco riuscito! Piazza l'astronave sulla casella desiderata");
                }
                else
                {
                    Interfaccia.ConsoleMessaggi.NuovoMessaggio 
                        ("Attacco fallito! Piazza l'astronave sulla casella desiderata");
                }

              //  _casellaPartenza.Occupante.RimuoviDalGioco();  // rimuovo temporaneamente dal gioco la nave attaccante

                giocatoreDiTurno.Azione();

                gui.Tabellone.ResetSelezioneMouse();

                // Illumino le caselle su cui può posizionarsi ora la nave attaccante
                // TODO: spostare questa fase dell'azione prima dell'attacco vero e proprio
                // per riprodurre l'azione come quella del gioco da tavolo
                var adiacenti = _casellaTarget.TileAdiacenti(true, naveMossa.MuoveInDiagonale);

                var disponibili = adiacenti.Where(t =>
               {
                   Casella c;
                   if (t?.EunaCasella == true) c = (Casella)t;
                   else return false;
                   int d = pathFinder.DistanzaCasella(c);
                   return
                   (
                    c.Occupante == null
                    && (d < naveMossa.Pwr || c.Equals(_casellaTarget) == true)
                    && (d > 0 || c.Equals(_casellaPartenza) == true)
                   );

               }).Select(t => t.ID).ToArray();
                
                gui.Tabellone.IlluminaCaselle (disponibili);

                // preparo la fase successiva (riposizionamento dopo l'attacco)
                faseAttuale = indietreggia;
                // faccio partire l'animazione
                lancioAnimazione();
            }

            else if (_casellaTarget != null && naveTarget == null)
            {
                // Movimento
                faseAttuale = fineMovimento;     // nuova fase
                AzioneSuccessiva = new AttesaAnimazione(naveMossa, this);
                lancioAnimazione();
            }
        }
        void fineMovimento ()
        {
            // Movimento
            naveMossa.Muovi(_casellaPartenza, _casellaTarget);
            giocatoreDiTurno.Azione();
            Cleanup();
        }
        void lancioAnimazione()
        {
            // Arrivati a questo punto dell'azione non possiamo più annullarla
            puòAbortire = false;

            // Prendo tutti i punti del percorso per visualizzare il movimento della nave
            var enumPunti = 
                pathFinder.PercorsoXCasella(_casellaTarget).Select
                (
                    idCasella => gui.Tabellone.Tile2Pixel(Tile.id2Tile(idCasella))
                );
            //Aggiungo la casella di partenza della nave
            Vector2[] punti = new Vector2[] { gui.Tabellone.Tile2Pixel(_casellaPartenza) }.Concat(enumPunti).ToArray();

            // Faccio partire l'animazione
        //    naveMossa.Animazione = new Movimento(punti);
            // Attendo che finisca
            AzioneSuccessiva = new AttesaAnimazione(naveMossa, this);
        }

        /// <summary>L'attaccante viene posizionato nella casella da cui proveniva l'attacco (ovvero, una casella distante 1 in meno del massimo movimento della nave attaccante)</summary>
        void indietreggia ()
        {
            int distanza = pathFinder.DistanzaCasella(casellaCliccata);
            if ((distanza > naveMossa.Pwr) || (distanza == 0 && 
                casellaCliccata?.Equals(_casellaPartenza) == false))
                return;
            if (casellaCliccata?.Occupante == null &&
               (casellaCliccata == _casellaTarget ||                   
               casellaCliccata?.Adiacente(_casellaTarget,true) == true))
            {
                naveMossa.Piazza(casellaCliccata);
                Cleanup();
            }
        }

        public override bool Abort()
        {
            if (puòAbortire)
            {
                Cleanup();
                return true;
            }
            else
            {
                Interfaccia.ConsoleMessaggi.NuovoMessaggio("Impossibile annullare l'azione!", Color.Salmon);
                return false;
            }
        }
        protected override void Cleanup()
        {
            gui.Tabellone.ResetSelezioneMouse();
            gui.Tabellone.SpegniCaselle();
            faseAttuale = null;
            AzioneSuccessiva = null;
        }

        Casella _casellaPartenza, _casellaTarget;
        Nave naveMossa;
        PathFinder pathFinder;
        bool puòAbortire = true;
    }
}
