using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;

namespace AssignmentOne_Pigeon_Sim
{
    class PlotClient
    {
        private List <Plot> plotList = new List<Plot>();
        private Dictionary<string, Actor> landPlots = new Dictionary<string, Actor>();
        private Block[,] gridMap;
        private ContentManager Content;
        private int sizeX;
        private int sizeY;
        private float plotScale;

        public PlotClient(ContentManager inputContent,  int inputX, int inputY, float inputScale)
        {
            this.Content = inputContent;
            this.sizeX = inputX;
            this.sizeY = inputY;
            this.plotScale = inputScale;

            // initialise map
            MapGenerator mapCreate = new MapGenerator(sizeX, sizeY);
            mapCreate.SetMap();
            mapCreate.PrintGrid();
            mapCreate.SetCoords();
            mapCreate.PrintCoords();
           
            gridMap = mapCreate.GetGridMap();
        }

        public void SetPlot(string plotName, Actor plotObj)
        {
            landPlots.Add(plotName, plotObj);
        }

        public Actor GetPlot(string plotName)
        {
            return landPlots[plotName];
        }

        public Dictionary<string, Actor> GetLandPlots()
        {
            return landPlots;
        }

        public List<Plot> GetPlotList()
        {
            return plotList;
        }

        // create prototypes
        public void SetPlotDictionary()
        {
            for(int ii = 0; ii < sizeX; ii++)
            {
                for(int jj = 0; jj < sizeY; jj++)
                {
                    Vector3 tempPosition = new Vector3(gridMap[ii, jj].GetCoordX(), gridMap[ii,jj].GetCoordY(), gridMap[ii, jj].GetCoordZ());
                    Vector3 tempRotation = new Vector3(0, 0, 0);
                    Plot tempPlot = new Plot(Content, gridMap[ii, jj].GetModelPath(), gridMap[ii, jj].GetTexturePath(),
                                                tempPosition, tempRotation, plotScale);
                    landPlots.Add(gridMap[ii, jj].GetBlockType().ToString(), tempPlot);

                }
            }
        }

        public void SetPlotList()
        {
            for (int ii = 0; ii < sizeX; ii++)
            {
                for (int jj = 0; jj < sizeY; jj++)
                {
                    Vector3 tempPosition = new Vector3(gridMap[ii, jj].GetCoordX(), gridMap[ii, jj].GetCoordY(), gridMap[ii, jj].GetCoordZ());
                    
                    Vector3 tempRotation = new Vector3(0, 0, 0);
                    Plot tempPlot = new Plot(Content, gridMap[ii, jj].GetModelPath(), gridMap[ii, jj].GetTexturePath(),
                                                tempPosition, tempRotation, gridMap[ii, jj].GetBlockScale());
                    //Debug.WriteLine("temp x: " + tempPlot.actorPosition.X + " y: " + tempPlot.actorPosition.Y + " Z: " + tempPlot.actorPosition.Z);

                    plotList.Add(tempPlot);

                    //Debug.WriteLine("plot x: " + plotList[ii].actorPosition.X + " y: " + plotList[ii].actorPosition.Y + " Z: "+ plotList[ii].actorPosition.Z);
                }
            }
        }

        public void PrintPlotList()
        {
            for(int ii = 0; ii < plotList.Count; ii++)
            {
                Debug.WriteLine("plot x: " + plotList[ii].actorPosition.X + " y: " + plotList[ii].actorPosition.Y + " Z: " + plotList[ii].actorPosition.Z);
            }
        }
    }
}
