using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AssignmentOne_Pigeon_Sim
{
    class Pigeon : Actor
    {
        Camera camera;
        private Vector3 cameraDelta;
        private Vector3 cameraPosition;
        private Vector3 cameraEye;
        private Quaternion deltaQuaternion;

        public Pigeon(ContentManager Content, String modelFile, String textureFile, Vector3 inputPosition, 
                        Vector3 inputRotation, float inputScale, Vector3 inputAABBOffset, Camera inputCamera)
        {
            this.modelPath = modelFile;
            this.texturePath = textureFile;
            this.actorModel = Content.Load<Model>(modelPath);
            this.actorTexture = Content.Load<Texture2D>(texturePath);
            this.actorPosition = inputPosition;
            this.actorRotation = inputRotation;
            this.actorScale = inputScale;
            this.AABBOffset = inputAABBOffset;
            this.maxPoint = this.actorPosition + this.AABBOffset;
            this.minPoint = this.actorPosition - this.AABBOffset;
            this.camera = inputCamera;

            cameraDelta = new Vector3(0, 5, -20);
            cameraPosition = Vector3.Add(actorPosition, cameraDelta);
        }

        public override Matrix ActorUpdate(Vector3 inputVector)
        {
            // calculate pitch axis for rotating, therefore the orthogonal between the forward and up 
            // assuming righthandedness
            Vector3 pitchAxis = Vector3.Cross(actorRotation, Vector3.Up);
            pitchAxis.Normalize();

            actorRotation = PigeonUpdate(actorRotation, pitchAxis, inputVector.Y, inputVector);
            actorRotation = PigeonUpdate(actorRotation, Vector3.Up, -inputVector.X, -inputVector);

            camera.actorRotation = camera.CameraUpdate(actorRotation, pitchAxis, inputVector.Y, inputVector);
            camera.actorRotation = camera.CameraUpdate(actorRotation, Vector3.Up, -inputVector.X, -inputVector);

            actorPosition = FloorCheck();

            //

            cameraEye = cameraPosition + camera.actorRotation; // this is the correct 
            
            Matrix tempCameraObj = Matrix.CreateLookAt(cameraPosition, cameraEye, Vector3.Up);

            return tempCameraObj;
        }

        public void ActorMove(InputHandler.keyStates direction, float cameraSpeed, float deltaTime, float fps)
        {
            //Debug.WriteLine("Input Down: " + direction);
            actorRotation.Normalize();

            if (direction == InputHandler.keyStates.Forwards)
            {

                actorPosition += cameraSpeed * actorRotation * deltaTime * fps;
                cameraPosition = Vector3.Add(actorPosition, cameraDelta);

                Debug.WriteLine("position Vector: " + actorPosition.X + " " + actorPosition.Y + " " + actorPosition.Z);
            }

            if (direction == InputHandler.keyStates.Backwards)
            {

                actorPosition -= cameraSpeed * actorRotation * deltaTime * fps;
                cameraPosition = Vector3.Add(actorPosition, cameraDelta);

                Debug.WriteLine("position Vector: " + actorPosition.X + " " + actorPosition.Y + " " + actorPosition.Z);
            }

            if (direction == InputHandler.keyStates.Left)
            {
                Vector3 tempDeltaVector = Vector3.Cross(Vector3.Up, actorRotation);
                tempDeltaVector.Normalize();
                actorPosition += cameraSpeed * tempDeltaVector * deltaTime * fps;
                cameraPosition = Vector3.Add(actorPosition, cameraDelta);
                //actorPosition *= -5 * deltaVector;
                Debug.WriteLine("position Vector: " + actorPosition.X + " " + actorPosition.Y + " " + actorPosition.Z);
            }

            if (direction == InputHandler.keyStates.Right)
            {
                Vector3 tempDeltaVector = Vector3.Cross(Vector3.Up, actorRotation);
                tempDeltaVector.Normalize();
                actorPosition -= cameraSpeed * tempDeltaVector * deltaTime * fps;
                cameraPosition = Vector3.Add(actorPosition, cameraDelta);
                //actorPosition *= 5 * deltaVector;

                Debug.WriteLine("position Vector: " + actorPosition.X + " " + actorPosition.Y + " " + actorPosition.Z);
            }

            if(direction == InputHandler.keyStates.CW)
            {
                Debug.WriteLine("Camera before: " + cameraPosition);

                cameraDelta = cameraSpeed * OrbitCW() * deltaTime * fps;
                cameraPosition = Vector3.Add(actorPosition, cameraDelta);

                Debug.WriteLine("Camera After: " + cameraPosition);

            }

            if(direction == InputHandler.keyStates.CCW)
            {
                cameraDelta = cameraSpeed * OrbitCCW() * deltaTime * fps;
                cameraPosition = Vector3.Add(actorPosition, cameraDelta);
            }
        }

        public Vector3 PigeonUpdate(Vector3 deltaVector, Vector3 targetAxis, float inputDegrees, Vector3 inputVector)
        {

            if (inputVector.Length() > 0)
            {
                float radianInput = ActorRadians(inputDegrees);

                deltaQuaternion = new Quaternion(deltaVector.X, deltaVector.Y, deltaVector.Z, 0);

                Quaternion resultQuaternion = RotateCamera(radianInput, targetAxis, deltaQuaternion);

                actorRotation = new Vector3(resultQuaternion.X, resultQuaternion.Y, resultQuaternion.Z);

                radianInput = 0;

                return actorRotation;

            }
            else
            {

                return deltaVector;

            }

        }

        // http://in2gpu.com/2016/03/14/opengl-fps-camera-quaternion/
        // qpq'
        private Quaternion RotateCamera(float inputAngle, Vector3 inputAxis, Quaternion pQuat)
        {

            Quaternion qQuat = AxisAngle(inputAngle, inputAxis);

            Quaternion pqQuat = Quaternion.Multiply(qQuat, pQuat);

            Quaternion resultQuat = Quaternion.Multiply(pqQuat, Quaternion.Inverse(qQuat));

            deltaQuaternion = resultQuat;

            return resultQuat;
        }

        private Quaternion AxisAngle(float theTheta, Vector3 inputAxis)
        {
            Quaternion rotationQuart;
            double sinTheta;

            // Normalise rotation axis to have unit length
            inputAxis.Normalize();

            sinTheta = Math.Sin(theTheta / 2);

            // Convert to rotation quaternion
            rotationQuart.X = (float)(inputAxis.X * sinTheta);  // theTheta should be in radians
            rotationQuart.Y = (float)(inputAxis.Y * sinTheta);
            rotationQuart.Z = (float)(inputAxis.Z * sinTheta);
            rotationQuart.W = (float)(Math.Cos(theTheta / 2));

            return rotationQuart;
        }

        private Vector3 FloorCheck()
        {
            if (actorPosition.Y <= 0)
            {
                Vector3 tempVector = new Vector3(actorPosition.X, 0, actorPosition.Z);

                return tempVector;
            }
            else
            {
                return actorPosition;
            }
        }

        private Vector3 OrbitCW()
        {
            
            return Orbit(3);
        }

        private Vector3 OrbitCCW()
        {

            return Orbit(-3);
        }

        private Vector3 Orbit(float inputDegrees)
        {
            float radian = ActorRadians(inputDegrees);

            Vector3 tempVector = cameraPosition - actorPosition;

            Vector3 rotateVector = Vector3.Transform(cameraDelta, Matrix.CreateRotationY(radian));
            rotateVector.Normalize();
            Vector3 resultVector = rotateVector * 20;


            Debug.WriteLine("Result Vector: " + resultVector.ToString());

            return resultVector;
        }
    }
}
