using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantum_Game.Interfaccia;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Azioni
{
    public class AzioneSelezioneDestra: Azione
    {

        public AzioneSelezioneDestra (Quantum quantum, Casella casellaPartenza)
        {
            this.casellaPartenza = casellaPartenza;
            this.quantum = quantum;
            naveUsata = casellaPartenza.Occupante;

            GuiManager gui = quantum.getGUI();
            gui.Tabellone.ResetSelezioneMouse();
            gui.Tabellone.MostraSelezione = false;

            // HACK: gestire in maniera sensata  e più agile il posizionamento dei Widget
            // (magari con un costruttore nuovo a cui si passa invece di solo un punto qualche informazione in più per posizionarsi?)
            // P.S: se ne è occupato Mirco! Ci metterà mano lui
            Point pos1 = gui.Tabellone.Tile2Pixel(this.casellaPartenza);
            Point pos2 = pos1;
            var lato = gui.Tabellone.LatoCasella;

            pos1 -= new Point(20 - lato / 2, 15);
            pos2 += new Point(20 + lato / 2, -15);

            bool puòRiconfig = !naveUsata.Riconfigurata;
            bool puòUsareSpecial = !naveUsata.SpecialUsata && checkSpecial(naveUsata);

            Riconfig = new Widget(pos1, widget.Riconfigura, puòRiconfig);
            Special = new Widget(pos2, widget.UsaSpecial, puòUsareSpecial);

            Riconfig.Click += riconfigura;
            Special.Click += usaSpecial;
            gui.Iscrivi(Riconfig);
            gui.Iscrivi(Special);

        }

        #region Implementazione di AzioneDiGioco
        protected override void Esegui()
        {
            Tabellone tabellone = quantum.getGUI().Tabellone;
            // Chiude i menù con il click destro
            if (tabellone.UltimoClick != TipoEventoMouse.nessuno)
            {
                Cleanup();
            }
            // per il resto questo blocco non fa niente - stiamo solo in attesa di Eventi
        }

        public override bool Abort()
        {
            Cleanup();
            return true;
        }

        protected override void Cleanup()
        {
            Riconfig.Click -= riconfigura;
            Special.Click -= usaSpecial;
            quantum.getGUI().RimuoviWidget();
            quantum.getGUI().Tabellone.ResetSelezioneMouse();
            Terminata = true;
        }
        #endregion

        #region Gestione Bottoni Speciali

        void riconfigura(object sender, EventArgs e)
        {
            e_nave t1 = naveUsata.Tipo;
            naveUsata.Riconfigura();
            quantum.getGestoreDiGiocatori().getGiocatoreDiTurno().Azione();
            e_nave t2 = naveUsata.Tipo;

            ConsoleMessaggi.NuovoMessaggio("Riconfigurata nave "+ t1+ " in nave" + t2 + "!!");
            Cleanup();
        }
        void usaSpecial(object sender, EventArgs e)
        {
            switch (naveUsata.Tipo)
            {
                case e_nave.Battlestation:
                    quantum.getGestoreDiAzioni().ImpilaAzione(new AzioneSpecialBattlestation(quantum, casellaPartenza));
                    break;
                case e_nave.Flagship:
                    quantum.getGestoreDiAzioni().ImpilaAzione(new AzioneSpecialFlagship(quantum, casellaPartenza));
                    break;
                case e_nave.Destroyer:
                   // AzioneSuccessiva = new Special_Warp(game, casellaPartenza);
                    break;
                case e_nave.Interceptor:
                    naveUsata.UsaSpecial();
                    break;
                case e_nave.Scout:
                    naveUsata.Riconfigura(true);
                    naveUsata.UsaSpecial();
                    break;

                default:
                    System.Diagnostics.Debug.WriteLine("Special non ancora implementata");
                    break;
            }

            Cleanup();
        }
        #endregion

        #region cose private
        /// <summary> Check sulla nave, per vedere se è in grado di effettuare la special.</summary>
        bool checkSpecial(Nave nave)
        {
            Casella casella;
            var direzioni = Enum.GetValues(typeof(Direzioni));

            switch (nave.Tipo)
            {
                case e_nave.Battlestation:

                    foreach (var dir in direzioni)
                    {
                        if ((int)dir > 0 && (int)dir <= 4)   // considera solo alto, basso, sin, dx 
                        {
                            casella = casellaPartenza + (Direzioni)dir as Casella;
                            if (casella != null && casella.Occupante != null &&     // null check
                                !casella.Occupante.Alleato(quantum.getGestoreDiGiocatori().getGiocatoreDiTurno()))
                                return true;
                        }
                    }
                    break;

                case e_nave.Flagship:
                    if (nave.Mossa) return false;
                    foreach (var dir in direzioni)
                    {
                        if ((int)dir <= 0) continue;
                        casella = casellaPartenza + (Direzioni)dir as Casella;
                        if (casella != null && casella.PresenzaAlleata(nave))
                            return true;
                    }
                    break;

                case e_nave.Destroyer:
                    return (quantum.getGestoreDiGiocatori().getGiocatoreDiTurno().NumeroNaviInGioco > 1);
                case e_nave.Frigate:
                    return true;
                case e_nave.Interceptor:
                    return !nave.Mossa;
                case e_nave.Scout:
                    return true;
            }
            return false;
        }
        private Quantum quantum;
        private Casella casellaPartenza;
        private Nave naveUsata;
        private Widget Riconfig, Special;
        #endregion
    }
}
