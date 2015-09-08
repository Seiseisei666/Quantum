using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;
using Quantum_Game.Interfaccia;
using Quantum_Game.Mappa;



namespace Quantum_Game
{
    /// <summary>
    /// Il gioco vero e proprio
    /// </summary>
    public class Quantum : Game
    {
        private GraphicsDeviceManager graphics;
        
        private FlussoDiGioco flussoGioco;

        public Quantum()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1366;  
            graphics.PreferredBackBufferHeight = 768;
            IsMouseVisible = true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            //Crea la mappa
            // il map generator farà le sue cose e poi stabilirùà da solo le dimensioni della mappa
            MapGenerator generatore = new MapGenerator(9,15); // <- da sistemare perché i 9 non possono rimanere fissi
            Tile.CreaMappa(generatore.GeneraMappa(), generatore.Righe, generatore.Colonne);

            // Crea il gamesystem con 4 giocatori
            GameSystem gameSystem = new GameSystem();
            gameSystem.AggiungiGiocatori(2);
            
            //L'evento InizioPartita viene generato dopo che sono state disposte le pedine iniziali
            //avviene una volta per partita e lo associamo manualmente a tutti gli oggetti
            //che lo utilizzeranno
            gameSystem.InizioPartita += InizioPartita;

            // QUESTA RIGA SERVE SOLO PER TESTARE IL POSIZIONAMENTO DELLE NAVI
            gameSystem.IniziaSetupPartita();
            // DA TOGLIERE

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            var gui = Services.GetService<GuiManager>();

            /*
            QUI POSSIAMO CARICARE L'INTERFACCIA
            */

            Bottone passaTurno = Bottone.Standard(bottone.Passa, 82, 85);
            Bottone boh = Bottone.Standard(bottone.Ricerca, 82, 75);
            gui.Iscrivi(passaTurno);
            gui.Iscrivi(boh);

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


            base.Draw(gameTime);
        }

        /*  Qua cominciano altre cose nostre */

        private void InizioPartita(object sender, EventArgs args)
        {
            Debug.WriteLine("Partita iniziata!!");
        }       
    }
}
