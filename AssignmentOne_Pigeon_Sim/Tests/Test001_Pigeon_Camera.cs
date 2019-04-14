using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace AssignmentOne_Pigeon_Sim.Tests
{
    [TestFixture]
    public class Test001_Pigeon_Camera
    {
        [Test]
        public void Test_000_Camera()
        {
            try
            {
                Vector3 camEyeVector = new Vector3(0, 0, 0);
                Vector3 camPositionVector = Vector3.Add(new Vector3(0, 0, 0), new Vector3(0, 1.6f, 0));
                Vector3 deltaVector = new Vector3(0, 0, 0.001f);
                Vector3 AABBOffsetCamera = new Vector3(0.5f, 0.25f, 0.5f);
                Matrix theCamera = Matrix.CreateLookAt(camPositionVector, camEyeVector, Vector3.Up); 
                Camera testCam = new Camera(theCamera, camPositionVector, camEyeVector, deltaVector, AABBOffsetCamera);
                Assert.Pass();
            }
            catch(Exception e)
            {
                Assert.Fail();
            }
        }
    }
}
