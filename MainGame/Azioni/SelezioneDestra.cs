using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Quantum_Game.Interfaccia;


namespace Quantum_Game.Azioni
{
    public class SelezioneDestra: AzioneDiGioco
    {
        public SelezioneDestra (Game game) : base (game)
        {
            _casellaPartenza = casellaCliccata;
            gui.Tabellone.ResetSelezioneMouse();

            // TODO: Questa è una pecionata! serve un sistema più uniforme e ragionevole per gestire dinamicamente 2 bottoni del cazzo
            bool special, riconfig;
            riconfig = !naveUsata.Riconfigurata;
            special = !naveUsata.SpecialUsata && checkSpecial(naveUsata);

            bottone[] bottoni = new bottone[2];
            bottoni[0] = bottone.Riconfigura;
            bottoni[1] = bottone.UsaSpecial;

            menu = new MenuTendina(gui.Tabellone.Tile2Pixel(_casellaPartenza), bottoni, this);

            menu.Elementi[0].Enabled = riconfig;
            menu.Elementi[1].Enabled = special;
            gui.Iscrivi(menu);
            menu.Elementi[0].Click += BottonePremuto;
            menu.Elementi[1].Click += BottonePremuto;
        }

        #region Implementazione di AzioneDiGioco
        public override void Esegui ()
        {
            // Chiude i menù con il click destro
            if (ultimoClick == TipoEventoMouse.ClkDx)
            {
                Cleanup(true);
            }
            // per il resto questo blocco non fa niente - stiamo solo in attesa di Eventi
        }

        public override bool Abort()
        {
            Cleanup(true);
            return true;
        }

        /// <param name="esciDalLoop">true se non si intende chiamare un'altra azione dopo, ma si sta tornando al ciclo idle</param>
        void Cleanup(bool esciDalLoop)
        {
            if (esciDalLoop) AzioneSuccessiva = null;
            Cleanup();
        }
        protected override void Cleanup()
        {
            menu.Elementi[0].Click -= BottonePremuto;
            menu.Elementi[1].Click -= BottonePremuto;
            gui.Rimuovi(this);
            gui.Tabellone.ResetSelezioneMouse();
        }
        #endregion

        #region Gestione Bottoni Speciali
        void BottonePremuto (object bott, EventArgs e)
        {
            var b = (Bottone)bott;
            if (b.TipoBottone == bottone.Riconfigura) riconfigura();
            else if (b.TipoBottone == bottone.UsaSpecial) usaSpecial();
        }
        void riconfigura()
        {
            e_nave t1 = naveUsata.Tipo;
            naveUsata.Riconfigura();
            giocatoreDiTurno.Azione();
            e_nave t2 = naveUsata.Tipo;

            System.Diagnostics.Debug.WriteLine("Riconfigurata nave {0} in nave {1}!", t1, t2);
            Cleanup(true);
        }
        void usaSpecial()
        {
            bool azioneSuccessivaNulla = false;
            switch (naveUsata.Tipo)
            {
                case e_nave.Battlestation:
                    AzioneSuccessiva = new Special_Battlestation(game, _casellaPartenza);
                    break;
                case e_nave.Flagship:
                    AzioneSuccessiva = new Special_Flagship(game, _casellaPartenza);
                    break;
                case e_nave.Destroyer:
                    AzioneSuccessiva = new Special_Warp(game, _casellaPartenza);
                    break;
                case e_nave.Interceptor:
                    naveUsata.UsaSpecial();
                    azioneSuccessivaNulla = true;
                    break;
                case e_nave.Scout:
                    naveUsata.Riconfigura(true);
                    naveUsata.UsaSpecial();
                    azioneSuccessivaNulla = true;
                    break;

                default:
                    System.Diagnostics.Debug.WriteLine("Special non ancora implementata");
                    AzioneSuccessiva = null;
                    break;
            }

            Cleanup(azioneSuccessivaNulla);
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
                            casella = _casellaPartenza + (Direzioni)dir as Casella;
                            if (casella != null && casella.Occupante != null &&     // null check
                                !casella.Occupante.Alleato(giocatoreDiTurno))
                                return true; }
                    } break;

                case e_nave.Flagship:
                    if (nave.Mossa) return false;
                    foreach (var dir in direzioni)
                    {
                        if ((int)dir <= 0) continue;
                        casella = _casellaPartenza + (Direzioni)dir as Casella;
                        if (casella != null && casella.PresenzaAlleata(nave))
                            return true;
                    } break;

                case e_nave.Destroyer:
                    return (giocatoreDiTurno.NumeroNaviInGioco > 1);
                case e_nave.Frigate:
                    return true;
                case e_nave.Interceptor:
                    return !nave.Mossa;
                case e_nave.Scout:
                    return true;
            }
            return false;
    }

        private readonly Casella _casellaPartenza;
        private Nave naveUsata { get { return _casellaPartenza.Occupante; } }
        private MenuTendina menu;
        #endregion
    }

}
