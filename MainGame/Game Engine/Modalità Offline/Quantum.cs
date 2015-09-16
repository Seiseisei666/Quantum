using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;
using Quantum_Game.Interfaccia;
using Quantum_Game.Mappa;



namespace Quantum_Game
{
    // Modalità OffLine ("Single"Player)
    public class Quantum : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Sfondo sfondo;


        /*Contiene le informazioni sullo stato della partita
        che vengono poi stampate a video dal metodo draw
        */
        private FlussoDiGioco flussoGioco;
        private GameSystem gameSystem;

        public Quantum()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1024;  
            graphics.PreferredBackBufferHeight = 576;
            IsMouseVisible = true;
            graphics.ApplyChanges();

        }

        protected override void Initialize()
        {
            //Crea la mappa.
            //Per ora il percorso del file sta qui, poi potrebbe essere una selezione tra vari preset,
            //o addirittura un map editor integrato nel gioco, basta fargli scrivere un txt con la mappa!
            string file = @"Content\Mappe\mappaeasy.txt";
            MapGenerator generatore = new MapGenerator(file);
            Tile.CreaMappa(generatore.GeneraMappa(), generatore.Righe, generatore.Colonne);

            // Crea il gamesystem con 2 giocatori
            gameSystem = new GameSystem();
            gameSystem.AggiungiGiocatori(2);
            
            //L'evento InizioPartita viene generato dopo che sono state disposte le pedine iniziali
            //avviene una volta per partita e lo associamo manualmente a tutti gli oggetti
            //che lo utilizzeranno
            gameSystem.InizioPartita += InizioPartita;

  

            // CREIAMO I COMPONENTI E LI AGGIUNGIAMO ALLA RACCOLTA GAME.COMPONENTS
            Services.AddService<GameSystem>(gameSystem);

            MouseInput mouseInput = new MouseInput(this);
            Components.Add(mouseInput);
            GuiManager gui = new GuiManager(this);
            Components.Add(gui);
            flussoGioco = new FlussoDiGioco(this);
            Components.Add(flussoGioco);
            Tabellone tab = new Tabellone(this, 3, 3, 80, 70);
            Components.Add(tab);
            sfondo = new Sfondo(this);
            Components.Add(sfondo);

            base.Initialize();

        }

        //crea l'interfaccia grafica con le sue componenti
        protected override void LoadContent()
        {
            spriteBatch = Services.GetService<SpriteBatch>();

            var gui = Services.GetService<GuiManager>();
            Bottone iniziaPartita = Bottone.Standard(bottone.IniziaPartita, 82, 95);
            Bottone passaTurno = Bottone.Standard(bottone.Passa, 82, 85);
            Bottone ricerca = Bottone.Standard(bottone.Ricerca, 82, 75);
            gui.Iscrivi(passaTurno);
            gui.Iscrivi(iniziaPartita);
            gui.Iscrivi(ricerca);
      
  

            ConsoleMessaggi console = new ConsoleMessaggi(3, 83, 80, 18);
            gui.Iscrivi(console);

            Cimitero cim = new Cimitero(3, 73, 80, 15);
            gui.Iscrivi(cim);


            base.LoadContent();
        } 

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit(); //questa ci stava per default, è per chiudere la finestra
            
            
            flussoGioco.Update();


            base.Update(gameTime);
        }
        
            
        
   
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            sfondo.Draw();

            base.Draw(gameTime);

            spriteBatch.End();
        }

        //trigger che si attiva ad inizio partita (?)

        private void InizioPartita(object sender, EventArgs args)
        {
            Debug.WriteLine("Partita iniziata!!");
        }
    }
}
