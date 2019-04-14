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
    public class Test005_PlotClient:Game
    {
        [Test]
        public void Test000_PlotDictionary()
        {
            try
            {
                Game game = new Game();
                PlotClient testClient = new PlotClient(game.Content,11,11,1);
                testClient.SetPlotDictionary();
                Assert.Pass();

            }
            catch(Exception e)
            {
                Debug.WriteLine("Exception: " + e);
                Assert.Fail();
            }
        }

        [Test]
        public void Test001_PlotList()
        {
            try
            {
                Game game = new Game();
                PlotClient testClient = new PlotClient(game.Content, 11, 11, 1);
                testClient.SetPlotList();
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
