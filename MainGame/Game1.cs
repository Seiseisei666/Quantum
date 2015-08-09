using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System;


namespace Quantum_Game
{
    /// <summary>
    /// Il gioco vero e proprio
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Tabellone tabellone;
        private Texture2D textureCaselle;
        private Texture2D contornoCasella;
        private MouseInput mouseInput;
        private GameSystem gameSystem;

        static public event EventHandler<ResizeEvntArgs> Ridimensionamento;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;  
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            IsMouseVisible = true; 
            mouseInput = new MouseInput();
        }

        protected override void Initialize()
        {
            // Crea il gamesystem con 4 giocatori
            gameSystem = new GameSystem();
            gameSystem.AggiungiGiocatori(4);

            //Crea la mappa
            MapGenerator gen = new MapGenerator(9, 9);
            tabellone = new Tabellone
                (gen.GeneraMappa(), gen.Righe, gen.Colonne, 0.05f, 0.1f, 800, 600);

            /*  
                ASSOCIAZIONE DEGLI EVENTI 
                Gli eventi del mouse vengono letti o ignorati a seconda del momento del gioco
                Ad esempio, una volta selezionata un'unità andiamo a leggere il click dx,
                che prima di aver effettuato una selezione valida veniva ignorato.
                Per gestire dinamicamente queste situazioni ogni riquadro dell'interfaccia grafica 
                dispone del metodo AssociaEvento
            */
            tabellone.AssociaEvento(mouseInput, TipoEventoMouse.ClkSin);

            //L'evento InizioPartita viene generato dopo che sono state disposte le pedine iniziali
            //avviene una volta per partita e lo associamo manualmente a tutti gli oggetti
            //che lo utilizzeranno
            gameSystem.InizioPartita += tabellone.InizioPartita;

            //TODO: usare i generics per fare un solo metodo AssociaEvento<TipoEvento> (object oggetto, TipoEvento tipo)


            // QUESTA RIGA SERVE SOLO PER TESTARE IL POSIZIONAMENTO DELLE NAVI
            gameSystem.FasePartita = FasiDiGioco.SetupPartita;
            // DA TOGLIERE

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textureCaselle = Content.Load<Texture2D>("TileSet_prova1");

            // texture di 1x1 pixel con alpha blending, per disegnare "a mano"
            contornoCasella = new Texture2D(GraphicsDevice, 1, 1);
            contornoCasella.SetData(new[] { (Color.White*0.5f) });
        } 

        protected override void UnloadContent()
        {
            spriteBatch.Dispose();
            contornoCasella.Dispose();
            textureCaselle.Dispose();
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit(); //questa ci stava per default, è per chiudere la finestra

            if (gameSystem.FasePartita == FasiDiGioco.PartitaInCorso)
            {

            }

            else if (gameSystem.FasePartita == FasiDiGioco.SetupPartita)
            {
                Test();
            }

            mouseInput.Update(); // routine di aggiornamento dell'input del mouse, di cui si occupa
                                 // l'oggetto mouseInput

            base.Update(gameTime);
        }

        // PROVA!!!!!!!!
        private void Test()
        {
            Casella tempCas = tabellone.TileSelezionato as Casella; // prova a castare il tile selezionato come casella

            if (tempCas != null && tempCas.Occupante == null) // se ho una casella valida e non occupata da una nave...
            {
                gameSystem.GiocatoreDiTurno.PiazzaNuovaNave(tempCas);
                if (gameSystem.GiocatoreDiTurno.Flotta.Count == 3) gameSystem.NextTurn();
            }
        }
   
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // qui cominciano le routine di disegno dei vari oggetti
            spriteBatch.Begin();

            tabellone.Draw(spriteBatch, textureCaselle);
            tabellone.DisegnaSelezione(spriteBatch, contornoCasella);

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



    }
}
