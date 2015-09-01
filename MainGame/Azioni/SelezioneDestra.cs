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
            Inizializzazione();
        }

        void Inizializzazione()
        {
            gui.Tabellone.ResetSelezioneMouse();

            bool special, riconfig;
            riconfig = !naveUsata.Riconfigurata;
            special = !naveUsata.SpecialUsata && checkSpecial(naveUsata);
            int size, n = 0;
            size = Convert.ToInt16(riconfig) + Convert.ToInt16(special);
            bottone[] bottoni = new bottone[size];
            if (riconfig)
                bottoni[n++] = bottone.Riconfigura;
            if (special)
                bottoni[n++] = bottone.UsaSpecial;

            MenuTendina menu = new MenuTendina (gui.Tabellone.Tile2Pixel(_casellaPartenza), bottoni, this);

            gui.Iscrivi (menu);
        }

        public override void Esegui ()
        {
            if (gui.BottonePremuto == bottone.Riconfigura)
            {
                e_nave t1 = naveUsata.Tipo;
                naveUsata.Riconfig();
                giocatoreDiTurno.Azione();
                e_nave t2 = naveUsata.Tipo;

                System.Diagnostics.Debug.WriteLine("Riconfigurata nave {0} in nave {1}!", t1, t2);
                AzioneSuccessiva = null;

                Cleanup();
            }
            else if (gui.BottonePremuto == bottone.UsaSpecial)
            {
                switch (naveUsata.Tipo)
                {
                    case e_nave.Flagship:
                        AzioneSuccessiva = new Special_Flagship(game, _casellaPartenza);
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Special non ancora implementata");
                        AzioneSuccessiva = null;
                        break;
                }

                Cleanup();
            }

            // Se c'è stato un click e il blocco precedente di codice non è stato eseguito
            // annulla tutto
            if (ultimoClick != TipoEventoMouse.nessuno)
            {
                AzioneSuccessiva = null;
                Cleanup();
            }
        }

        protected override void Cleanup()
        {
            gui.Rimuovi(this);
            gui.Tabellone.ResetSelezioneMouse();
        }

        // Controlla se la nave può usare la special
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

    }
}
