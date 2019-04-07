using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentOne_Pigeon_Sim
{
    // base clase for map generation smallest unit is a "Block"

    public class Block
    {

        // the abstracted grid position
        private int positionX; // oopsie
        private int positionY; // oopsie
        private int positionZ; // oopsie
        private float coordX;
        private float coordY;
        private float coordZ;
        private float blockScale;
        private Vector3 blockRotation;

        // enum of building types
        public enum buildType {NullBlock, RoadHorizontal, RoadVertical, RoadCorner, Building, RoadT, RoadCross, SkyBox};
        private buildType blockType;

        private String modelPath;
        private String texturePath;

        public Block()
        {
            this.blockType = buildType.NullBlock;
        }

        public Block(int gridX, int gridY, int gridZ, buildType gridType, string modelFile, 
                        string textureFile, float gridScale, Vector3 gridRotation)
        {
            this.positionX = gridX;
            this.positionY = gridY;
            this.positionZ = gridZ;
            this.blockType = gridType;
            this.modelPath = modelFile;
            this.texturePath = textureFile;
            this.blockScale = gridScale;
            this.blockRotation = gridRotation;
        }

        public void SetPositionX(int inputX)
        {
            this.positionX = inputX;
        }

        public int GetPositionX()
        {
            return this.positionX;
        }

        public void SetPositionY(int inputY)
        {
            this.positionY = inputY;
        }

        public int GetPositionY()
        {
            return this.positionY;
        }

        public void SetPositionZ(int inputZ)
        {
            this.positionZ = inputZ;
        }

        public int GetPositionZ()
        {
            return this.positionZ;
        }

        public void SetBlockType(buildType inputType)
        {
            this.blockType = inputType;
        }

        public buildType GetBlockType()
        {
            return this.blockType;
        }

        public void SetCoordX(float inputX)
        {
            this.coordX = inputX;
        }

        public float GetCoordX()
        {
            return this.coordX;
        }

        public void SetCoordY(float inputY)
        {
            this.coordY = inputY;
        }

        public float GetCoordY()
        {
            return this.coordY;
        }

        public void SetCoordZ(float inputZ)
        {
            this.coordZ = inputZ;
        }

        public float GetCoordZ()
        {
            return this.coordZ;
        }

        public void SetModelPath(string modelFile)
        {
            this.modelPath = modelFile;
        }

        public string GetModelPath()
        {
            return this.modelPath;
        }

        public void SetTexturePath(string textureFile)
        {
            this.texturePath = textureFile;
        }

        public string GetTexturePath()
        {
            return this.texturePath;
        }

        public void SetBlockScale(float gridScale)
        {
            this.blockScale = gridScale;
        }

        public float GetBlockScale()
        {
            return this.blockScale;
        }

        public void SetBlockRotation(Vector3 gridRotation)
        {
            this.blockRotation = gridRotation;
        }

        public Vector3 GetBlockRotation()
        {
            return this.blockRotation;
        }

    }
}
