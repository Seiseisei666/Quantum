using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
//qwe
namespace Quantum_Game.Interfaccia
{
    public enum Allineamento
    {
        Sotto,
        aDestra
    }
    public class Riquadro
    {
        public Rectangle Superficie { get { return new Rectangle(xAbs, yAbs, larghezzaAbs, altezzaAbs); } }

        private readonly float _paddingLaterale;
        private readonly float _paddingTopBottom;
        private readonly bool _eUnaColonna;

        protected List<Riquadro> _figli;

        /// <summary>
        /// Costruttore per lo Schermo, l'oggetto master dei riquadri
        /// </summary>
        protected Riquadro()
        {
            _figli = new List<Riquadro>();
            _larghRelativa = 100;
            _altRelativa = 100;
            larghezzaAbs = 0;
            altezzaAbs = 0;
        }
        /// <summary>
        /// Costruttore privato dei riquadri, chiamato dai metodi Riga e Colonna
        /// </summary>
        private Riquadro( Riquadro parent, float xRel, float yRel, float larghezza,float altezza, bool èUnaColonna, float padX = 0, float padY = 0)
        {
            _figli = new List<Riquadro>();
            _eUnaColonna = èUnaColonna;
            _larghRelativa = larghezza;
            _altRelativa = altezza;
            _Xrelativa = xRel;
            _Yrelativa = yRel;
            _paddingLaterale = padX; _paddingTopBottom = padY;
        }
        /// <summary>
        /// Restituisce un oggetto Riquadro Colonna, Figlia dell'istanza che ha chiamato questo metodo.
        /// </summary>
        /// <param name="LarghezzaRelativa">in 100esimi</param>
        /// <param name="PaddingLaterale">in 100esimi</param>
        /// <param name="PaddingTopBottom">in 100esimi</param>
        /// <returns></returns>
        public Riquadro Colonna (float LarghezzaRelativa, float PaddingLaterale = 0, float PaddingTopBottom = 0)
        {
            Riquadro figlio;
            float xRel = 0, yRel = 0;
            float xClippata = LarghezzaRelativa, yClippata = 100;
            if (_figli.Any())
            {
                var ultimo = _figli.Last();
                if (ultimo._eUnaColonna)
                {
                    xRel = ultimo._Xrelativa + ultimo._larghRelativa;
                    yRel = ultimo._Yrelativa;
                    xClippata = 100 - ultimo._Xrelativa - ultimo._larghRelativa;
                    yClippata = 100 - ultimo._Yrelativa;
                }
                else
                {
                    xRel = ultimo._Xrelativa;
                    yRel = ultimo._Yrelativa + ultimo._altRelativa;
                    xClippata = 100 - ultimo._Xrelativa;
                    yClippata = 100 - ultimo._Yrelativa - ultimo._altRelativa;
                }
                if (xClippata > LarghezzaRelativa) xClippata = LarghezzaRelativa;

            }
            figlio = new Riquadro(this, xRel, yRel, xClippata, yClippata, èUnaColonna: true, padX: PaddingLaterale, padY: PaddingTopBottom );
            iscriviFiglio(figlio);
            return figlio;
        }
        /// <summary>
        /// Restituisce un oggetto Riquadro Riga, Figlia dell'istanza che ha chiamato questo metodo.
        /// </summary>
        /// <param name="AltezzaRelativa">in 100esimi</param>
        /// <param name="PaddingLaterale">in 100esimi</param>
        /// <param name="PaddingTopBottom">in 100esimi</param>
        /// <returns></returns>
        public Riquadro Riga(float AltezzaRelativa, float PaddingLaterale = 0, float PaddingTopBottom = 0)
        {
            Riquadro figlio;
            float xRel = 0, yRel = 0;
            float xClippata = 100, yClippata = AltezzaRelativa;
            if (_figli.Any())
            {
                var ultimo = _figli.Last();

                if (ultimo._eUnaColonna)
                {
                    xRel = ultimo._Xrelativa + ultimo._larghRelativa;
                    yRel = ultimo._Yrelativa;
                    xClippata = 100 - ultimo._Xrelativa - ultimo._larghRelativa;
                    yClippata = 100 - ultimo._Yrelativa;
                }
                else
                {
                    xRel = ultimo._Xrelativa;
                    yRel = ultimo._Yrelativa + ultimo._altRelativa;
                    xClippata = 100 - ultimo._Xrelativa;
                    yClippata = 100 - ultimo._Yrelativa - ultimo._altRelativa;
                }
                if (yClippata > AltezzaRelativa) yClippata = AltezzaRelativa;

            }

            figlio = new Riquadro(this, xRel, yRel, xClippata, yClippata, èUnaColonna: false, padX: PaddingLaterale, padY: PaddingTopBottom);
            iscriviFiglio(figlio);
            return figlio;
        }

        private void iscriviFiglio (Riquadro figlio)
        {
            _figli.Add(figlio);
            figlio.calcolaDimensioniInPixel(xAbs, yAbs, larghezzaAbs, altezzaAbs);
        }

        protected virtual void calcolaDimensioniInPixel(int xParent, int yParent, int wParent, int hParent)
        {
            // calcolo dimensioni assolute in base alle dimensioni relative e alle dimensioni in pixel del parent
            xAbs = (int)(xParent + (_Xrelativa * wParent / 100f));
            yAbs = (int)(yParent + (_Yrelativa * hParent / 100f));
            larghezzaAbs = (int)(_larghRelativa * wParent / 100f);
            altezzaAbs = (int)(_altRelativa * hParent / 100f);
            // Ridimensionamento della superficie assoluta in base al padding
            int padW = (int)_paddingLaterale * larghezzaAbs / 100;
            int padH = (int)_paddingTopBottom * altezzaAbs / 100;
            xAbs += padW /2;
            yAbs += padH / 2;
            larghezzaAbs -= padW;
            altezzaAbs -= padH;
            // Chiamata ricorsiva ai figli (in caso di ridimensionamento in corso d'opera i figli vengono ridimensionati anch'essi)
            if (_figli.Any())
                foreach (var figlio in _figli) figlio.calcolaDimensioniInPixel(xAbs, yAbs, larghezzaAbs, altezzaAbs);
        }

        protected void Ridimensionamento(object s, EventArgs e)
        {
            foreach (Riquadro figlio in _figli)
                figlio.calcolaDimensioniInPixel(xAbs, yAbs, larghezzaAbs, altezzaAbs);
        }

        private float _Xrelativa, _Yrelativa, _larghRelativa, _altRelativa;
        protected int xAbs, yAbs, larghezzaAbs, altezzaAbs;

        #region Schermo


        public static Schermo Main { get; protected set; }
        #endregion
    }


    public class Schermo: Riquadro, IGameComponent
    {

        Game _game;
        public Texture2D spriteSheet;
        public Texture2D pennello;
        public SpriteFont font;
        public MouseInput mouse;

        public Schermo (Game game): base()
        {
            if (Main == null) Main = this;
            _game = game;
        }

        public void Initialize()
        {
            var finestra = _game.Window;
            finestra.ClientSizeChanged += Ridimensionamento;
            mouse = _game.Services.GetService<MouseInput>();




            calcolaDimensioniInPixel(0, 0, finestra.ClientBounds.Width, finestra.ClientBounds.Height);

        }
    }
}
