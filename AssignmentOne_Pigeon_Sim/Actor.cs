using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AssignmentOne_Pigeon_Sim
{
    /// @brief prototype class for actors implements obsever pattern 
    public abstract class Actor : ICollidable<Subject>
    {
        public string modelPath;
        public string texturePath;
        public ContentManager Content;
        public Model actorModel;
        public Texture2D actorTexture;
        public Vector3 futurePosition;
        public Vector3 actorPosition;
        public Vector3 actorRotation;
        public float actorScale;
        public float actorSpeed;
        public float actorRotateSpeed;
        public Vector3 minPoint;
        public Vector3 maxPoint;
        public Vector3 AABBOffset;

        
        /** 
        *   @brief This function draws the actor
        *   @see http://rbwhitaker.wikidot.com/using-3d-models
        *	@param world world coordinates
        *	@param view the camera viewport
        *	@param projection the 3d matrix for projecting onto a 2d screen
        *	@param 
        *	@return void
        *	@pre 
        *	@post 
        */
        public void ActorDraw(Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in actorModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world * ActorInit();
                    effect.View = view;
                    effect.Projection = projection;
                    effect.TextureEnabled = true;
                    effect.Texture = actorTexture;
                }

                mesh.Draw();
            }
        }

        /** 
        *   @brief This function initialises the actor object
        *   @see 
        *	@param 
        *	@param 
        *	@param 
        *	@param 
        *	@return objPosition initialises the object position in game world space
        *	@pre 
        *	@post 
        */
        public Matrix ActorInit()
        {

            float radianX = ActorRadians(actorRotation.X);
            float radianY = ActorRadians(actorRotation.Y);
            float radianZ = ActorRadians(actorRotation.Z);
            
            Matrix objScale = Matrix.CreateScale(actorScale);
            Matrix objTranslate = Matrix.CreateTranslation(actorPosition);
            Matrix objRotateX = Matrix.CreateRotationX(radianX);
            Matrix objRotateY = Matrix.CreateRotationY(radianY);
            Matrix objRotateZ = Matrix.CreateRotationZ(radianZ);

            Matrix objPosition = objScale * objRotateX * objRotateY * objRotateZ * objTranslate;

            return objPosition;
        }

        /** 
        *   @brief This abstract function updates the actor object
        *   @see 
        *	@param inputVector the new positon of the object in vectors
        *	@param 
        *	@param 
        *	@param 
        *	@return 
        *	@pre 
        *	@post 
        */
        public abstract Matrix ActorUpdate(Vector3 inputVector);

        /** 
        *   @brief This function converts degrees to radians. It a convenience function, should use math.
        *   @see 
        *	@param inputDegrees the degree to convert to radians
        *	@param 
        *	@param 
        *	@param 
        *	@return 
        *	@pre 
        *	@post 
        */
        public float ActorRadians(float inputDegrees)
        {
            return (float)(inputDegrees * (Math.PI / 180));
        }

        /** 
        *   @brief Abstract function to implement the prototype pattern
        *   @see 
        *	@param Content
        *	@param modelFile the filepath of the model asset
        *	@param textureFile the file path of the texture asset
        *	@param inputPosition the initial position of the asset
        *	@param inputRotation the initial rotation of the asset
        *	@param inputScale the initial scale of the asset
        *	@param inputAABBOffset the bounding box offset
        *	@return Actor a new actor object
        *	@pre 
        *	@post 
        */
        public abstract Actor ActorClone(ContentManager Content, String modelFile, String textureFile, Vector3 inputPosition,
                                    Vector3 inputRotation, float inputScale, Vector3 inputAABBOffset);

        /** 
        *   @brief Function implementing AABB collision detection
        *   @see 
        *	@param targetActor the subject to test against
        *	@param 
        *	@param 
        *	@param 
        *	@param 
        *	@param 
        *	@param 
        *	@return boolean Whether the objects collide
        *	@pre 
        *	@post 
        */
        public bool AABBtoAABB(Subject targetActor)
        {

            return (maxPoint.X > targetActor.minPoint.X &&
                    minPoint.X < targetActor.maxPoint.X &&
                    maxPoint.Y > targetActor.minPoint.Y &&
                    minPoint.Y < targetActor.maxPoint.Y &&
                    maxPoint.Z > targetActor.minPoint.Z &&
                    minPoint.Z < targetActor.maxPoint.Z);
        }

        
    }
}
