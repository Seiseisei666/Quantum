using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quantum_Game.Interfaccia;
using Quantum_Game.Mappa;
using Quantum_Game.Azioni;
using Quantum_Game.Schermate;

namespace Quantum_Game
{
    /* Core della Modalità Off-Line ("Single"Player) */
    public class Quantum : Game
    {
        //Componenti del motore di gioco
        private GestoreDiGiocatori gestoreDiGiocatori;
        private GestoreDiAzioni gestoreDiAzioni;
        private GestoreDiSchermate gestoreDiSchermate;
       
        //Componenti del motore grafico
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GuiManager gui;


        /* Costruttore di default che carica i settings salvati nel file settings.config  */
        public Quantum()
        {
            //creazione dei gestori degli elementi di gioco
            gestoreDiGiocatori = new GestoreDiGiocatori();
            gestoreDiAzioni = new GestoreDiAzioni();

            //creazione motore grafico
            graphics = new GraphicsDeviceManager(this);

            gui = new GuiManager(this);
            Components.Add(gui);

            //carichiamo i settings di default (o salvati)
            loadSettings();
            
        }

        protected override void Initialize()
        {
            //colleghiamo periferiche I/O

            Schermo schermo = new Schermo(this);
            Components.Add(schermo);

            MouseInput mouseInput = new MouseInput(this);
            Components.Add(mouseInput);

            // TODO: creare menu apposito per caricare le  opzioni di partita (giocatori, mappa, etc) e sintetizzarlo con un metodo


            //Crea la mappa.

            //MapGenerator generatore = new MapGenerator(@"Data Content\Mappe\mappaeasy.txt");
            //Tile.CreaMappa(generatore.GeneraMappa(), generatore.Righe, generatore.Colonne);

            //carichiamo lo sfondo per la mappa
            //Sfondo sfondo = new Sfondo(this);
            //Components.Add(sfondo);

            // Imposta il numero di giocatori
            int numeroGiocatori = 2;

            //viene incodata un'azione che si occuperà di eseguire il setup di una partita offline con due giocatore
          //  gestoreDiAzioni.IncodaAzione(new AzioneSetupPartitaOffLine(this, numeroGiocatori));

            base.Initialize();

        }

        //crea l'interfaccia grafica con le sue componenti
        protected override void LoadContent()
        {
            //Per qualche motivo mistorioso lo spriteBatch va preso in LoadContent()
            spriteBatch = Services.GetService<SpriteBatch>();

            /*   MENU INIZIALE   */
            gestoreDiSchermate = new GestoreDiSchermate(this);
            gestoreDiSchermate.CaricaSchermata(new SchermataMenu(this));


            base.LoadContent();
        } 

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
        
        protected override void Update(GameTime gameTime)
        {

            gestoreDiAzioni.Update();

            base.Update(gameTime);
        }
   
        protected override void Draw(GameTime gameTime)
        {
           spriteBatch.Begin();

            GraphicsDevice.Clear(Color.Black);
               
            base.Draw(gameTime);

            spriteBatch.End();
        }

        public void loadSettings()
        {
            //TODO: 1) mettere dei settings di default nel file settings.config; 
            //TODO: 2) sostituire tutto quello con una istruzione che carica il contenuto di settings.config.
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            IsMouseVisible = true;
            graphics.ApplyChanges();
        }

        public GuiManager getGUI() {return gui;}
        public GestoreDiGiocatori getGestoreDiGiocatori() {return gestoreDiGiocatori; }
        public GestoreDiAzioni getGestoreDiAzioni() {return gestoreDiAzioni;}
        public GestoreDiSchermate schermateDiGioco { get { return gestoreDiSchermate; } }
    }
}
