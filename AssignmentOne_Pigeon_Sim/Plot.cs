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
                            Vector3 inputPosition, Vector3 inputRotation, float inputScale)
        {
            this.modelPath = modelFile;
            this.texturePath = textureFile;

            this.actorModel = Content.Load<Model>(modelPath);
            this.actorTexture = Content.Load<Texture2D>(texturePath);
            this.actorPosition = inputPosition;
            this.actorRotation = inputRotation;
            this.actorScale = inputScale;
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

        public override void ActorDraw(Matrix world, Matrix view, Matrix projection)
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

        public override float ActorRadians(float inputDegrees)
        {
            return (float)(inputDegrees * (Math.PI / 180));
        }

        public override Matrix ActorInit()
        {

            float radianX = ActorRadians(actorRotation.X);
            float radianY = ActorRadians(actorRotation.Y);
            float radianZ = ActorRadians(actorRotation.Z);


            Matrix objScale = Matrix.CreateScale(actorScale);
            Matrix objTranslate = Matrix.CreateTranslation(actorPosition);
            Matrix objRotateX = Matrix.CreateRotationX(radianX);
            Matrix objRotateY = Matrix.CreateRotationY(radianY);
            Matrix objRotateZ = Matrix.CreateRotationZ(radianZ);

            Matrix objPosition =  objScale * objRotateX * objRotateY * objRotateZ * objTranslate;

            return objPosition;
        }

        public override bool ActorCollider()
        {
            return false;
        }

        public Actor ActorClone()
        {
            return new Plot(Content, modelPath, texturePath, actorPosition, actorRotation, actorScale);
        }

    }
}
