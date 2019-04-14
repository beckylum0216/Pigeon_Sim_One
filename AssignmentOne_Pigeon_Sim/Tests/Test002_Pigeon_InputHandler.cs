using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace AssignmentOne_Pigeon_Sim.Tests
{
    [TestFixture]
    public class Test002_Pigeon_InputHandler
    {
        [Test]
        public void Test_000_MouseHandler()
        {
            try
            {
                int screenX = 300;
                int screenY = 300;
                InputHandler testHandler = new InputHandler(screenX, screenY);
                Vector3 resultVector = testHandler.MouseHandler(screenX, screenY, 1f);

                Assert.Pass();
            }
            catch(Exception e)
            {
                Assert.Fail();
            }
            
        }

        [Test]
        public void Test_001_KeyboardHandler()
        {
            try
            {
                int screenX = 300;
                int screenY = 300;
                InputHandler testHandler = new InputHandler(screenX, screenY);
                testHandler.KeyboardHandler();

                Assert.Pass();
            }
            catch(Exception e)
            {
                Debug.WriteLine("Exception: " + e);
                Assert.Fail();
            }
            
        }

    }
}
