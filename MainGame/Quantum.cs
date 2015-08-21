using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;



namespace Quantum_Game
{
    /// <summary>
    /// Il gioco vero e proprio
    /// </summary>
    public class Quantum : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont font;

        private Texture2D schermataIniziale;
        private Texture2D textureCaselle;
        private Texture2D contornoCasella;

        private Tabellone tabellone;
        private MouseInput mouseInput;
        private GameSystem gameSystem;
        private FlussoDiGioco flussoGioco;
        private PathFinder pathFinder;
        private GUI Gui;

        static public event EventHandler<ResizeEvntArgs> Ridimensionamento;
        public static int contaFrames = 0; //mi serve per fare prove di attesa di tot secondi

        public Quantum()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;  
            graphics.PreferredBackBufferHeight = 600;
            IsMouseVisible = true;
            graphics.ApplyChanges();
            mouseInput = new MouseInput();
            if (mouseInput == null) throw new Exception("Stocazzo");

        }

        protected override void Initialize()
        {
            // Crea il gamesystem con 2 giocatori
            gameSystem = new GameSystem();
                        
            // Inizializzazioni fase MenuIniziale
            gameSystem.IniziaMenuIniziale();
            
            gameSystem.AggiungiGiocatori(2);

            //Crea la mappa
                // il map generator farà le sue cose e poi stabilirà da solo le dimensioni della mappa
            MapGenerator generatore = new MapGenerator(9, 9); // <- da sistemare perché i 9 non possono rimanere fissi
            Mappa mappa = new Mappa( generatore.GeneraMappa(), generatore.Righe, generatore.Colonne);
            
            //L'evento InizioPartita viene generato dopo che sono state disposte le pedine iniziali
            //avviene una volta per partita e lo associamo manualmente a tutti gli oggetti
            //che lo utilizzeranno
            gameSystem.InizioPartita += this.InizioPartita;

            // DA TOGLIERE
            Services.AddService(mappa);

            Services.AddService(mouseInput);
            Services.AddService(gameSystem);

            pathFinder = new PathFinder(this);
            Components.Add(pathFinder);

            // QUESTA RIGA SERVE SOLO PER TESTARE IL POSIZIONAMENTO DELLE NAVI
            gameSystem.IniziaSetupPartita();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Caricamento e iscrizione ai servizi della roba x la grafica
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(GraphicsDevice);
            Services.AddService(spriteBatch);

            // CARICAMENTO CONTENUTO

            schermataIniziale = Content.Load<Texture2D>(@"Graphica\schermataIniziale_sfondo_2.1");
            textureCaselle = Content.Load<Texture2D>(@"Graphica\TileSet_prova2");
            font = Content.Load<SpriteFont>(@"Font\Font");
            // texture di 1x1 pixel con alpha blending, per disegnare "a mano"
            contornoCasella = new Texture2D(GraphicsDevice, 1, 1);
            contornoCasella.SetData(new[] { (Color.White*0.5f) });

            // Inizializzazione GUI
            Gui = new GUI(this, contornoCasella);
            Gui.Font = font;
            Gui.AddElement(new Bottone(bottone.Passa, 0.72f, 0.8f, 0.8f, 0.85f, 800, 600));
            tabellone = new Tabellone(this, 0.05f, 0.1f);
            Gui.AddElement(tabellone);
            Components.Add(Gui);


            flussoGioco = new FlussoDiGioco (this);

            base.LoadContent();
        } 

        protected override void UnloadContent()
        {
            spriteBatch.Dispose();
            contornoCasella.Dispose();
            textureCaselle.Dispose();
            schermataIniziale.Dispose();
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit(); //questa ci stava per default, è per chiudere la finestra
            
            mouseInput.Update(); // routine di aggiornamento dell'input del mouse, di cui si occupa
                                 // l'oggetto mouseInput
            flussoGioco.Update();
            


            base.Update(gameTime);
        }
        
            
        
   
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // qui cominciano le routine di disegno dei vari oggetti
            spriteBatch.Begin();

            if (gameSystem.FasePartita == FasiDiGioco.MenuIniziale)
            {
                // disegna la schermata iniziale per 300 frames (5s), poi passa alla fase Setup
                if (Quantum.contaFrames < 300)
                {
                    Rectangle destRect = new Rectangle(0, 0, 800, 600);
                    spriteBatch.Draw(schermataIniziale, destRect, Color.White);
                }
                else gameSystem.SettaFaseDiGioco(FasiDiGioco.SetupPartita);
            }
            else if (gameSystem.FasePartita == FasiDiGioco.PartitaInCorso || gameSystem.FasePartita == FasiDiGioco.SetupPartita)
            {

                tabellone.Draw(spriteBatch, textureCaselle);
                tabellone.DisegnaSelezione(spriteBatch, contornoCasella);
                pathFinder.Draw(tabellone, spriteBatch, contornoCasella);
                Gui.Draw();
            }

            spriteBatch.End();
            // finiscono qui

            base.Draw(gameTime);
        }
        /// <summary>
        /// Evento ipotetico per gestire un improbabile ridimensionamento delle finestre
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnRidimensionamento(ResizeEvntArgs args) 
        {
            if (Ridimensionamento != null)
                Ridimensionamento(this, args);
        }

        /*  Qua cominciano altre cose nostre */

        private void InizioPartita(object sender, EventArgs args)
        {
            Debug.WriteLine("Partita iniziata!!");
            Debug.WriteLine("Turno del giocatore {0}", gameSystem.GiocatoreDiTurno.Colore);
        }       
    }
}
