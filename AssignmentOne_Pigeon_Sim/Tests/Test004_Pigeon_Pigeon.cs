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
    public class Test004_Pigeon_Pigeon
    {
        [Test]
        public void Test_000_FloorCheck()
        {
            try
            {
                Game1 game = new Game1();   
                Vector3 camEyeVector = new Vector3(0, 0, 0);
                Vector3 camPositionVector = Vector3.Add(new Vector3(0, 0, 0), new Vector3(0, 1.6f, 0));
                Vector3 deltaVector = new Vector3(0, 0, 0.001f);
                Vector3 AABBOffsetCamera = new Vector3(0.5f, 0.25f, 0.5f);
                Matrix theCamera = Matrix.CreateLookAt(camPositionVector, camEyeVector, Vector3.Up);
                Camera camera = new Camera(theCamera, camPositionVector, camEyeVector, deltaVector, AABBOffsetCamera);
               
                string modelPigeon = "Models/SK_Pigeon";
                string texturePigeon = "Maya/sourceimages/pigeon_normal2";
                Vector3 predictedPigeon = camPositionVector;
                Vector3 positionPigeon = camPositionVector;
                Vector3 rotationPigeon = new Vector3(-90, 0, 0) + deltaVector;
                Vector3 AABBOffsetPigeon = new Vector3(0.5f, 0.25f, 0.5f);
                float scalePigeon = 0.05f;
                Pigeon pigeon = new Pigeon(game.Content, modelPigeon, texturePigeon, predictedPigeon, positionPigeon, rotationPigeon,
                                        scalePigeon, AABBOffsetPigeon, camera);
                Assert.Pass();
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
    }
}
