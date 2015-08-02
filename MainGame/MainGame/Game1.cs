using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Quantum_Game
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Tabellone tabellone;

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
            // TODO: Add your initialization logic here

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
            Texture2D textureCaselle = Content.Load<Texture2D>("Graphica\\TileSet_prova1");
            tabellone = new Tabellone(textureCaselle, 1, 1);

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
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //tabellone.DisegnaCaselleQuadrante(spriteBatch, 100, 100, 1, 1);
            tabellone.DisegnaCaselleQuadrante(spriteBatch, 100, 100, 1, 4);
            //tabellone.DisegnaCaselleQuadrante(spriteBatch, 100, 100, 1, 7);
            tabellone.DisegnaCaselleQuadrante(spriteBatch, 100, 100, 4, 1);
            tabellone.DisegnaCaselleQuadrante(spriteBatch, 100, 100, 4, 4);
            tabellone.DisegnaCaselleQuadrante(spriteBatch, 100, 100, 4, 7);
            //tabellone.DisegnaCaselleQuadrante(spriteBatch, 100, 100, 7, 1);
            tabellone.DisegnaCaselleQuadrante(spriteBatch, 100, 100, 7, 4);
            //tabellone.DisegnaCaselleQuadrante(spriteBatch, 100, 100, 7, 7);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
