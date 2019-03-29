using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentOne_Pigeon_Sim
{
    class SkyBox:Actor
    {
        public SkyBox(ContentManager Content, String modelFile, String textureFile,
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

        public override void ActorDraw(Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in actorModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world * ActorInit();
                    effect.View = view;
                    effect.Projection = projection;
                    effect.TextureEnabled = false;
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

            float scale = 0.25f;
            float radianX = ActorRadians(0);
            float radianY = ActorRadians(0);
            float radianZ = ActorRadians(0);


            Matrix objScale = Matrix.CreateScale(scale);
            Matrix objTranslate = Matrix.CreateTranslation(0, 0, 0);
            Matrix objRotateX = Matrix.CreateRotationX(radianX);
            Matrix objRotateY = Matrix.CreateRotationY(radianY);
            Matrix objRotateZ = Matrix.CreateRotationZ(radianZ);

            Matrix objPosition = objScale * objRotateX * objRotateY * objRotateZ * objTranslate;

            return objPosition;
        }

        public override bool ActorCollider()
        {
            return false;
        }

        

    }
}
