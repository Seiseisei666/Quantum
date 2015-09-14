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


        Texture2D texture;

        public Quantum()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1024;  
            graphics.PreferredBackBufferHeight = 576;
            graphics.IsFullScreen = false;
            IsMouseVisible = true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            //Crea la mappa.
            //Per ora il percorso del file sta qui, poi potrebbe essere una selezione tra vari preset,
            //o addirittura un map editor integrato nel gioco, basta fargli scrivere un txt con la mappa!
            string file = @"Data Content\Mappe\mappaeasy.txt";
            MapGenerator generatore = new MapGenerator(file);
            Tile.CreaMappa(generatore.GeneraMappa(), generatore.Righe, generatore.Colonne);

            // Crea il gamesystem con 2 giocatori
            GameSystem gameSystem = new GameSystem();
            gameSystem.AggiungiGiocatori(2);
            
            //L'evento InizioPartita viene generato dopo che sono state disposte le pedine iniziali
            //avviene una volta per partita e lo associamo manualmente a tutti gli oggetti
            //che lo utilizzeranno
            gameSystem.InizioPartita += InizioPartita;

            // QUESTA RIGA SERVE SOLO PER TESTARE IL POSIZIONAMENTO DELLE NAVI
            gameSystem.IniziaSetupPartita();
            //TODO: DA TOGLIERE

            // CREIAMO I COMPONENTI E LI AGGIUNGIAMO ALLA RACCOLTA GAME.COMPONENTS
            Services.AddService<GameSystem>(gameSystem);

            var schermo = new Schermo(this);
            Components.Add(schermo);

            MouseInput mouseInput = new MouseInput(this);
            Components.Add(mouseInput);
            GuiManager gui = new GuiManager(this);
            Components.Add(gui);
            flussoGioco = new FlussoDiGioco(this);
            Components.Add(flussoGioco);
            sfondo = new Sfondo(this);
            Components.Add(sfondo);


            base.Initialize();
        }

        //crea l'interfaccia grafica con le sue componenti
        protected override void LoadContent()
        {
            spriteBatch = Services.GetService<SpriteBatch>();

            var gui = Services.GetService<GuiManager>();

            /*  ESEMPIO DEL SISTEMA RIQUADRI  */

            var schermo = Riquadro.Main;
            var barraSuperiore = schermo.Riga(5);

            var main = schermo.Colonna(75);
                var tabellone = main.Riga(100, 5,5);
                

            var laterale = schermo.Colonna(100, 5);

                var info = laterale.Riga(50, 0,10);
                var bott1 = laterale.Riga(10,35,5);
                var bott2 = laterale.Riga(10,35,5);
                var bott3 = laterale.Riga(10,35,5);
                var bott4 = laterale.Riga(10,35,5);
                var msg = laterale.Riga(100, 0,15);

            Tabellone tab2 = new Tabellone(this, tabellone);
            gui.Iscrivi(tab2);
            Bottone colonizza = new Bottone(bottone.Colonizza, bott1);
            Bottone passaTurno = new Bottone(bottone.Passa, bott4);
            Bottone ricerca = new Bottone(bottone.Ricerca, bott3) ;
            gui.Iscrivi(colonizza);
            gui.Iscrivi(passaTurno);
            gui.Iscrivi(ricerca);
            ConsoleMessaggi console = new ConsoleMessaggi(msg);
            gui.Iscrivi(console);
            Cimitero cim = new Cimitero(info);
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
