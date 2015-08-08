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
            MapGenerator gen = new MapGenerator(6, 9);
            tabellone = new Tabellone
                (gen.GeneraMappa(), gen.Righe, gen.Colonne, 0.3f, 0.1f, 800, 600);

            tabellone.AssociaEvento(mouseInput, TipoEventoMouse.ClkSin);

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textureCaselle = Content.Load<Texture2D>("Graphica\\TileSet_prova1");
            // texture con alpha blending per evidenziare la casella selezionata
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
                Exit();

            mouseInput.Update();

            base.Update(gameTime);
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


        //metodi x far partire gli eventi!!!
        protected virtual void OnRidimensionamento(ResizeEvntArgs args) //evento ipotetico per gestire il ridimensionamento delle finestre
        {
            if (Ridimensionamento != null)
                Ridimensionamento(this, args);
        }



    }
}
