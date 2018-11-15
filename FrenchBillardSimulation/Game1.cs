using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FrenchBillardSimulation
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private MouseState previousState;
        private Texture2D billardTableTexture, cursorTexture, instructionTexture;
        private Vector2 billardTablePosition;

        public Vector2 cursorPosition, instructionPosition;
        public bool isLeftClick, isShooting;
        public Ball player1, player2, red;
        public float pressedClickTime;

        public Wind wind;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 600;
            isLeftClick = false;
            isShooting = false;
            pressedClickTime = 0f;
            Content.RootDirectory = "Content";
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

            billardTableTexture = Content.Load<Texture2D>("French_Billard_Table");
            billardTablePosition = new Vector2(0, 0);

            player1 = new Ball(Content.Load<Texture2D>("ball"), new Vector2(100,300), 0f, Color.White);
            player2 = new Ball(Content.Load<Texture2D>("ball"), new Vector2(750, 150), 0f, Color.White);
            cursorTexture = Content.Load<Texture2D>("Cursor");
            red = new Ball(Content.Load<Texture2D>("ball"), new Vector2(600, 300), 0f, Color.Red);

            wind = new Wind(Content.Load<Texture2D>("WindArrow"), new Vector2(1000, 100), 0f);

            instructionTexture = Content.Load<Texture2D>("Instruction");
            instructionPosition = new Vector2(25, 25);
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

            MouseState mouseState = Mouse.GetState();

            cursorPosition.X = mouseState.Position.X;
            cursorPosition.Y = mouseState.Position.Y;

            player1.Update(gameTime, cursorPosition, isLeftClick,wind.intensity,wind.angle);
            player2.Update(gameTime, cursorPosition, isLeftClick, wind.intensity, wind.angle);
            red.Update(gameTime, cursorPosition, isLeftClick, wind.intensity, wind.angle);
            wind.Update(gameTime);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                float elapse = (float)(gameTime.ElapsedGameTime.TotalSeconds);
                pressedClickTime += elapse;
                isLeftClick = true;
                isShooting = true;
                player1.shootingAngle = player1.getAngle(player1.position, cursorPosition);
            }
            else
            {
                if(previousState.LeftButton == ButtonState.Pressed && 
                    mouseState.LeftButton == ButtonState.Released && player1.isStatic)
                {
                    player1.initialVelocity = pressedClickTime * 400;
                    player1.velocity.X = (float)(player1.initialVelocity * System.Math.Cos(player1.shootingAngle));
                    player1.velocity.Y = (float)(player1.initialVelocity * System.Math.Sin(player1.shootingAngle));
                    //System.Console.WriteLine("initial Velocity: " + ball.initialVelocity);
                    isLeftClick = false;
                    pressedClickTime = 0f;
                    
                }
            }

            //Player 1 Collision with red ball (flags)
            if (System.Math.Sqrt(System.Math.Pow(player1.position.X - red.position.X,2) +
               System.Math.Pow(player1.position.Y - red.position.Y,2)) <= 
               player1.radius + red.radius)
            {
                //System.Console.WriteLine("Collision!!");
                player1.isBallCollision = true;
                red.isBallCollision = true;
                //red.velocity = player1.velocity;
            }
            else
            {
                player1.isBallCollision = false;
                red.isBallCollision = false;
            }

            if(player1.isBallCollision && red.isBallCollision)
            {
                System.Console.WriteLine("Collision!!");

                //player1.velocity.X = (float)(red.initialVelocity * System.Math.Cos(-red.shootingAngle));
                //player1.velocity.Y = (float)(red.initialVelocity * System.Math.Cos(-red.shootingAngle));

                /*player1.velocity = new Vector2((float)(player1.velocity.X * System.Math.Sin(player1.shootingAngle))
                    , (float)(player1.velocity.Y * System.Math.Sin(player1.shootingAngle)));

                red.velocity = new Vector2((float)(player1.velocity.X * System.Math.Cos(player1.shootingAngle)),
                    (float)(player1.velocity.Y * System.Math.Cos(player1.shootingAngle)));*/

                red.velocity.X = (float)(player1.initialVelocity * System.Math.Cos(
                    player1.getAngle(player1.position, red.position)));

                red.velocity.Y = (float)(player1.initialVelocity * System.Math.Cos(
                    MathHelper.ToRadians(90) - player1.getAngle(player1.position, red.position)));

                player1.velocity.X = (float)(player1.initialVelocity * System.Math.Sin(
                    MathHelper.ToRadians(90) - player1.getAngle(player1.position, red.position)));

                player1.velocity.Y = (float)(player1.initialVelocity * System.Math.Sin(
                    MathHelper.ToRadians(90) - player1.getAngle(player1.position, red.position)));

                System.Console.WriteLine("player1 velocity x,y: " + player1.velocity.X + ", " + player1.velocity.Y);
                System.Console.WriteLine("red velocity x,y: " + red.velocity.X + ", " + red.velocity.Y);

                //red.velocity.X = (float)(player1.initialVelocity * System.Math.Cos(-player1.shootingAngle));
                //red.velocity.Y = (float)(player1.initialVelocity * System.Math.Cos(-player1.shootingAngle));

            }

            //Player 1 Collision with Player 2 (flags)
            if (System.Math.Sqrt(System.Math.Pow(player1.position.X - player2.position.X, 2) +
               System.Math.Pow(player1.position.Y - player2.position.Y, 2)) <=
               player1.radius + player2.radius)
            {
                player1.isBallCollision = true;
                player2.isBallCollision = true;
            }
            else
            {
                player1.isBallCollision = false;
                player2.isBallCollision = false;
            }

            if (player1.isBallCollision && player2.isBallCollision)
            {
                player2.velocity.X = (float)(player1.initialVelocity * System.Math.Cos(
                    player1.getAngle(player1.position, player2.position)));

                player2.velocity.Y = (float)(player1.initialVelocity * System.Math.Cos(
                    MathHelper.ToRadians(90) - player1.getAngle(player1.position, player2.position)));

                player1.velocity.X = (float)(player1.initialVelocity * System.Math.Sin(
                    MathHelper.ToRadians(90) - player1.getAngle(player1.position, player2.position)));

                player1.velocity.Y = (float)(player1.initialVelocity * System.Math.Sin(
                    MathHelper.ToRadians(90) - player1.getAngle(player1.position, player2.position)));
            }

            //Player 2 Collision with Red Ball (flags)
            if (System.Math.Sqrt(System.Math.Pow(player2.position.X - red.position.X, 2) +
               System.Math.Pow(player2.position.Y - red.position.Y, 2)) <=
               player2.radius + red.radius)
            {
                player2.isBallCollision = true;
                red.isBallCollision = true;
            }
            else
            {
                player2.isBallCollision = false;
                red.isBallCollision = false;
            }

            if (player2.isBallCollision && red.isBallCollision)
            {
                player2.velocity.X = (float)(player2.initialVelocity * System.Math.Cos(
                    player2.getAngle(player2.position, red.position)));

                player2.velocity.Y = (float)(player2.initialVelocity * System.Math.Cos(
                    MathHelper.ToRadians(90) - player2.getAngle(player2.position, red.position)));

                red.velocity.X = (float)(player2.initialVelocity * System.Math.Sin(
                    MathHelper.ToRadians(90) - player2.getAngle(player2.position, red.position)));

                red.velocity.Y = (float)(player1.initialVelocity * System.Math.Sin(
                    MathHelper.ToRadians(90) - player2.getAngle(player2.position, red.position)));
            }

            previousState = mouseState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(billardTableTexture, billardTablePosition, Color.White);
            player1.Draw(spriteBatch);
            player2.Draw(spriteBatch);
            red.Draw(spriteBatch);
            spriteBatch.Draw(cursorTexture, cursorPosition, Color.White);
            wind.Draw(spriteBatch);
            spriteBatch.Draw(instructionTexture, instructionPosition, Color.Orange);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
