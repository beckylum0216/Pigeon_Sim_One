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
    public abstract class Actor
    {
        public string modelPath;
        public string texturePath;
        public ContentManager Content;
        public Model actorModel;
        public Texture2D actorTexture;
        public Vector3 actorPosition;
        public Vector3 actorRotation;
        public float actorScale;
        public float actorSpeed;
        public float actorRotateSpeed;

        public abstract bool ActorCollider();
        public abstract Matrix ActorInit();
        public abstract void ActorDraw(Matrix world, Matrix view, Matrix projection);
        public abstract float ActorRadians(float inputDegrees);
        

    }
}
