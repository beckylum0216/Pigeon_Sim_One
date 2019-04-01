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
        private Camera camera;

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

            int centerX = (int)(screenX / 2);
            int centerY = (int)(screenY / 2);

            cameraSpeed = 2f;
            fps = 60f;

            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), screenX / screenY, 0.1f, 8000f);

            mapClient = new PlotClient(Content, 11, 11, 1.0f);
            mapClient.SetPlotDictionary();
            mapClient.SetPlotList();
            mapClient.PrintPlotList();

            this.IsMouseVisible = true;
            
            Vector3 camEyeVector = new Vector3(0, 0, 0);
            Debug.WriteLine("camEyeVector" + camEyeVector.X + " " + camEyeVector.Y + " " + camEyeVector.Z);
            Vector3 camPositionVector = Vector3.Add(new Vector3(50, 0, 0), new Vector3(0, 1.6f, 0));
            Vector3 deltaVector = new Vector3(0.001f, 0, 0);
            camera = new Camera(theCamera, camPositionVector, camEyeVector, deltaVector);
            Mouse.SetPosition((int)centerX, (int)centerY);

            Song birdSong = Content.Load<Song>("Audio/Pigeon-Song");
            MediaPlayer.Play(birdSong);
            MediaPlayer.IsRepeating = true;

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

            //setting up collisions
            for(int ii = 0; ii < mapClient.GetPlotList().Count; ii++)
            {
                camera.AABBResolution(mapClient.GetPlotList()[ii]);
            }
            

            //int screenX = GraphicsDevice.Viewport.Width;
            //int screenY = GraphicsDevice.Viewport.Height;

            int screenX = Window.ClientBounds.Width;
            int screenY = Window.ClientBounds.Height;

            int centerX = (int)(screenX / 2);
            int centerY = (int)(screenY / 2);

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            inputHandlers = new InputHandler(screenX, screenY);
            mouseInputDelta = inputHandlers.MouseHandler(screenX, screenY, 1.00f);
            InputHandler.Direction keyboardInput = inputHandlers.KeyboardHandler(this);
            //InputDown(keyboardInput);

            camera.CameraMove(keyboardInput, cameraSpeed, deltaTime, fps);

            theCamera = camera.ActorUpdate(mouseInputDelta);

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

            base.Draw(gameTime);
        }
        
        private void PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.GraphicsProfile = GraphicsProfile.HiDef;
        }

    }
}
