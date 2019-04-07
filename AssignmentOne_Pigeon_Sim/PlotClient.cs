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
        private List <Actor> plotList = new List<Actor>();
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

        public List<Actor> GetPlotList()
        {
            return plotList;
        }

        // create prototypes
        public void SetPlotDictionary()
        {
            string modelFile = "Models/skybox_cube";
            string textureFile = "Maya/sourceimages/skybox_diffuse";
            float centerOrigin = (23 * 22) / 2;
            Vector3 positionSkyBox = new Vector3(centerOrigin, 0f, centerOrigin);
            Vector3 rotationSkyBox = new Vector3(0, 0, 0);
            Vector3 AABBOffset = new Vector3(0, 0, 0);
            float scaleSkyBox = 15f;
            SkyBox plotSkyBox = new SkyBox(Content, modelFile, textureFile, positionSkyBox, rotationSkyBox, scaleSkyBox, AABBOffset);
            landPlots.Add(Block.buildType.SkyBox.ToString(), plotSkyBox);

            modelFile = "Models/city_residential_03";
            textureFile = "Maya/sourceimages/city_residential_03_dif";
            Vector3 positionBuilding = new Vector3(0, 0, 0);
            Vector3 rotationBuilding = new Vector3(0, 0, 0);
            Vector3 AABBOffsetBuilding = new Vector3(15, 20, 15);
            float scaleBuilding = 2.5f;
            Plot plotBuilding = new Plot(Content, modelFile, textureFile, positionBuilding, rotationBuilding, scaleBuilding, AABBOffsetBuilding);
            landPlots.Add(Block.buildType.Building.ToString(), plotBuilding);

            //set up roads and tiles
            for (int ii = 0; ii < sizeX; ii++)
            {
                for(int jj = 0; jj < sizeY; jj++)
                {
                    try
                    {
                        Vector3 tempPosition = new Vector3(gridMap[ii, jj].GetCoordX(), gridMap[ii, jj].GetCoordY(), gridMap[ii, jj].GetCoordZ());
                        Vector3 tempOffset = new Vector3(0, 0, 0);
                        Plot tempPlot = new Plot(Content, gridMap[ii, jj].GetModelPath(), gridMap[ii, jj].GetTexturePath(),
                                                    tempPosition, gridMap[ii, jj].GetBlockRotation(), plotScale, tempOffset);
                        landPlots.Add(gridMap[ii, jj].GetBlockType().ToString(), tempPlot);
                    }
                    catch( System.ArgumentException e)
                    {
                        Debug.WriteLine("Item Already Added! " + e);
                    }
                }
            }
            
        }

        // bad bad code 
        public void SetPlotList()
        {
            string modelFile = "Models/skybox_cube";
            string textureFile = "Maya/sourceimages/skybox_diffuse";
            float centerOrigin = (23 * 22) / 2;
            Vector3 positionSkyBox = new Vector3(centerOrigin, 0f, centerOrigin);
            Vector3 rotationSkyBox = new Vector3(0, 0, 0);
            Vector3 AABBOffset = new Vector3(0, 0, 0);
            float scaleSkyBox = 15f;
            Camera pseudoCamera = new Camera();
            Actor plotSkyBox = landPlots["SkyBox"].ActorClone(Content, modelFile, textureFile, positionSkyBox, positionSkyBox, rotationSkyBox, scaleSkyBox, AABBOffset, pseudoCamera);
            plotList.Add(plotSkyBox);
            
            // adds to the list the land and road tiles
            for (int ii = 0; ii < sizeX; ii++)
            {
                for (int jj = 0; jj < sizeY; jj++)
                {
                    Vector3 tempPosition = new Vector3(gridMap[ii, jj].GetCoordX(), gridMap[ii, jj].GetCoordY(), gridMap[ii, jj].GetCoordZ());
                    
                    Vector3 tempOffset = new Vector3(10, 1, 10);
                    Plot tempPlot = new Plot(Content, gridMap[ii, jj].GetModelPath(), gridMap[ii, jj].GetTexturePath(), tempPosition, 
                                            gridMap[ii, jj].GetBlockRotation(), gridMap[ii, jj].GetBlockScale(), tempOffset);
                    
                    plotList.Add(tempPlot);

                    
                }
            }

            for(int ii = 0; ii < sizeX; ii++)
            {
                for(int jj = 0; jj < sizeY; jj++)
                {
                    if(gridMap[ii,jj].GetBlockType() == Block.buildType.Building)
                    {
                        modelFile = "Models/city_residential_03";
                        textureFile = "Maya/sourceimages/city_residential_03_dif";
                        Vector3 positionBuilding = new Vector3(gridMap[ii,jj].GetCoordX(), 0, gridMap[ii, jj].GetCoordZ());
                        Vector3 rotationBuilding = new Vector3(0, 0, 0);
                        Vector3 AABBOffsetBuilding = new Vector3(17, 25, 17);
                        float scaleBuilding = 3f;
                        Plot plotBuilding = new Plot(Content, modelFile, textureFile, positionBuilding, rotationBuilding, scaleBuilding, AABBOffsetBuilding);
                        plotList.Add(plotBuilding);
                    }
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
