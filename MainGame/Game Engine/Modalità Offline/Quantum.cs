using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System;
using Quantum_Game.Interfaccia;
using Quantum_Game.Mappa;



namespace Quantum_Game
{
    /* Core della Modalità Off-Line ("Single"Player) */
    public class Quantum : Game
    {
        //Componenti del motore di gioco
        private GameSystem gameSystem;
        private FlussoDiGioco flussoGioco;
       
        //Componenti del motore grafico
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GuiManager gui;


        /* Costruttore di default che carica i settings salvati nel file settings.config  */
        public Quantum()
        {
            //creazione motore di giogo
            gameSystem = new GameSystem();
            Services.AddService(gameSystem);

            //perché gamesystem è un servizio e flussoDiGioco un componente?
            flussoGioco = new FlussoDiGioco(this);
            Components.Add(flussoGioco);

            //creazione motore grafico
            graphics = new GraphicsDeviceManager(this);

            gui = new GuiManager(this);
            gui = Services.GetService<GuiManager>();
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

            /*Crea la mappa.
            //TODO: Per ora il percorso del file sta qui, poi potrebbe essere una selezione tra vari preset,
            o addirittura un map editor integrato nel gioco, basta fargli scrivere un txt con la mappa!
            */
            MapGenerator generatore = new MapGenerator(@"Data Content\Mappe\mappaeasy.txt");
            Tile.CreaMappa(generatore.GeneraMappa(), generatore.Righe, generatore.Colonne);

            //carichiamo lo sfondo per la mappa
            Sfondo sfondo = new Sfondo(this);
            Components.Add(sfondo);

            // Imposta il numero di giocatori
            this.gameSystem.AggiungiGiocatori(2);

            //Mettiamo il gamesystem in attesa di un evento InizioPartita che viene generato dopo la disposizione delle pedine iniziali
            gameSystem.InizioPartita += InizioPartita;

            base.Initialize();

        }

        //crea l'interfaccia grafica con le sue componenti
        protected override void LoadContent()
        {
            //Per qualche motivo mistorioso lo spriteBatch va preso in LoadContent()
            spriteBatch = Services.GetService<SpriteBatch>();
            
          //TODO: da sistemare


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
            Bottone colonizza = new Bottone(bottone.Colonizza, bott1);
            Bottone iniziaPartita = new Bottone(bottone.IniziaPartita, bott2);
            Bottone passaTurno = new Bottone(bottone.Passa, bott4);
            Bottone ricerca = new Bottone(bottone.Ricerca, bott3) ;

            gui.Iscrivi(colonizza);
            gui.Iscrivi(passaTurno);
            gui.Iscrivi(ricerca);
            gui.Iscrivi(iniziaPartita);

            gui.Iscrivi(tab2);


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
            flussoGioco.Update();

            base.Update(gameTime);
        }
   
        protected override void Draw(GameTime gameTime)
        {
           spriteBatch.Begin();
           
            //fa uno scan delle componenti in cerca di cose da fare
           foreach (IGameComponent component in Components)
            {

                //disegna lo sfondo
                if (component is Sfondo)
                {
                    ((Sfondo)component).Draw();
                }
            }
               
            base.Draw(gameTime);

            spriteBatch.End();
        }

        //trigger che si attiva ad inizio partita (?)

        private void InizioPartita(object sender, EventArgs args)
        {
            ConsoleMessaggi.NuovoMessaggio("Partita iniziata!!");
            Debug.WriteLine("Partita iniziata!!");
        }       

        public void loadSettings()
        {
            //TODO: 1) mettere dei settings di default nel file settings.config; 
            //TODO: 2) sostituire tutto quello con una istruzione che carica il contenuto di settings.config.
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 576;
            graphics.IsFullScreen = false;
            IsMouseVisible = true;
            graphics.ApplyChanges();
        }
    }
}
