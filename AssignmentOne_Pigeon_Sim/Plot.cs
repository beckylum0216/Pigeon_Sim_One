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
        /**
	    *	@brief parameterised constructor to the plot object. Create a complete plot object.
	    *	@param Content 
        *	@param modelFile model file path
        *	@param textureFile texture file path
        *	@param predictedPosition predicitve collision method position. Not used
        *	@param inputPosition initial position of the pigeon
        *	@param inputRotation initial rotation of the pigeon
        *	@param inputScale initial scale of the pigeon
        *	@param inputAABBOffset the bounding box offsets
	    *	@return 
	    *	@pre 
	    *	@post Camera will exist
	    */
        public Plot(ContentManager Content,  String modelFile, String textureFile,
                        Vector3 inputPosition, Vector3 inputRotation, float inputScale, Vector3 inputAABBOffset)
        {
            this.modelPath = modelFile;
            this.texturePath = textureFile;
            this.actorModel = Content.Load<Model>(modelPath);
            this.actorTexture = Content.Load<Texture2D>(texturePath);
            this.futurePosition = inputPosition;
            this.actorPosition = inputPosition;
            this.actorRotation = inputRotation;
            this.actorScale = inputScale;
            this.AABBOffset = inputAABBOffset;
            this.maxPoint = this.actorPosition + this.AABBOffset;
            this.minPoint = this.actorPosition - this.AABBOffset;
        }


        /** 
        *   @brief mutator to the actor position 
        *   @see
        *	@param input position the new position
        *	@param  
        *	@param 
        *	@param 
        *	@return void
        *	@pre 
        *	@post 
        */
        public void SetPosition(Vector3 inputPosition)
        {
            this.actorPosition = inputPosition;
        }

        /** 
        *   @brief accessor to the actor position
        *   @see
        *	@param 
        *	@param  
        *	@param 
        *	@param 
        *	@return actorPosition
        *	@pre 
        *	@post 
        */
        public Vector3 GetPosition()
        {
            return this.actorPosition;
        }

        /** 
        *   @brief mutator to set the actor rotation
        *   @see
        *	@param inputRotation new rotation based on vector
        *	@param  
        *	@param 
        *	@param 
        *	@return 
        *	@pre 
        *	@post 
        */
        public void SetRotation(Vector3 inputRotation)
        {
            this.actorRotation = inputRotation;
        }

        /** 
        *   @brief accessor to the rotation of the actor
        *   @see
        *	@param 
        *	@param  
        *	@param 
        *	@param 
        *	@return actorRotation the current actor rotation
        *	@pre 
        *	@post 
        */
        public Vector3 GetRotation()
        {
            return this.actorRotation;
        }

        /** 
        *   @brief mutator to set scale of actor
        *   @see
        *	@param inputScale the new scale
        *	@param  
        *	@param 
        *	@param 
        *	@return void
        *	@pre 
        *	@post 
        */
        public void SetScale(float inputScale)
        {
            this.actorScale = inputScale;
        }

        /** 
        *   @brief accessor to the object's scale
        *   @see
        *	@param 
        *	@param  
        *	@param 
        *	@param 
        *	@return actorScale
        *	@pre 
        *	@post 
        */
        public float GetScale()
        {
            return this.actorScale;
        }

        /** 
        *   @brief function to update the state of the actor. For plot this is not implmented
        *   @see
        *	@param 
        *	@param  
        *	@param 
        *	@param 
        *	@return actorScale
        *	@pre 
        *	@post 
        */
        public override Matrix ActorUpdate(Vector3 inputVector)
        {
            throw new NotImplementedException();
        }

        /** 
        *   @brief Function that implement the prototype pattern clone functionality 
        *   @see
        *	@param 
        *	@param  
        *	@param 
        *	@param 
        *	@return new plot object
        *	@pre 
        *	@post 
        */
        public override Actor ActorClone(ContentManager Content, String modelFile, String textureFile, Vector3 inputPosition,
                                    Vector3 inputRotation, float inputScale, Vector3 inputAABBOffset)
        {
            return new Plot(Content, modelPath, texturePath, actorPosition, actorRotation, actorScale, AABBOffset);
        }

        /** 
        *   @brief mutator to the 
        *   @see
        *	@param 
        *	@param  
        *	@param 
        *	@param 
        *	@return actorScale
        *	@pre 
        *	@post 
        */
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
