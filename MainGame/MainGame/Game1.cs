using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Quantum_Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Tabellone2 tabellone;
        private Texture2D textureCaselle;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 900;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 900;   // set this value to the desired height of your window
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // INIZIALIZZAZIONE E RIEMPIMENTO TABELLONE 4*4

            tabellone = new Tabellone2(5,3);
            int lancio = 0;
            
           // riempimento a caso del tabellone
            for (int i = 0; i < tabellone.Tessere.Capacity; i++)
            {
                lancio = util.Dadi();
                if (lancio == 1) tabellone.Popola(new Vuoto());
                else tabellone.Popola(new Settore(e_sett.Pianeta10));
               
            }


            Debug.WriteLine(tabellone.Tessere.Count);




            base.Initialize();


        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textureCaselle = Content.Load<Texture2D>("Graphica\\TileSet_prova1");
  

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //variabili varie che uso per disegnare i rettangoli
            int casW = 50;
            int casH = 50;
            int tabW = casW * tabellone.Colonne * 3;
            int tabH = casH * tabellone.Righe * 3;
            Rectangle target = new Rectangle();
            target.Width = casW;
            target.Height = casH;
            Rectangle source = new Rectangle();
            source.Height = 100;
            source.Width = 100;
            float x, y;
            int tempX, tempY;

            //queste non le ho usate ma l'algoritmo vero ovviamente dovrà tenere cvonto della risoluz dello schermo
            int scrW = graphics.PreferredBackBufferWidth;
            int scrH = graphics.PreferredBackBufferHeight;
        

            spriteBatch.Begin();

            //loop per disegnare i settori
            for (int sett = 0; sett < tabellone.Tessere.Count; sett++) {

                // il metodo indicesettore prende l'indice e mi rimanda le coordinate relative
                // di dove sta il settore in questione sulla plancia
                // (0,0 = angolo in alto a sin, 1,1 = angolo in basso a dx)
                tabellone.IndiceSettore2xy(sett, out x, out y);
                x *= (float)tabW; y *= (float)tabH; // moltiplico queste coord relative
                                                    // per la dimensione del tabellone che vogliamo disegnare

                for (int cas = 0; cas < 9; cas++) { //nested loop per disegnare le 9 caselle del settore

                    //offset (valori assoluti in pixel) delle 9 caselle
                    tempX = (cas % 3) * casW;
                    tempY = (cas / 3) * casH;

                    // se il settore è vuoto lo disegno vuoto e pace
                    if (tabellone.Tessere[sett].Tipo == 0) { source.X = 0; source.Y = 0; }
                    else
                    {
                        switch (cas) {

                            case 0:
                            case 2:
                            case 6:
                            case 8:
                                source.X = 0;
                                source.Y = 0;
                                break;
                            case 1:
                            case 3:
                            case 5:
                            case 7:
                                source.X = 100;
                                source.Y = 0;
                                break;
                            default:
                                source.X = 200;
                                source.Y = 0;
                                break;
                        }

                    }



                    target.X = tempX + (int)x;
                    target.Y = tempY + (int)y;

//
                 // Debug.WriteLine("X Rettangolo: " + target.X + "Y Rettangolo: " + target.Y);
                    spriteBatch.Draw(textureCaselle, target, source, Color.White);

                }

            }
            spriteBatch.End();





            base.Draw(gameTime);
        }
    }
}
