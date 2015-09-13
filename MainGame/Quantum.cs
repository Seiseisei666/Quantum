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
        private SpriteBatch spriteBatch;

        private Sfondo sfondo;

        private FlussoDiGioco flussoGioco;

        Dummy caselle, bottonilaterali, barraindentata;
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
            string file = @"Content\Mappe\mappaeasy.txt";
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
            // DA TOGLIERE

            // CREIAMO I COMPONENTI E LI AGGIUNGIAMO ALLA RACCOLTA GAME.COMPONENTS
            Services.AddService<GameSystem>(gameSystem);

            MouseInput mouseInput = new MouseInput(this);
            Components.Add(mouseInput);
            GuiManager gui = new GuiManager(this);
            Components.Add(gui);
            flussoGioco = new FlussoDiGioco(this);
            Components.Add(flussoGioco);
            //parametri: origine x-y; larghezza; altezza;

            sfondo = new Sfondo(this);
            Components.Add(sfondo);

            base.Initialize();
        }

        protected override void LoadContent()
        {

            spriteBatch = Services.GetService<SpriteBatch>();

            var gui = Services.GetService<GuiManager>();

            /*
            QUI POSSIAMO CARICARE L'INTERFACCIA
            */

            Bottone passaTurno = Bottone.Standard(bottone.Passa, 82, 85);
            Bottone boh = Bottone.Standard(bottone.Ricerca, 82, 75);
            gui.Iscrivi(passaTurno);
            gui.Iscrivi(boh);

            var tab = gui.Tabellone;
            //ConsoleMessaggi console = new ConsoleMessaggi(tab, 100, 20);
            //gui.Iscrivi(console);

            Cimitero cim = new Cimitero(3, 73, 80, 15);
            gui.Iscrivi(cim);

            // PARTE BRUTTA
            var schermo = new Schermo(Window);

            var barra = new Riquadro(schermo, 0, 0, 100, 10);
            var tabellone = new Riquadro(schermo, 0, barra.AltRelativa, 70, 100 - barra.AltRelativa);
            var latodestro = new Riquadro(schermo, tabellone.LarghRelativa, barra.AltRelativa, 100 - tabellone.LarghRelativa, 100 - barra.AltRelativa);


            caselle = new Dummy(tabellone,0);
            barraindentata = new Dummy(barra,1);
            bottonilaterali = new Dummy(latodestro,2);


            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new[] { (Color.White) });

            Tabellone tab2 = new Tabellone(this, tabellone);
            Components.Add(tab);
            gui.Iscrivi(tab2);
            // FINE PARTE BRUTTA

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

            var spr = Services.GetService<SpriteBatch>();

            caselle.Draw(spr, texture);
            barraindentata.Draw(spr, texture);
            bottonilaterali.Draw(spr, texture);

            base.Draw(gameTime);

            spriteBatch.End();
        }

        /*  Qua cominciano altre cose nostre */

        private void InizioPartita(object sender, EventArgs args)
        {
            Debug.WriteLine("Partita iniziata!!");
        }       
    }
}
