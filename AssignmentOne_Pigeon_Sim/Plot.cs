using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AssignmentOne_Pigeon_Sim
{
    public class Plot : Actor
    {

        public Plot(ContentManager Content,  String modelFile, String textureFile, 
                        Vector3 inputPosition, Vector3 inputRotation, float inputScale, Vector3 inputAABBOffset)
        {
            this.modelPath = modelFile;
            this.texturePath = textureFile;
            this.actorModel = Content.Load<Model>(modelPath);
            this.actorTexture = Content.Load<Texture2D>(texturePath);
            this.actorPosition = inputPosition;
            this.actorRotation = inputRotation;
            this.actorScale = inputScale;
            this.AABBOffset = inputAABBOffset;
            this.maxPoint = this.actorPosition + this.AABBOffset;
            this.minPoint = this.actorPosition - this.AABBOffset;
        }

        public void SetPosition(Vector3 inputPosition)
        {
            this.actorPosition = inputPosition;
        }

        public Vector3 GetPosition()
        {
            return this.actorPosition;
        }

        public void SetRotation(Vector3 inputRotation)
        {
            this.actorRotation = inputRotation;
        }
        
        public Vector3 GetRotation()
        {
            return this.actorRotation;
        }

        public void SetScale(float inputScale)
        {
            this.actorScale = inputScale;
        }

        public float GetScale()
        {
            return this.actorScale;
        }


        public override Matrix ActorUpdate(Vector3 inputVector)
        {
            throw new NotImplementedException();
        }

        
        public Actor ActorClone()
        {
            return new Plot(Content, modelPath, texturePath, actorPosition, actorRotation, actorScale, AABBOffset);
        }

        public void SetMinPoint()
        {
            this.minPoint = actorPosition - AABBOffset;
        }

        public Vector3 GetMinPoint()
        {
            return this.minPoint;
        }

        public void SetMaxPoint()
        {
            this.maxPoint = actorPosition + AABBOffset;
        }

        public Vector3 GetMaxPoint()
        {
            return this.minPoint;
        }

        
    }
}
