using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Interfaccia
{
    public class Cimitero: ElementoGrafico
    {
        public Cimitero (Riquadro contenitore): base (contenitore)
        {
            //non c'è niente da costruire in particolare
        }

        public override void CaricaContenuti(GuiManager gui)
        {
            // carico le sprite e il font
            _texture = gui.SpriteSheet;
            _font = gui.Font;
        }
        /// <summary> Popola il cimitero con le navi della riserva del giocatore. Da chiamare a inizio turno e in casi speciali per aggiornare il cimitero durante il turno </summary>
        public void Aggiorna (Giocatore giocatore)
        {
            _naveSelezionata = null;
            _rottami = new List<Nave>(giocatore.Rottami);
        }
       
        /// <summary>Riferimento pubblico alla Nave del cimitero che è stata selezionata.</summary>
        public Nave NaveSelezionata { get { return _naveSelezionata; } }

        public override void Draw (SpriteBatch spriteBatch)
        {
            // TODO: Disegnare lo sfondo

            int i = 0;
            if (_rottami != null)
            foreach (Nave n in _rottami)
            {

                spriteBatch.Draw(_texture, new Rectangle(contenitore.Superficie.Location.X +  (LARGH_SPRITES + DISTANZA_X) * i, contenitore.Superficie.Location.Y, LARGH_SPRITES, LARGH_SPRITES), new Rectangle(300, 0, 100, 100), Color.White);
                spriteBatch.DrawString(_font, n.Tipo.ToString(), new Vector2(contenitore.Superficie.Location.X + (LARGH_SPRITES + DISTANZA_X) * i, contenitore.Superficie.Location.Y + LARGH_SPRITES), Color.White);

                i++;
            }

            // TODO: Disegnare i bordi
        }

        protected override void MouseOver(object sender, MouseEvntArgs args)
        {
            if (contenitore.Superficie.Contains(args.Posizione) && args.Posizione.Y - contenitore.Superficie.Location.Y <= LARGH_SPRITES)
            {
                int x = args.Posizione.X - contenitore.Superficie.Location.X; //posizione X del mouse - posizione X del cimitero
                
                // calcolo per capire se il mouse sta nello spazio fra una navicella e l'altra
                _selezione = x % LATO < LARGH_SPRITES ? (int)Math.Floor(x / (float)LATO) : -1;

                // annulla una selezione non valida
                if (_selezione >= _rottami?.Count) _selezione = -1; 
            }

            else _selezione = -1; //annulla la selezione se il click è fuori dai bordi del cimitero
        }

        protected override void ClickSinistro(object sender, MouseEvntArgs args)
        {

            if (_selezione >= 0 && _rottami != null)
            {
                ConsoleMessaggi.NuovoMessaggio(_rottami[_selezione].Tipo.ToString());
                _naveSelezionata = _rottami[_selezione];
            }
        }

        List<Nave> _rottami;
        Texture2D _texture;
        SpriteFont _font;
        Nave _naveSelezionata;

        const int LATO = LARGH_SPRITES + DISTANZA_X;
        /// <summary> ID della nave della riserva selezionata: -1 = nessuna selezione </summary>
        int _selezione = -1;

        const int LARGH_SPRITES = 50; // larghezza dei singoli sprite delle astronavi da disegnare
        const int DISTANZA_X = 25;  // spaziatura fra uno sprite e l'altro
    }
}
