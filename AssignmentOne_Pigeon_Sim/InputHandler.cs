using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AssignmentOne_Pigeon_Sim
{
    /// This class implements inputs handling
    /// @see https://www.gamefromscratch.com/post/2015/06/28/MonoGame-Tutorial-Handling-Keyboard-Mouse-and-GamePad-Input.aspx
    public class InputHandler
    {
        public enum keyStates { Forwards, Backwards, Left, Right, NULL, Pigeon, FPS, CW, CCW, ZoomIn, ZoomOut};
        private KeyboardState keyboardInput;
        private MouseState mouseInput;
        private GamePadState gamePadInput;

        private Vector3 mouseDelta;
        private Vector3 mousePosition;

        public InputHandler(int screenX, int screenY)
        {
            this.mouseDelta = new Vector3(0, 0, 0);
            float centerX = (float)screenX / 2;
            float centerY = (float)screenY / 2;
            this.mousePosition = new Vector3(centerX, centerY, 0);


        }


        
        /** 
        *   @brief function for dealing with mouse inputs
        *   @see https://www.gamasutra.com/blogs/JoshSutphin/20130416/190541/Doing_Thumbstick_Dead_Zones_Right.php
        *	@param screenX the total size of the window width
        *	@param screenY the total size the window height
        *	@param mouseSensitivity dampenerfor the mouse
        *	@param 
        *	@param 
        *	@param 
        *	@param 
        *	@return mouseDelta the slice of mouse movement locked between 0 and 2 degrees
        *	@pre 
        *	@post 
        */
        public Vector3 MouseHandler(int screenX, int screenY, float mouseSensitivity)
        {
            float magnitude = 0.25f;
            float centerX = (float)screenX / 2;
            float centerY = (float)screenY / 2;

            // get mouse input
            mouseInput = Mouse.GetState();

            /// load mouse input into vector and calculate magnitude
            Vector3 inputVector = new Vector3(mouseInput.X, mouseInput.Y, 0);
            Vector3 centerVector = new Vector3(centerX, centerY, 0);
            Vector3 positionVector = Vector3.Subtract(inputVector, centerVector);

            /// if magnitude of vector is greater than radius of x
            if (positionVector.Length() < magnitude)
            {
                mouseDelta = new Vector3(0, 0, 0);
            }
            else
            {
                /// @see https://books.google.com.au/books?id=RFF0AgAAQBAJ&pg=PA98&lpg=PA98&dq=deadzone+implementation+game+algorithm&source=bl&ots=fZCDZUNrPf&sig=ACfU3U3M4KSKOIelMGPKC9LFrcELk5aZTA&hl=en&sa=X&ved=2ahUKEwja_dnNkZzhAhUIeisKHaaTCzwQ6AEwAXoECAkQAQ#v=onepage&q=deadzone%20implementation%20game%20algorithm&f=false
                /// calculate distance and work out the proportion of distance to center
                float percent = ((float)positionVector.Length() - magnitude) / (magnitude + positionVector.Length());
                positionVector.Normalize();

                /// restricting values between 0 and 2 degrees
                mouseDelta = Vector3.Multiply(positionVector, percent * 2);

                // only use when restricting between 0 and 1
                //mouseDelta.Normalize();

            }
            
            /// reset the mouse cursor to the middle of the screen
            Mouse.SetPosition((int)centerX, (int)centerY);

            return mouseDelta;
        }

        /** 
        *   @brief function for dealing with keyboardinputs
        *   @see 
        *	@param 
        *	@param 
        *	@param 
        *	@param 
        *	@param 
        *	@param 
        *	@param 
        *	@return direction state the discrete movement from the keyboard
        *	@pre 
        *	@post 
        */
        public keyStates KeyboardHandler()
        {
            keyboardInput = Keyboard.GetState();
            keyStates directionState = keyStates.NULL;

            if (keyboardInput.IsKeyDown(Keys.W))
            {
                Debug.WriteLine("KeyDown W!!!");
                directionState = keyStates.Forwards;
            }

            if (keyboardInput.IsKeyDown(Keys.S))
            {
                directionState = keyStates.Backwards;
            }

            if (keyboardInput.IsKeyDown(Keys.A))
            {
                directionState = keyStates.Left;
            }

            if (keyboardInput.IsKeyDown(Keys.D))
            {
                directionState = keyStates.Right;
            }

            if (keyboardInput.IsKeyDown(Keys.E))
            {
                directionState = keyStates.CW;
            }

            if (keyboardInput.IsKeyDown(Keys.Q))
            {
                Debug.WriteLine("Pressed Q");
                directionState = keyStates.CCW;
            }

            if (keyboardInput.IsKeyDown(Keys.D1))
            {
                directionState = keyStates.FPS;
            }

            if (keyboardInput.IsKeyDown(Keys.D2))
            {
                directionState = keyStates.Pigeon;
            }

            if(keyboardInput.IsKeyDown(Keys.Z))
            {
                directionState = keyStates.ZoomIn;
            }
            
            
            if(keyboardInput.IsKeyDown(Keys.C))
            {
                directionState = keyStates.ZoomOut;
            }
            

            // this is a hack
            return directionState;
        }

        /** 
        *   @brief function for dealing with xbox controller inputs
        *   @see 
        *	@param screenX the total width of the window
        *	@param screenY the total height of the window
        *	@param gamePadSensitivity 
        *	@param 
        *	@param 
        *	@param 
        *	@param 
        *	@return direction state the discrete movement from the keyboard
        *	@pre 
        *	@post 
        */
        public Vector3 RightGamePadHandler(int screenX, int screenY, float gamePadSensitivity)
        {
            float magnitude = 0.25f;
            float centerX = (float)screenX / 2;
            float centerY = (float)screenY / 2;

            // get gamepad input
            gamePadInput = GamePad.GetState(PlayerIndex.One);

            // load gamepad input into vector and calculate magnitude
            Vector3 inputVector = new Vector3(gamePadInput.ThumbSticks.Right.X, gamePadInput.ThumbSticks.Right.Y, 0);
            Debug.WriteLine("Game Pad input: " + gamePadInput.ThumbSticks.Right.X + " " + gamePadInput.ThumbSticks.Right.Y);
            //Vector3 centerVector = new Vector3(centerX, centerY, 0);
            //Vector3 positionVector = Vector3.Subtract(inputVector, centerVector);
            Vector3 positionVector = inputVector;

            // if magnitude of vector is greater than radius of x
            if (gamePadInput.IsConnected)
            {
                if (positionVector.Length() < magnitude)
                {
                    mouseDelta = new Vector3(0, 0, 0);
                }
                else
                {
                    /// @see https://books.google.com.au/books?id=RFF0AgAAQBAJ&pg=PA98&lpg=PA98&dq=deadzone+implementation+game+algorithm&source=bl&ots=fZCDZUNrPf&sig=ACfU3U3M4KSKOIelMGPKC9LFrcELk5aZTA&hl=en&sa=X&ved=2ahUKEwja_dnNkZzhAhUIeisKHaaTCzwQ6AEwAXoECAkQAQ#v=onepage&q=deadzone%20implementation%20game%20algorithm&f=false
                    /// calculate distance and work out the proportion of distance to center
                    float percent = ((float)positionVector.Length() - magnitude) / (magnitude + positionVector.Length());
                    positionVector.Normalize();

                    /// restricting values between 0 and 2 degrees
                    mouseDelta = Vector3.Multiply(positionVector, percent * 2);

                    // only use when restricting between 0 and 1
                    //mouseDelta.Normalize();

                    Debug.WriteLine("Mouse Vector: " + mouseDelta.X + " " + mouseDelta.Y + " " + mouseDelta.Z);

                }
            }

            Mouse.SetPosition((int)centerX, (int)centerY);

            return mouseDelta;
        }

        /** 
        *   @brief function for dealing with xbox controller analog inputs
        *   @see 
        *	@param 
        *	@param 
        *	@param  
        *	@param 
        *	@param 
        *	@param 
        *	@param 
        *	@return direction state the discrete movement from the xbox controller
        *	@pre 
        *	@post 
        */
        public keyStates LeftGamePadHandler()
        {
            // get gamepad input
            gamePadInput = GamePad.GetState(PlayerIndex.One);
            keyStates directionState = keyStates.NULL;

            if(gamePadInput.IsConnected)
            {
                if (gamePadInput.DPad.Up == ButtonState.Pressed)
                {
                   
                    directionState = keyStates.Forwards;
                }

                if (gamePadInput.DPad.Down == ButtonState.Pressed)
                {
                    directionState = keyStates.Backwards;
                }

                if (gamePadInput.DPad.Left == ButtonState.Pressed)
                {
                    directionState = keyStates.Left;
                }

                if (gamePadInput.DPad.Right == ButtonState.Pressed)
                {
                    directionState = keyStates.Right;
                }

                if (gamePadInput.Buttons.RightShoulder == ButtonState.Pressed)
                {
                    directionState = keyStates.CW;
                }

                if (gamePadInput.Buttons.LeftShoulder == ButtonState.Pressed)
                {
                    Debug.WriteLine("Pressed Q");
                    directionState = keyStates.CCW;
                }

                if (keyboardInput.IsKeyDown(Keys.D1))
                {
                    directionState = keyStates.FPS;
                }

                if (keyboardInput.IsKeyDown(Keys.D2))
                {
                    directionState = keyStates.Pigeon;
                }
                
            }
            
            // this is a hack
            return directionState;

        }

    }
    
}
