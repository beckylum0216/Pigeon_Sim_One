using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AssignmentOne_Pigeon_Sim.Tests
{
    [TestFixture]
    public class Test003_Pigeon_MapGenerator
    {
        [Test]
        public void Test000_SetMap()
        {
            try
            {
                MapGenerator testGenerator = new MapGenerator(11,11);
                testGenerator.SetMap();
                Assert.Pass();
            }
            catch(Exception e)
            {
                Debug.WriteLine("Exception: " + e);
                Assert.Fail();
            }
        }

        [Test]
        public void Test001_SetCoords()
        {
            try
            {
                MapGenerator testGenerator = new MapGenerator(11, 11);
                testGenerator.SetMap();
                testGenerator.SetCoords();
                Assert.Pass();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e);
                Assert.Fail();
            }
        }
    }
}
