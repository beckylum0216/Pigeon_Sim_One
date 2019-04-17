using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentOne_Pigeon_Sim
{
    public class Camera: Subject
    {

        private Matrix theCamera;
        private Vector3 cameraEye;
        private Vector3 zoomVector;
        private Quaternion deltaQuaternion;

        public Camera()
        { }

        public Camera(Matrix inputCamera, Vector3 initPosition, Vector3 eyePosition, Vector3 deltaVector, Vector3 inputOffset)
        {
            this.theCamera = inputCamera;
            this.futurePosition = initPosition;
            this.subjectPosition = initPosition;
            this.cameraEye = eyePosition;
            this.subjectRotation = deltaVector;
            this.deltaQuaternion = Quaternion.Identity;
            this.AABBOffset = new Vector3(1f, 1f, 1f);
            this.maxPoint = this.subjectPosition + this.AABBOffset;
            this.minPoint = this.subjectPosition - this.AABBOffset;
            zoomVector = new Vector3(0, 0, 0);
        }

        public Camera(ContentManager Content, String modelFile, String textureFile, Vector3 predictedPosition, Vector3 inputPosition, 
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

        }

        /** 
         *  @brief update the position state of the camera 
         *	@param inputVector the mouse inputs in vector form
         *	@param deltaTime the time the game has elapsed
         *	@param fps the frame rate
         *	@return tempCameraObj the position of the camera in matrix form
         *	@pre 
         *	@post position update of the camera object
         */
        public override Matrix SubjectUpdate(Vector3 inputVector, float deltaTime, float fps)
        {
            /// calculate pitch axis for rotating, therefore the orthogonal between the forward and up 
            /// assuming righthandedness
            Vector3 pitchAxis = Vector3.Cross(subjectRotation, Vector3.Up);
            pitchAxis.Normalize();

            subjectRotation = CameraUpdate(subjectRotation, pitchAxis, inputVector.Y, inputVector);
            subjectRotation = CameraUpdate(subjectRotation, Vector3.Up, -inputVector.X, -inputVector);

            cameraEye = subjectPosition + subjectRotation; // this is the correct 

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

            Matrix tempCameraObj = Matrix.CreateLookAt(subjectPosition, cameraEye, Vector3.Up);

            return tempCameraObj;
        }


        /** 
         *  @brief mutator for the camera position
         *	@param inputVector the new position 
         *	@return 
         *	@pre 
         *	@post position update of the camera object
         */
        public void SetCameraPosition(Vector3 inputVector)
        {
            this.subjectPosition = inputVector;
        }


        /** 
        *   @brief accessor for the camera position
        *	@param 
        *	@return subjectPosition 
        *	@pre position must exist
        *	@post 
        */
        public Vector3 GetCameraPosition()
        {
            return this.subjectPosition;
        }

        /** 
       *    @brief mutator for the camera viewport 
       *	@param inputVector the new camera viewport position
       *	@return 
       *	@pre 
       *	@post position must exist 
       */
        public void SetCameraEye(Vector3 inputVector)
        {
            this.cameraEye = inputVector;
        }


        /** 
        *   @brief This is actually the camera rotation function. Calculates the new rotated position using quaternion rotation
        *	@param deltaVector the front vector
        *	@param targetAxis the axis to rotate on
        *	@param inputDegrees the amount of rotation in degrees
        *	@param inputVector the mouse input. used to check if there is movement
        *	@return deltaVector, subjectRotation   
        *	@pre deltaVector must not be zero
        *	@post  
        */
        public Vector3 CameraUpdate(Vector3 deltaVector, Vector3 targetAxis, float inputDegrees, Vector3 inputVector)
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
        *   @brief This function updates the location of the camera. 
        *	@param direction the direction the camera is travelling
        *	@param cameraSpeed the velocity of the camera
        *	@param deltaTime the slice of time the game has elapsed
        *	@param fps the framerate
        *	@return subjectPosition the new camera position    
        *	@pre 
        *	@post subjectPosition will be updated
        */
        public void SubjectMove(InputHandler.keyStates direction, float cameraSpeed, float deltaTime, float fps)
        {
           
            subjectRotation.Normalize();

            if (direction == InputHandler.keyStates.Forwards)
            {
                //futurePosition += cameraSpeed * actorRotation * deltaTime * fps;
                subjectPosition += cameraSpeed * subjectRotation * deltaTime * fps;
                
                Debug.WriteLine("position Vector: " + subjectPosition.X + " " + subjectPosition.Y + " " + subjectPosition.Z);
            }

            if (direction == InputHandler.keyStates.Backwards)
            {

                //futurePosition -= cameraSpeed * actorRotation * deltaTime * fps;
                subjectPosition -= cameraSpeed * subjectRotation * deltaTime * fps;

                Debug.WriteLine("position Vector: " + subjectPosition.X + " " + subjectPosition.Y + " " + subjectPosition.Z);
            }

            if (direction == InputHandler.keyStates.Left)
            {
                Vector3 tempDeltaVector = Vector3.Cross(Vector3.Up, subjectRotation);
                tempDeltaVector.Normalize();
                //futurePosition += cameraSpeed * tempDeltaVector * deltaTime * fps;
                subjectPosition += cameraSpeed * tempDeltaVector * deltaTime * fps;
                
                Debug.WriteLine("position Vector: " + subjectPosition.X + " " + subjectPosition.Y + " " + subjectPosition.Z);
            }

            if (direction == InputHandler.keyStates.Right)
            {
                Vector3 tempDeltaVector = Vector3.Cross(Vector3.Up, subjectRotation);
                tempDeltaVector.Normalize();
                //futurePosition -= cameraSpeed * tempDeltaVector * deltaTime * fps;
                subjectPosition -= cameraSpeed * tempDeltaVector * deltaTime * fps;
               
                Debug.WriteLine("position Vector: " + subjectPosition.X + " " + subjectPosition.Y + " " + subjectPosition.Z);
            }

            if(direction == InputHandler.keyStates.ZoomIn)
            {
                float zoomFactor = 1.005f;
                subjectPosition *= zoomFactor * subjectRotation * deltaTime * fps;
                 
            }

            if (direction == InputHandler.keyStates.ZoomOut)
            {
                float zoomFactor = 0.005f;
                subjectPosition *= zoomFactor * subjectRotation * deltaTime *fps;
                

            }

            // calculates the new camera bounding box
            this.maxPoint = this.subjectPosition + this.AABBOffset;
            this.minPoint = this.subjectPosition - this.AABBOffset;
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
            
            Quaternion resultQuat =  Quaternion.Multiply(pqQuat, Quaternion.Inverse(qQuat));
            
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
            if(subjectPosition.Y <= 1)
            {
                Vector3 tempVector = new Vector3(subjectPosition.X, 1, subjectPosition.Z);

                return tempVector;
            }
            else
            {
                return subjectPosition;
            }
        }
        
    }
}
