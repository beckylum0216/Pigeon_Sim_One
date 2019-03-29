using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentOne_Pigeon_Sim
{
    class Camera
    {
        Matrix theCamera;
        private Vector3 cameraPosition;
        private Vector3 cameraEye;
        private Vector3 cameraDelta;

        private Vector3 axisVector = new Vector3(1, 0, 0);
        
        
        private Quaternion deltaQuaternion;
        
        public Camera(Matrix inputCamera, Vector3 initPosition, Vector3 eyePosition, Vector3 deltaVector)
        {
            this.theCamera = inputCamera;
            this.cameraPosition = initPosition;
            this.cameraEye = eyePosition;
            this.cameraDelta = deltaVector;
            this.deltaQuaternion = Quaternion.Identity;
        }
        
        public void SetCameraPosition(Vector3 inputVector)
        {
            this.cameraPosition = inputVector;
        }

        public Vector3 GetCameraPosition()
        {
            return this.cameraPosition;
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
                float radianInput = CameraRadian(inputDegrees);
               
                deltaQuaternion = new Quaternion(deltaVector.X, deltaVector.Y, deltaVector.Z, 0);

                Quaternion resultQuaternion = RotateCamera(radianInput, targetAxis, deltaQuaternion);
                
                cameraDelta = new Vector3(resultQuaternion.X, resultQuaternion.Y, resultQuaternion.Z);
                // Debug.WriteLine("front Axis: " + cameraDelta.X + " " + cameraDelta.Y + " " + cameraDelta.Z);

                radianInput = 0;

                return cameraDelta;
               
            }
            else
            {

                return deltaVector;
               
            }
            
        }

        


        public void cameraMove()
        {

        }


        // qpq'
        private Quaternion RotateCamera(float inputAngle, Vector3 inputAxis, Quaternion pQuat)
        {
            
            Quaternion qQuat = AxisAngle(inputAngle, inputAxis);
            // Debug.WriteLine("qQuat: " + qQuat.X + " " + qQuat.Y + " " + qQuat.Z + " " + qQuat.W);

            // Debug.WriteLine("pQuat: " + pQuat.X + " " + pQuat.Y + " " + pQuat.Z + " " + pQuat.W);
            
            Quaternion pqQuat = Quaternion.Multiply(qQuat, pQuat);
            // Debug.WriteLine("pqQuat: " + pqQuat.X + " " + pqQuat.Y + " " + pqQuat.Z + " " + pqQuat.W);

            Quaternion resultQuat =  Quaternion.Multiply(pqQuat, Quaternion.Inverse(qQuat));

            // Debug.WriteLine("resultQuat: " + resultQuat.X + " " + resultQuat.Y + " " + resultQuat.Z + " " + resultQuat.W);

            deltaQuaternion = resultQuat;

            return resultQuat;
        }

        private Quaternion AxisAngle(float theTheta, Vector3 inputAxis)
        {
            Quaternion rotationQuart;
            double sinTheta;

            // Normalise rotation axis to have unit length
            inputAxis.Normalize();
            //Debug.WriteLine("input Axis: " + inputAxis.X + " "+ inputAxis.Y + " " + inputAxis.Z);

            sinTheta = Math.Sin(theTheta / 2);

            //Debug.WriteLine("sinTheta: " + sinTheta);

            // Convert to rotation quaternion
            rotationQuart.X = (float)(inputAxis.X * sinTheta);  // theTheta should be in radians
            rotationQuart.Y = (float)(inputAxis.Y * sinTheta);
            rotationQuart.Z = (float)(inputAxis.Z * sinTheta);
            rotationQuart.W = (float)(Math.Cos(theTheta / 2));

            //Debug.WriteLine("QangleAxis: " + rotationQuart.X + " " + rotationQuart.Y + " " + rotationQuart.Z + " "+ rotationQuart.W);

            return rotationQuart;
        }
        
        public float CameraRadian(float inputDegree)
        {
            return inputDegree * (float)(Math.PI / 180);
        }

        
    }
}
