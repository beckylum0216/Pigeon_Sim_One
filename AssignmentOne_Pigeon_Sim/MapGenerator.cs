using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace AssignmentOne_Pigeon_Sim
{
    class MapGenerator
    {
        int sizeX = 0;
        int sizeZ = 0;
        Block[,] gridMap;
        

        public MapGenerator(int inputX, int inputZ)
        {
            sizeX = inputX;
            sizeZ = inputZ;
            gridMap = new Block[sizeX, sizeZ];
        }

        // generates the map based on a typical chinese chequerboard pattern
        public void SetMap()
        {

            // sets the initial "block consisting of building and adjacent road
            for(int ii = 0; ii < sizeX; ii++)
            {
                for(int jj = 0; jj < sizeZ; jj++)
                {
                    if(ii % 2 == 1 && jj % 2 == 1)
                    {
                        string modelPath = "Models/city_residential_03";
                        string texturePath = "Maya/sourceimages/city_residential_03_dif";
                        Vector3 buildingRotation = new Vector3(0,0,0);
                        Block tempBlock = new Block(ii, 0, jj, Block.buildType.Building, modelPath, texturePath, 2, buildingRotation);
                        gridMap[ii, jj] = tempBlock;

                        if((ii - 1) >= 0)
                        {
                            string modelFile = "Models/city_road_02";
                            string textureFile = "Maya/sourceimages/city_road_02_dif";
                            Vector3 roadRotation = new Vector3(0, 0, 0);
                            Block roadWest = new Block(ii - 1, -2, jj, Block.buildType.RoadVertical, modelFile, textureFile, 1f, roadRotation);
                            gridMap[ii - 1, jj] = roadWest;
                        }
                        
                        if((jj - 1) >= 0)
                        {
                            string modelFile = "Models/city_road_02";
                            string textureFile = "Maya/sourceimages/city_road_02_dif";
                            Vector3 roadRotation = new Vector3(1, 0, 0);
                            Block roadNorth = new Block(ii, -2, jj - 1, Block.buildType.RoadHorizontal, modelFile, textureFile, 1f, roadRotation);
                            gridMap[ii, jj - 1] = roadNorth;
                        }

                        if((ii + 1) < sizeX)
                        {
                            string modelFile = "Models/city_road_02";
                            string textureFile = "Maya/sourceimages/city_road_02_dif";
                            Vector3 roadRotation = new Vector3(0, 0, 0);
                            Block roadEast = new Block(ii + 1, -2, jj, Block.buildType.RoadVertical, modelFile, textureFile, 1f, roadRotation);
                            gridMap[ii + 1, jj] = roadEast;
                        }

                        if((jj + 1) < sizeZ)
                        {
                            string modelFile = "Models/city_road_02";
                            string textureFile = "Maya/sourceimages/city_road_02_dif";
                            Vector3 roadRotation = new Vector3(1, 0, 0);
                            Block roadSouth = new Block(ii, -2, jj + 1, Block.buildType.RoadHorizontal, modelFile, textureFile, 1f, roadRotation);
                            gridMap[ii, jj + 1] = roadSouth;
                        }
                        
                    }
                }
            }

            // searches for empty grid spaces and populates it with a suitable asset
            for (int ii = 0; ii < sizeX; ii++)
            {
                for (int jj = 0; jj < sizeZ; jj++)
                {
                    if (gridMap[ii, jj] == null)
                    {
                        int junctions = FindNeighbours(ii, jj, gridMap);

                        Debug.WriteLine("Junctions: " + junctions);

                        if (junctions == 2)
                        {
                            string modelFile = "Models/city_road_02";
                            string textureFile = "Maya/sourceimages/city_road_02_dif";
                            Vector3 cornerRotation = new Vector3(0, 0, 0);
                            gridMap[ii, jj] = new Block(ii, -2, jj, Block.buildType.RoadCorner, modelFile, textureFile, 1f, cornerRotation);
                        }
                        else if(junctions == 3)
                        {
                            string modelFile = "Models/city_road_02";
                            string textureFile = "Maya/sourceimages/city_road_02_dif";
                            Vector3 TRotation = new Vector3(0, 0, 0);
                            gridMap[ii, jj] = new Block(ii, -2, jj, Block.buildType.RoadT, modelFile, textureFile, 1f, TRotation);
                        }
                        else
                        {
                            string modelFile = "Models/city_road_03";
                            string textureFile = "Maya/sourceimages/city_road_03_dif";
                            Vector3 crossRotation = new Vector3(0, 0, 0);
                            gridMap[ii, jj] = new Block(ii, -2, jj, Block.buildType.RoadCross, modelFile, textureFile, 1f, crossRotation);
                        }
                    }
                }
            }
        }

        // this functions helps the program to find the right junction for each empty space
        // does not take into account orientation
        private int FindNeighbours(int gridX, int gridY, Block[,] inputGrid)
        {
            int junction = 0;

            if((gridX - 1) >= 0)
            {
                if (inputGrid[gridX - 1, gridY].GetBlockType() == Block.buildType.RoadHorizontal)
                {
                    junction += 1;
                }
            }
            
            if((gridY - 1) >= 0)
            {
                if (inputGrid[gridX, gridY - 1].GetBlockType() == Block.buildType.RoadVertical)
                {
                    junction += 1;
                }
            }

            if((gridX + 1) < sizeX)
            {
                if(inputGrid[gridX + 1, gridY].GetBlockType() == Block.buildType.RoadHorizontal)
                {
                    junction += 1;
                }
            }
            
            if((gridY + 1) < sizeZ)
            {
                if (inputGrid[gridX, gridY + 1].GetBlockType() == Block.buildType.RoadVertical)
                {
                    junction += 1;
                }
            }
            
            return junction;
           
        }

        public void SetCoords()
        {
            for(int ii = 0; ii < sizeX; ii++)
            {
                for(int jj = 0; jj < sizeZ; jj++)
                {
                    int tempX = gridMap[ii, jj].GetPositionX() * 22;
                    int tempZ = gridMap[ii, jj].GetPositionZ() * 22;

                    gridMap[ii, jj].SetCoordX(tempX);
                    gridMap[ii, jj].SetCoordZ(tempZ);

                    if (gridMap[ii,jj].GetBlockType() == Block.buildType.Building)
                    {
                        gridMap[ii,jj].SetCoordY(0);
                    }
                    else
                    {
                        gridMap[ii, jj].SetCoordY(-2.45f);
                    }

                }
            }
        }

        public Block[,] GetGridMap()
        {
            return gridMap;
        }
        
        // output function for debugging
        public void PrintGrid()
        {
            for(int ii = 0; ii < sizeX; ii++)
            {
                for(int jj = 0; jj < sizeZ; jj++)
                {
                    Debug.Write(gridMap[jj, ii].GetBlockType() + " ");
                }

                Debug.Write("\n");
            }
        }

        public void PrintCoords()
        {
            for (int ii = 0; ii < sizeX; ii++)
            {
                for (int jj = 0; jj < sizeZ; jj++)
                {
                    Debug.WriteLine("Coord X: " + gridMap[ii,jj].GetCoordX() +" Y: "+ gridMap[ii, jj].GetCoordY() + " Z: "+ gridMap[ii,jj].GetCoordZ());
                }
            }
        }



    }

    
}
