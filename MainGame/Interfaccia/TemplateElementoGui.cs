using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quantum_Game.Interfaccia
{
    public class TemplateElementoGui: RiquadroGui
    {
        //Qualche esempio di costrutture:

        // Costruttore vuoto, con dimensioni scelte arbitrariamente
        public TemplateElementoGui () : base (50, 10, 20, 25)
        {

        }        

        // Costruttore con oggetto parent
        public TemplateElementoGui(int x, int y, int larg, int alt, object parent) : base(x,y,larg, alt, parent)
        {

        }


        public override void Inizializzazione(GuiManager gui)
        {
            /*
            Questa inizializzazione viene chiamata nel momento in cui iscriviamo l'oggetto al gui
            */
            base.Inizializzazione(gui);

            /*
            SOLO QUI, dopo l'inizializzazione della base, possiamo lavorare con le dimensioni assolute in pixel
            */

            System.Diagnostics.Debug.WriteLine("Dimensioni in pixel di questo oggetto:");
            System.Diagnostics.Debug.WriteLine("{0} (Larghezza)", this.Larghezza);
            System.Diagnostics.Debug.WriteLine("{0} (Altezza)", this.Altezza);
            System.Diagnostics.Debug.WriteLine("{0}, {1} (Distanza dal bordo sup sin", this.Posizione.X, this. Posizione.Y);

            /*
            Inoltre possiamo caricare skin e font del GuiManager
            */

            var texture = gui.SpriteSheet;
            var font = gui.Font;

            pennello = gui.Pennello;

        }

        protected override void ClickSinistro(object sender, MouseEvntArgs infoSulMouse)
        {
            /*
            Una volta iscritto al GuiManager, questo oggetto è risponsivo al mouse:
            */

            if (Compreso (infoSulMouse.Posizione))
            {
                System.Diagnostics.Debug.WriteLine("CLICK!");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("LISCIO!");
            }

            /*
            Overrideare ClickSinistro, ClickDestro e MouseOver se necessario
            Occhio che MouseOver viene sparato ogni frame!! (forse potrebbe essere reso più intelligente, ma non so se ne vale la pena)
            */

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            /*
            Qui si disegna
            Ora faccio una serie di quadrati
            */

            spriteBatch.Draw(pennello, new Rectangle(Posizione.X, Posizione.Y, Larghezza, Altezza), Color.Brown);
            spriteBatch.Draw(pennello, new Rectangle(Posizione.X + 80, Posizione.Y +5 , Larghezza - 10, Altezza - 10), Color.LightSeaGreen*0.6f);
            spriteBatch.Draw(pennello, new Rectangle(Posizione.X + 25, Posizione.Y + 64, Larghezza - 40, Altezza - 20), Color.Pink);

            // Animazioni non supportate al momento =D

        }

        Texture2D pennello;
    }
}
