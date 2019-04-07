using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace AssignmentOne_Pigeon_Sim
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Matrix theWorld = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        private Matrix theCamera;
        private Matrix projection;
        private PlotClient mapClient;
        private InputHandler inputHandlers;
        private float cameraSpeed;
        private float fps;
        private Vector3 mouseInputDelta;
        private Pigeon pigeon;
        private Camera camera;
        private InputHandler.keyStates gameState;

        public Game1()
        {
            
            graphics = new GraphicsDeviceManager(this);
            graphics.PreparingDeviceSettings += PreparingDeviceSettings;
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;
            graphics.HardwareModeSwitch = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            int screenX = GraphicsDevice.Viewport.Width;
            int screenY = GraphicsDevice.Viewport.Height;
            //int screenX = Window.ClientBounds.Width;
            //int screenY = Window.ClientBounds.Height;
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), screenX / screenY, 0.1f, 8000f);

            int centerX = (int)(screenX / 2);
            int centerY = (int)(screenY / 2);
            
            this.IsMouseVisible = true;
            Mouse.SetPosition((int)centerX, (int)centerY);

            mapClient = new PlotClient(Content, 23, 23, 1.0f);
            mapClient.SetPlotDictionary();
            mapClient.SetPlotList();
            mapClient.PrintPlotList();
            
            Vector3 camEyeVector = new Vector3(0, 0, 0);
            Vector3 camPositionVector = Vector3.Add(new Vector3(0, 0, 0), new Vector3(0, 1.6f, 0));
            Vector3 deltaVector = new Vector3(0, 0, 0.001f);
            Vector3 AABBOffsetCamera = new Vector3(0.5f, 0.25f, 0.5f);
            camera = new Camera(theCamera, camPositionVector, camEyeVector, deltaVector, AABBOffsetCamera);
            cameraSpeed = 2f;
            fps = 60f;

            // need to singleton this
            gameState = InputHandler.keyStates.Pigeon;
            string modelPigeon = "Models/SK_Pigeon";
            string texturePigeon = "Maya/sourceimages/pigeon_normal2";
            Vector3 predictedPigeon = camPositionVector;
            Vector3 positionPigeon = camPositionVector;
            Vector3 rotationPigeon = new Vector3(-90, 0, 0) + deltaVector;
            Vector3 AABBOffsetPigeon = new Vector3(0.5f, 0.25f, 0.5f);
            float scalePigeon = 0.05f;
            pigeon = new Pigeon(Content, modelPigeon, texturePigeon, predictedPigeon, positionPigeon, rotationPigeon,
                                    scalePigeon, AABBOffsetPigeon, camera);


            Song birdSong = Content.Load<Song>("Audio/Pigeon-Song");
            // MediaPlayer.Play(birdSong);
            // MediaPlayer.IsRepeating = true;


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

            //int screenX = GraphicsDevice.Viewport.Width;
            //int screenY = GraphicsDevice.Viewport.Height;
            int screenX = Window.ClientBounds.Width;
            int screenY = Window.ClientBounds.Height;

            int centerX = (int)(screenX / 2);
            int centerY = (int)(screenY / 2);

            inputHandlers = new InputHandler(screenX, screenY);
            mouseInputDelta = inputHandlers.MouseHandler(screenX, screenY, 1.00f);
            mouseInputDelta = inputHandlers.RightGamePadHandler(screenX, screenY, 1.00f);
            InputHandler.keyStates keyboardInput = inputHandlers.KeyboardHandler(this);
            keyboardInput = inputHandlers.LeftGamePadHandler(this);
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // selects between first person and third person states 
            // copies the last known position and rotation to the game state 
            if(keyboardInput == InputHandler.keyStates.Pigeon)
            {
                gameState = InputHandler.keyStates.Pigeon;
                pigeon.actorPosition = camera.actorPosition;
                pigeon.actorRotation = camera.actorRotation;
            }
            
            if(keyboardInput == InputHandler.keyStates.FPS)
            {
                gameState = InputHandler.keyStates.FPS;
                camera.actorPosition = pigeon.actorPosition;
                camera.actorRotation = pigeon.actorRotation;
            }

            if (gameState == InputHandler.keyStates.Pigeon)
            {
                
                pigeon.ActorMove(keyboardInput, cameraSpeed, deltaTime, fps);
                
                for(int ii = 0; ii < mapClient.GetPlotList().Count; ii += 1)
                {
                    pigeon.AABBResolution(mapClient.GetPlotList()[ii], deltaTime, fps);
                }
                

                theCamera = pigeon.ActorUpdate(mouseInputDelta);
            }
            else
            {
                camera.CameraMove(keyboardInput, cameraSpeed, deltaTime, fps);
                //setting up collisions

                for(int ii = 0; ii < mapClient.GetPlotList().Count; ii += 1)
                {
                    camera.AABBResolution(mapClient.GetPlotList()[ii], deltaTime, fps);
                }
                
                theCamera = camera.ActorUpdate(mouseInputDelta);
            }
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            for (int ii = 0; ii < mapClient.GetPlotList().Count; ii++)
            {
                mapClient.GetPlotList()[ii].ActorDraw(theWorld, theCamera, projection);

            }

            if(gameState == InputHandler.keyStates.Pigeon)
            {
                pigeon.ActorDraw(theWorld, theCamera, projection);
            }

            base.Draw(gameTime);
        }
        
        private void PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.GraphicsProfile = GraphicsProfile.HiDef;
        }

    }
}
