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
    class InputHandler
    {
        public enum Direction { Forwards, Backwards, Left, Right, NULL };
        KeyboardState keyboardInput;
        MouseState mouseInput;
        Vector3 mouseDelta;
        Vector3 mousePosition;

        public InputHandler(int screenX, int screenY)
        {
            this.mouseDelta = new Vector3(0, 0, 0);
            float centerX = (float)screenX / 2;
            float centerY = (float)screenY / 2;
            this.mousePosition = new Vector3(centerX, centerY, 0);

        }

        public Vector3 MouseHandler( int screenX, int screenY, float mouseSensitivity)
        {
            float magnitude = 0.25f;
            float centerX = (float)screenX / 2;
            float centerY = (float)screenY / 2;

            
            // get mouse input
            mouseInput = Mouse.GetState();

            // load mouse input into vector and calculate magnitude
            Vector3 inputVector = new Vector3(mouseInput.X, mouseInput.Y, 0);
            Vector3 centerVector = new Vector3(centerX, centerY, 0);
            Vector3 positionVector = Vector3.Subtract(inputVector, centerVector);

            // Debug.WriteLine("Input Vector: " + inputVector.X + " " + inputVector.Y + " " + inputVector.Length());
            // Debug.WriteLine("Center Input: " + centerVector.X + " " + centerVector.Y + " " + centerVector.Length());
            // Debug.WriteLine("Mouse Input: "+ positionVector.X +" "+ positionVector.Y + " " + positionVector.Length());
            


            // if magnitude of vector is greater than radius of x
            if ( positionVector.Length() < magnitude)
            {
                mouseDelta = new Vector3(0, 0, 0);
            }
            else
            {
                // https://books.google.com.au/books?id=RFF0AgAAQBAJ&pg=PA98&lpg=PA98&dq=deadzone+implementation+game+algorithm&source=bl&ots=fZCDZUNrPf&sig=ACfU3U3M4KSKOIelMGPKC9LFrcELk5aZTA&hl=en&sa=X&ved=2ahUKEwja_dnNkZzhAhUIeisKHaaTCzwQ6AEwAXoECAkQAQ#v=onepage&q=deadzone%20implementation%20game%20algorithm&f=false
                // calculate distance and work out the proportion of distance to center
                float percent = ((float)positionVector.Length() - magnitude) / (magnitude + positionVector.Length());
                positionVector.Normalize();

                // restricting values between 0 and 2 degrees
                mouseDelta = Vector3.Multiply(positionVector,  percent * 2);
                
                // only use when restricting between 0 and 1
                //mouseDelta.Normalize();
                
            }

            // Debug.WriteLine("Mouse Vector: " + mouseDelta.X + " " + mouseDelta.Y + " " + mouseDelta.Z);

            Mouse.SetPosition((int)centerX, (int)centerY);
            
            return mouseDelta;
        }

        
        public Direction KeyboardHandler(Game1 game)
        {
            keyboardInput = Keyboard.GetState();
            Direction directionState = Direction.NULL;

            if(keyboardInput.IsKeyDown(Keys.W))
            {
                Debug.WriteLine("KeyDown W!!!");
                directionState = Direction.Forwards;
            }

            if(keyboardInput.IsKeyUp(Keys.W))
            {
                directionState = Direction.NULL;
            }

            if(keyboardInput.IsKeyDown(Keys.S))
            {
                directionState = Direction.Backwards;
            }

            if(keyboardInput.IsKeyDown(Keys.A))
            {
                directionState = Direction.Left;
            }

            if(keyboardInput.IsKeyDown(Keys.D))
            {
                directionState = Direction.Right;
            }

            if(keyboardInput.IsKeyDown(Keys.Q))
            {
                Debug.WriteLine("Pressed Q");
                game.Exit();
            }

            // this is a hack
            return directionState;
        }
        
    }
}
