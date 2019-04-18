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
    class Pigeon : Subject
    {
        private Camera camera;
        private Vector3 cameraDelta;
        private Vector3 cameraPosition;
        private Vector3 cameraEye;
        private Vector3 rotationDelta;
        private Quaternion deltaQuaternion;

        public Pigeon(ContentManager Content, String modelFile, String textureFile, Vector3 predictedPosition, Vector3 inputPosition, 
                        Vector3 inputRotation, float inputScale, Vector3 inputAABBOffset, Camera inputCamera)
        {
            this.modelPath = modelFile;
            this.texturePath = textureFile;
            this.subjectModel = Content.Load<Model>(modelPath);
            this.subjectTexture = Content.Load<Texture2D>(texturePath);
            this.futurePosition = predictedPosition;
            this.subjectPosition = inputPosition;
            this.subjectRotation = inputRotation;
            this.subjectScale = inputScale;
            this.AABBOffset = inputAABBOffset;
            this.maxPoint = this.subjectPosition + this.AABBOffset;
            this.minPoint = this.subjectPosition - this.AABBOffset;
            this.camera = inputCamera;

            cameraDelta = new Vector3(0, 0, -20);
            Matrix temp = Matrix.CreateRotationY(MathHelper.ToRadians(subjectRotation.Y)) * Matrix.CreateTranslation(subjectPosition);
            cameraPosition = Vector3.Add(subjectPosition, cameraDelta);
            //cameraPosition = temp.Translation + (temp.Backward * 20f);
        }

        /** 
         *  @brief update the position state of the pigeon
         *	@param inputVector the mouse inputs in vector form
         *	@param deltaTime the time the game has elapsed
         *	@param fps the frame rate
         *	@return tempCameraObj the position of the camera in matrix form
         *	@pre 
         *	@post position update of the camera object
         */
        public override Matrix SubjectUpdate(Vector3 inputVector, float deltaTime, float fps)
        {

            Debug.WriteLine("Rotation: " + subjectRotation);

            // calculate pitch axis for rotating, therefore the orthogonal between the forward and up 
            // assuming righthandedness
            Vector3 pitchAxis = Vector3.Cross(subjectRotation, Vector3.Up);
            pitchAxis.Normalize();

            // do not use actorrotation changes the "front" orientation when rotated
            //actorRotation = PigeonUpdate(actorRotation, pitchAxis, inputVector.Y, inputVector);
            //actorRotation = PigeonUpdate(actorRotation, Vector3.Up, -inputVector.X, -inputVector);

            camera.subjectRotation = camera.CameraUpdate(camera.subjectRotation, pitchAxis, inputVector.Y, inputVector);
            camera.subjectRotation = camera.CameraUpdate(camera.subjectRotation, Vector3.Up, -inputVector.X, -inputVector);

            //camera.actorRotation = PigeonUpdate(actorRotation, pitchAxis, inputVector.Y, inputVector);
            //camera.actorRotation = PigeonUpdate(actorRotation, Vector3.Up, -inputVector.X, -inputVector);

            subjectPosition = FloorCheck();

            for (int ii = 0; ii < this.GetObservers().Count; ii += 1)
            {

                if (this.GetObservers()[ii].AABBtoAABB(this))
                {
                    if (this.subjectPosition.Y > 20f)
                    {
                        this.AABBResolution(this.GetObservers()[ii], deltaTime, fps);
                    }
                    else
                    {
                        this.AABBCollider(this.GetObservers()[ii]);
                    }
                }

            }

            cameraEye = cameraPosition + camera.subjectRotation; // this is the correct 
            
            Matrix tempCameraObj = Matrix.CreateLookAt(cameraPosition, cameraEye, Vector3.Up);

            return tempCameraObj;
        }


        /** 
        *   @brief This function updates the location of the pigeon. 
        *	@param direction the direction the pigeon is travelling
        *	@param cameraSpeed the velocity of the pigeon
        *	@param deltaTime the slice of time the game has elapsed
        *	@param fps the framerate
        *	@return subjectPosition the new pigeon position    
        *	@pre 
        *	@post subjectPosition will be updated
        */
        public void SubjectMove(InputHandler.keyStates direction, float cameraSpeed, float deltaTime, float fps)
        {
            //Debug.WriteLine("Input Down: " + direction);
            

            if (direction == InputHandler.keyStates.Right)
            {
                rotationDelta = subjectRotation;
                rotationDelta.Normalize();

                //futurePosition += cameraSpeed * rotationDelta * deltaTime * fps;
                subjectPosition += cameraSpeed * rotationDelta * deltaTime * fps;

                cameraPosition = Vector3.Add(subjectPosition, cameraDelta);

            }

            if (direction == InputHandler.keyStates.Left)
            {
                rotationDelta = subjectRotation;
                rotationDelta.Normalize();
                //futurePosition -= cameraSpeed * rotationDelta * deltaTime * fps;
                subjectPosition -= cameraSpeed * rotationDelta * deltaTime * fps;
                cameraPosition = Vector3.Add(subjectPosition, cameraDelta);

            }

            if (direction == InputHandler.keyStates.Forwards)
            {
                Vector3 tempDeltaVector = Vector3.Cross(Vector3.Up, subjectRotation);
                tempDeltaVector.Normalize();
                // futurePosition += cameraSpeed * tempDeltaVector * deltaTime * fps;
                subjectPosition += cameraSpeed * tempDeltaVector * deltaTime * fps;
                cameraPosition = Vector3.Add(subjectPosition, cameraDelta);
                
            }

            if (direction == InputHandler.keyStates.Backwards)
            {
                Vector3 tempDeltaVector = Vector3.Cross(Vector3.Up, subjectRotation);
                tempDeltaVector.Normalize();
                // futurePosition -= cameraSpeed * tempDeltaVector * deltaTime * fps;
                subjectPosition -= cameraSpeed * tempDeltaVector * deltaTime * fps;
                cameraPosition = Vector3.Add(subjectPosition, cameraDelta);
                
            }

            
            if(direction == InputHandler.keyStates.CW)
            {
                
                cameraDelta = cameraSpeed * OrbitCW() * deltaTime * fps;
                cameraPosition = Vector3.Add(subjectPosition, cameraDelta);

                // rotating the model to the direction you are facing
                // calculate pitch axis for rotating, therefore the orthogonal between the forward and up 
                // assuming righthandedness
                Vector3 pitchAxis = Vector3.Cross(subjectRotation, Vector3.Up);
                //pitchAxis.Normalize();

                float radian = SubjectRadians(1);

                subjectRotation = Vector3.Transform(pitchAxis, Matrix.CreateRotationY(radian));

            }

            if (direction == InputHandler.keyStates.CCW)
            {
                cameraDelta = cameraSpeed * OrbitCCW() * deltaTime * fps;
                cameraPosition = Vector3.Add(subjectPosition, cameraDelta);
                
                // rotating the model to the direction you are facing
                // calculate pitch axis for rotating, therefore the orthogonal between the forward and up 
                // assuming righthandedness
                Vector3 pitchAxis = Vector3.Cross(subjectRotation, Vector3.Up);
                //pitchAxis.Normalize();

                float radian = SubjectRadians(-1);

                subjectRotation = Vector3.Transform(pitchAxis, Matrix.CreateRotationY(radian));
            }
            

            // calculates the new camera bounding box
            this.maxPoint = this.subjectPosition + this.AABBOffset;
            this.minPoint = this.subjectPosition - this.AABBOffset;
        }

        /** 
        *   @brief This is actually the pigeon rotation function. Calculates the new rotated position using quaternion rotation
        *	@param deltaVector the front vector
        *	@param targetAxis the axis to rotate on
        *	@param inputDegrees the amount of rotation in degrees
        *	@param inputVector the mouse input. used to check if there is movement
        *	@return deltaVector, subjectRotation   
        *	@pre deltaVector must not be zero
        *	@post  
        */
        public Vector3 PigeonUpdate(Vector3 deltaVector, Vector3 targetAxis, float inputDegrees, Vector3 inputVector)
        {

            if (inputVector.Length() > 0)
            {
                float radianInput = SubjectRadians(inputDegrees);

                deltaQuaternion = new Quaternion(deltaVector.X, deltaVector.Y, deltaVector.Z, 0);

                Quaternion resultQuaternion = RotateCamera(radianInput, targetAxis, deltaQuaternion);

                subjectRotation = new Vector3(resultQuaternion.X, resultQuaternion.Y, resultQuaternion.Z);

                radianInput = 0;

                return subjectRotation;

            }
            else
            {
                return deltaVector;
            }

        }

        /** 
        *   @brief This function implements the quaternion rotation. qpq'
        *   @see http://in2gpu.com/2016/03/14/opengl-fps-camera-quaternion/
        *	@param inputAngle the angle to rotate on 
        *	@param inputAxis the axis to rotate on 
        *	@param pQuat the starting position
        *	@param 
        *	@return resultQuat the final position of the rotation   
        *	@pre 
        *	@post 
        */
        private Quaternion RotateCamera(float inputAngle, Vector3 inputAxis, Quaternion pQuat)
        {

            Quaternion qQuat = AxisAngle(inputAngle, inputAxis);

            Quaternion pqQuat = Quaternion.Multiply(qQuat, pQuat);

            Quaternion resultQuat = Quaternion.Multiply(pqQuat, Quaternion.Inverse(qQuat));

            deltaQuaternion = resultQuat;

            return resultQuat;
        }

        /** 
        *   @brief This function creates a quaternion from an angle and axis
        *   @see
        *	@param theTheta the angle in radians
        *	@param inputAxis the axis to rotate on 
        *	@param 
        *	@param 
        *	@return rotationQuat the quaternion calculated from axis and angle
        *	@pre 
        *	@post 
        */
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

        /** 
        *   @brief This function creates a ground plane
        *   @see
        *	@param 
        *	@param  
        *	@param 
        *	@param 
        *	@return tempVector the ground position
        *	@pre 
        *	@post 
        */
        private Vector3 FloorCheck()
        {
            if (subjectPosition.Y <= 1)
            {
                Vector3 tempVector = new Vector3(subjectPosition.X, 1, subjectPosition.Z);

                return tempVector;
            }
            else
            {
                return subjectPosition;
            }
        }

        /** 
        *   @brief calls the orbit function and locks it to 1 degree movement
        *   @see
        *	@param 
        *	@param  
        *	@param 
        *	@param 
        *	@return orbit function
        *	@pre 
        *	@post 
        */
        private Vector3 OrbitCW()
        {
            
            return Orbit(1);
        }

        /** 
        *   @brief calls the orbit function and locks it to 1 degree movement
        *   @see
        *	@param 
        *	@param  
        *	@param 
        *	@param 
        *	@return orbit function
        *	@pre 
        *	@post 
        */
        private Vector3 OrbitCCW()
        {

            return Orbit(-1);
        }

        /** 
        *   @brief Calculates the orbit of the camera around the subject model
        *   @see
        *	@param 
        *	@param  
        *	@param 
        *	@param 
        *	@return resultVector the new position based on the pigeon position
        *	@pre 
        *	@post 
        */
        private Vector3 Orbit(float inputDegrees)
        {
            float radian = SubjectRadians(inputDegrees);

            Vector3 rotateVector = Vector3.Transform(cameraDelta, Matrix.CreateRotationY(radian));
            rotateVector.Normalize();
            Vector3 resultVector = rotateVector * 10;
            
            return resultVector;
        }
    }
}
