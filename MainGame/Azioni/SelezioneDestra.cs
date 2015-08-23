using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace Quantum_Game.Azioni
{
    public class SelezioneDestra: AzioneDiGioco
    {
        public SelezioneDestra (Game game) : base (game)
        {
            _casellaPartenza = casellaCliccataDx;
            Inizializzazione();
        }

        protected override void Inizializzazione()
        {
            gui.tabellone.MouseAttivo = false;
            MenuTendina menu = new MenuTendina
                (gui.tabellone.Tile2Pixel(_casellaPartenza), bottone.Riconfigura, bottone.UsaSpecial);

            gui.PopupMenu(menu);
        }

        public override void Esegui ()
        {
            if (ultimoClick == TipoEventoMouse.ClkDx)
            {    // Click non valido: deselezione
                Cleanup();
                return;
            }
            /* TODO:
            Mostrare il menù di selezione fra riconfigurazione e special;
            riconfigurazione -> si chiama il metodo e stop
            Special -> si entra in un altro blocco di codice per la gestione dello special 
            */
            
            if (gui.BottonePremuto == bottone.Riconfigura && !naveUsata.Riconfigurata)
            {
                e_nave t1 = naveUsata.Tipo;
                naveUsata.Riconfig();
                e_nave t2 = naveUsata.Tipo;

                System.Diagnostics.Debug.WriteLine("Riconfigurata nave {0} in nave {1}!", t1, t2);
                Cleanup();
            }
        }

        protected override void Cleanup()
        {
            gui.ChiudiMenu();
            base.Cleanup();
            gui.tabellone.MouseAttivo = true;
        }

        private readonly Casella _casellaPartenza;
        private Nave naveUsata { get { return _casellaPartenza.Occupante; } }

    }
}
