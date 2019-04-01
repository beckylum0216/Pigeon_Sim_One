using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentOne_Pigeon_Sim
{
    public class Camera: Actor
    {

        Matrix theCamera;
        private Vector3 cameraEye;
        //private Vector3 deltaVector = new Vector3(1, 0, 0);
        private Quaternion deltaQuaternion;
        
        public Camera(Matrix inputCamera, Vector3 initPosition, Vector3 eyePosition, Vector3 deltaVector)
        {
            this.theCamera = inputCamera;
            this.actorPosition = initPosition;
            this.cameraEye = eyePosition;
            this.actorRotation = deltaVector;
            this.deltaQuaternion = Quaternion.Identity;
            this.AABBOffset = new Vector3(1f, 1f, 1f);
            this.maxPoint = this.actorPosition + this.AABBOffset;
            this.minPoint = this.actorPosition - this.AABBOffset;
        }

        
        public override Matrix ActorUpdate(Vector3 inputVector)
        {
            // calculate pitch axis for rotating, therefore the orthogonal between the forward and up 
            // assuming righthandedness
            Vector3 pitchAxis = Vector3.Cross(actorRotation, Vector3.Up);
            pitchAxis.Normalize();

            actorRotation = CameraUpdate(actorRotation, pitchAxis, inputVector.Y, inputVector);
            actorRotation = CameraUpdate(actorRotation, Vector3.Up, -inputVector.X, -inputVector);

            cameraEye = actorPosition + actorRotation; // this is the correct 
            
            Matrix tempCameraObj = Matrix.CreateLookAt(actorPosition, cameraEye, Vector3.Up);

            return tempCameraObj;
        }

        public void SetCameraPosition(Vector3 inputVector)
        {
            this.actorPosition = inputVector;
        }

        public Vector3 GetCameraPosition()
        {
            return this.actorPosition;
        }

        public void SetCameraEye(Vector3 inputVector)
        {
            this.cameraEye = inputVector;
        }

        public Vector3 GetDeltaVector()
        {
            Vector3 deltaVector = new Vector3(deltaQuaternion.X, deltaQuaternion.Y, deltaQuaternion.Z);

            return deltaVector;
        }

        public Vector3 CameraUpdate(Vector3 deltaVector, Vector3 targetAxis, float inputDegrees, Vector3 inputVector)
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
        

        public void CameraMove(InputHandler.Direction direction, float cameraSpeed, float deltaTime, float fps)
        {
            Debug.WriteLine("Input Down: " + direction);
            actorRotation.Normalize();

            if (direction == InputHandler.Direction.Forwards)
            {
         
                actorPosition += cameraSpeed * actorRotation * deltaTime * fps;

                Debug.WriteLine("position Vector: " + actorPosition.X + " " + actorPosition.Y + " " + actorPosition.Z);
            }

            if (direction == InputHandler.Direction.Backwards)
            {
               
                actorPosition -= cameraSpeed * actorRotation * deltaTime * fps;

                Debug.WriteLine("position Vector: " + actorPosition.X + " " + actorPosition.Y + " " + actorPosition.Z);
            }

            if (direction == InputHandler.Direction.Left)
            {
                Vector3 tempDeltaVector = Vector3.Cross(Vector3.Up, actorRotation);
                tempDeltaVector.Normalize();
                actorPosition += cameraSpeed * tempDeltaVector * deltaTime * fps;
                //actorPosition *= -5 * deltaVector;
                Debug.WriteLine("position Vector: " + actorPosition.X + " " + actorPosition.Y + " " + actorPosition.Z);
            }

            if (direction == InputHandler.Direction.Right)
            {
                Vector3 tempDeltaVector = Vector3.Cross(Vector3.Up, actorRotation);
                tempDeltaVector.Normalize();
                actorPosition -= cameraSpeed * tempDeltaVector * deltaTime * fps;
                //actorPosition *= 5 * deltaVector;
                
                Debug.WriteLine("position Vector: " + actorPosition.X + " " + actorPosition.Y + " " + actorPosition.Z);
            }

            // calculates the new camera bounding box
            this.maxPoint = this.actorPosition + this.AABBOffset;
            this.minPoint = this.actorPosition - this.AABBOffset;
        }

        // http://in2gpu.com/2016/03/14/opengl-fps-camera-quaternion/
        // qpq'
        private Quaternion RotateCamera(float inputAngle, Vector3 inputAxis, Quaternion pQuat)
        {
            
            Quaternion qQuat = AxisAngle(inputAngle, inputAxis);
            
            Quaternion pqQuat = Quaternion.Multiply(qQuat, pQuat);
            
            Quaternion resultQuat =  Quaternion.Multiply(pqQuat, Quaternion.Inverse(qQuat));
            
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

        



    }
}
