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
            _casellaPartenza = casellaCliccata;
            Inizializzazione();
        }

        protected override void Inizializzazione()
        {
            gui.tabellone.ResetSelezioneMouse();
            gui.tabellone.SelezTileVisibile = false;

            MenuTendina menu = new MenuTendina
                (gui.tabellone.Tile2Pixel(_casellaPartenza), bottone.Riconfigura, bottone.UsaSpecial);

            gui.PopupMenu(menu);
        }

        public override void Esegui ()
        {
            if (gui.BottonePremuto == bottone.Riconfigura && !naveUsata.Riconfigurata)
            {
                e_nave t1 = naveUsata.Tipo;
                naveUsata.Riconfig();
                giocatoreDiTurno.Azione();
                e_nave t2 = naveUsata.Tipo;

                System.Diagnostics.Debug.WriteLine("Riconfigurata nave {0} in nave {1}!", t1, t2);
                Cleanup();
            }

            // Se c'è stato un click e il blocco precedente di codice non è stato eseguito
            // annulla tutto
            if (ultimoClick != TipoEventoMouse.nessuno)
                Cleanup();
        }

        protected override void Cleanup()
        {
            gui.ChiudiMenu();
            gui.tabellone.ResetSelezioneMouse();
            gui.tabellone.SelezTileVisibile = true;
            base.Cleanup();
        }

        private readonly Casella _casellaPartenza;
        private Nave naveUsata { get { return _casellaPartenza.Occupante; } }

    }
}
