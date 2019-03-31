using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private Vector3 camPositionVector;
        private Vector3 camEyeVector;
        private Vector3 deltaVector;
        private Vector3 mouseInputDelta;
        private Camera camera;

        public Game1()
        {
            
            graphics = new GraphicsDeviceManager(this);
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

            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), screenX / screenY, 0.1f, 1000f);

            mapClient = new PlotClient(Content, 11, 11, 1.0f);
            mapClient.SetPlotDictionary();
            mapClient.SetPlotList();
            mapClient.PrintPlotList();

            this.IsMouseVisible = true;
            
            camEyeVector = new Vector3(0, 0, 0);
            Debug.WriteLine("camEyeVector" + camEyeVector.X + " " + camEyeVector.Y + " " + camEyeVector.Z);
            camPositionVector = Vector3.Add(new Vector3(50, 0, 0), new Vector3(0, 1.6f, 0));
            deltaVector = new Vector3(0.001f, 0, 0);
            camera = new Camera(theCamera, camPositionVector, camEyeVector, deltaVector);
            
            Mouse.SetPosition((int)centerX, (int)centerY);
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

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            inputHandlers = new InputHandler(screenX, screenY);
            mouseInputDelta = inputHandlers.MouseHandler(screenX, screenY, 1.00f);
            InputHandler.Direction testInput = inputHandlers.KeyboardHandler(this);
            InputDown(testInput);

            // calculate pitch axis for rotating, therefore the orthogonal between the forward and up 
            // assuming righthandedness
            Vector3 pitchAxis = Vector3.Cross(deltaVector, Vector3.Up);
            pitchAxis.Normalize();

            deltaVector = camera.CameraUpdate(deltaVector, pitchAxis, mouseInputDelta.Y, mouseInputDelta);
            deltaVector = camera.CameraUpdate(deltaVector, Vector3.Up, -mouseInputDelta.X, -mouseInputDelta);
            
            //camPositionVector *= deltaVector; // this is the correct multiply
            camEyeVector = camPositionVector + deltaVector; // this is the correct add

            theCamera = Matrix.CreateLookAt(camPositionVector, camEyeVector, Vector3.Up);
            

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

        //placeholder for keyboard movement
        private void InputDown(InputHandler.Direction direction)
        {
            Debug.WriteLine("Input Down: " + direction);
            deltaVector.Normalize();

            if(direction == InputHandler.Direction.Forwards)
            {
                //getting the invers of the rotation vector
                Quaternion inverseQuaternion = Quaternion.Inverse(new Quaternion(deltaVector.X, deltaVector.Y, deltaVector.Z, 0));
                // removing the  quaternion rotation and getting the "front" heading
                Vector3 tempDeltaVector = deltaVector * new Vector3(inverseQuaternion.X, inverseQuaternion.Y, inverseQuaternion.Z);
                // using the "front" heading and translating it;
                //camPositionVector -= 3f * tempDeltaVector;
                
                camPositionVector += 3f * deltaVector;

                Debug.WriteLine("position Vector: " + camPositionVector.X + " " + camPositionVector.Y + " " + camPositionVector.Z);
            }

            if(direction == InputHandler.Direction.Backwards)
            {
                Quaternion inverseQuaternion = Quaternion.Inverse(new Quaternion(deltaVector.X, deltaVector.Y, deltaVector.Z, 0));
                Vector3 tempDeltaVector = Vector3.Cross( deltaVector, new Vector3(inverseQuaternion.X, inverseQuaternion.Y, inverseQuaternion.Z));
                //camPositionVector += 3f * tempDeltaVector;

                camPositionVector -= 3f * deltaVector;

                Debug.WriteLine("position Vector: " + camPositionVector.X + " " + camPositionVector.Y + " " + camPositionVector.Z);
            }

            if (direction == InputHandler.Direction.Left)
            {
        
                Quaternion inverseQuaternion = Quaternion.Inverse(new Quaternion(deltaVector.X, deltaVector.Y, deltaVector.Z, 0));
                Vector3 tempDeltaVector = Vector3.Cross( Vector3.Up, deltaVector);
                tempDeltaVector.Normalize();
                camPositionVector += 3 * tempDeltaVector;
                //camPositionVector *= -5 * deltaVector;
                Debug.WriteLine("position Vector: " + camPositionVector.X + " " + camPositionVector.Y + " " + camPositionVector.Z);
            }

            if(direction == InputHandler.Direction.Right)
            {
                Quaternion inverseQuaternion = Quaternion.Inverse(new Quaternion(deltaVector.X, deltaVector.Y, deltaVector.Z, 0));
                Vector3 tempDeltaVector = Vector3.Cross(Vector3.Up, deltaVector);
                tempDeltaVector.Normalize();
                camPositionVector -= 3 * tempDeltaVector;
                //camPositionVector *= 5 * deltaVector;
                Debug.WriteLine("position Vector: " + camPositionVector.X + " " + camPositionVector.Y + " " + camPositionVector.Z);
            }
        }


    }
}
