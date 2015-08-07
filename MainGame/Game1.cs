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
         
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Tabellone tabellone;
        private Texture2D textureCaselle;
        private Texture2D contornoCasella;
        private MouseState mouseState, oldState;

        static public event EventHandler<ResizeEvntArgs> Ridimensionamento;
        static public event EventHandler<MouseEvntArgs> MouseClick;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 800;  
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            //graphics.ToggleFullScreen;
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            MapGenerator gen = new MapGenerator(3, 5);
            tabellone = new Tabellone(gen.GeneraMappa(), 3, 5, 0.1f, 0.1f, 800, 600);

            base.Initialize();
        }



        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textureCaselle = Content.Load<Texture2D>("Graphica\\TileSet_prova1");
            contornoCasella = new Texture2D(GraphicsDevice, 1, 1);
            contornoCasella.SetData(new[] { (Color.White) });
        


            // TODO: use this.Content to load your game content here
        }



        protected override void UnloadContent()
        {
            spriteBatch.Dispose();

            // TODO: Unload any non ContentManager content here
        }

     

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
            mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
            {
                //  int x = mouseState.X; int y = mouseState.Y;

                OnMouseClick(new MouseEvntArgs(mouseState.X, mouseState.Y, true, false));

            }
            oldState = mouseState;


            base.Update(gameTime);
        }

   
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            tabellone.Draw(spriteBatch, textureCaselle);
          
            tabellone.DisegnaSelezione(spriteBatch, contornoCasella);

            spriteBatch.End();
            base.Draw(gameTime);
            
        }


        //metodi x far partire gli eventi!!!
        protected virtual void OnRidimensionamento(ResizeEvntArgs args) //evento ipotetico per gestire il ridimensionamento delle finestre
        {
            if (Ridimensionamento != null)
                Ridimensionamento(this, args);
        }
        protected virtual void OnMouseClick(MouseEvntArgs args) // evento per gestire i cilck del mouse
        {
            if (MouseClick != null)
                MouseClick(this, args);
        }


    }
}
