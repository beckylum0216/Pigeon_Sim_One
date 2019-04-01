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
    public abstract class Actor : ICollidable<Actor>
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
        public Vector3 minPoint;
        public Vector3 maxPoint;
        public Vector3 AABBOffset;


        // http://rbwhitaker.wikidot.com/using-3d-models
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

        public abstract Matrix ActorUpdate(Vector3 inputVector);

        public float ActorRadians(float inputDegrees)
        {
            return (float)(inputDegrees * (Math.PI / 180));
        }

        public bool AABBtoAABB(Actor targetActor)
        {

            return (maxPoint.X > targetActor.minPoint.X &&
                    minPoint.X < targetActor.maxPoint.X &&
                    maxPoint.Y > targetActor.minPoint.Y &&
                    minPoint.Y < targetActor.maxPoint.Y &&
                    maxPoint.Z > targetActor.minPoint.Z &&
                    minPoint.Z < targetActor.maxPoint.Z);
        }

        // https://gamedev.stackexchange.com/questions/44500/how-many-and-which-axes-to-use-for-3d-obb-collision-with-sat
        public void AABBResolution(Actor targetActor)
        {

            List<Vector3> targetVectors = new List<Vector3>();
            List<Vector3> actorVectors = new List<Vector3>();

            if(AABBtoAABB(targetActor))
            {
                targetVectors.Add(new Vector3(targetActor.actorPosition.X - targetActor.AABBOffset.X, targetActor.actorPosition.Y - targetActor.AABBOffset.Y, targetActor.actorPosition.Z - targetActor.AABBOffset.Z));
                targetVectors.Add(new Vector3(targetActor.actorPosition.X + targetActor.AABBOffset.X, targetActor.actorPosition.Y - targetActor.AABBOffset.Y, targetActor.actorPosition.Z - targetActor.AABBOffset.Z));
                targetVectors.Add(new Vector3(targetActor.actorPosition.X + targetActor.AABBOffset.X, targetActor.actorPosition.Y + targetActor.AABBOffset.Y, targetActor.actorPosition.Z - targetActor.AABBOffset.Z));
                targetVectors.Add(new Vector3(targetActor.actorPosition.X - targetActor.AABBOffset.X, targetActor.actorPosition.Y + targetActor.AABBOffset.Y, targetActor.actorPosition.Z - targetActor.AABBOffset.Z));
                targetVectors.Add(new Vector3(targetActor.actorPosition.X - targetActor.AABBOffset.X, targetActor.actorPosition.Y - targetActor.AABBOffset.Y, targetActor.actorPosition.Z + targetActor.AABBOffset.Z));
                targetVectors.Add(new Vector3(targetActor.actorPosition.X + targetActor.AABBOffset.X, targetActor.actorPosition.Y - targetActor.AABBOffset.Y, targetActor.actorPosition.Z + targetActor.AABBOffset.Z));
                targetVectors.Add(new Vector3(targetActor.actorPosition.X + targetActor.AABBOffset.X, targetActor.actorPosition.Y + targetActor.AABBOffset.Y, targetActor.actorPosition.Z + targetActor.AABBOffset.Z));
                targetVectors.Add(new Vector3(targetActor.actorPosition.X - targetActor.AABBOffset.X, targetActor.actorPosition.Y + targetActor.AABBOffset.Y, targetActor.actorPosition.Z + targetActor.AABBOffset.Z));

                actorVectors.Add(new Vector3(actorPosition.X - AABBOffset.X, actorPosition.Y - AABBOffset.Y, actorPosition.Z - AABBOffset.Z));
                actorVectors.Add(new Vector3(actorPosition.X + AABBOffset.X, actorPosition.Y - AABBOffset.Y, actorPosition.Z - AABBOffset.Z));
                actorVectors.Add(new Vector3(actorPosition.X + AABBOffset.X, actorPosition.Y + AABBOffset.Y, actorPosition.Z - AABBOffset.Z));
                actorVectors.Add(new Vector3(actorPosition.X - AABBOffset.X, actorPosition.Y + AABBOffset.Y, actorPosition.Z - AABBOffset.Z));
                actorVectors.Add(new Vector3(actorPosition.X - AABBOffset.X, actorPosition.Y - AABBOffset.Y, actorPosition.Z + AABBOffset.Z));
                actorVectors.Add(new Vector3(actorPosition.X + AABBOffset.X, actorPosition.Y - AABBOffset.Y, actorPosition.Z + AABBOffset.Z));
                actorVectors.Add(new Vector3(actorPosition.X + AABBOffset.X, actorPosition.Y + AABBOffset.Y, actorPosition.Z + AABBOffset.Z));
                actorVectors.Add(new Vector3(actorPosition.X - AABBOffset.X, actorPosition.Y + AABBOffset.Y, actorPosition.Z + AABBOffset.Z));

                Vector3 theMTV = MinimumTranslationVector(targetVectors, actorVectors);

                if (theMTV.Length() > 0)
                {

                    if (theMTV.Length() < 0)
                    {
                        actorPosition = Vector3.Subtract(actorPosition, theMTV);
                    }
                    else
                    {
                        actorPosition = Vector3.Add(actorPosition, theMTV);
                    }
                }
            }
        }

        // finding the closest normal 
        public Vector3 ProjectionNormal(List <Vector3> targetVertices, List<Vector3> actorVertices)
        {
            List<Vector3> tempEdges = new List<Vector3>();
            List<Vector3> edgeVectors = new List<Vector3>();
            Vector3 normalVector;
            int targetIndex = 0;
            int actorIndex = 0;
            
            // getting corner cases
            for(int ii = 0; ii < targetVertices.Count; ii++)
            {
                for(int jj = 0; jj < targetVertices.Count; jj++)
                {
                    Vector3 tempVector;
                    tempVector = targetVertices[ii] - targetVertices[jj];
                    if(tempVector.Length() != 0)
                    {
                        tempEdges.Add(tempVector);
                    }
                }
            }


            for(int ii = 0; ii < tempEdges.Count; ii++)
            {
                for(int jj = 0; jj < tempEdges.Count; jj++)
                {
                    Vector3 tempVector;
                    tempVector = tempEdges[ii] - tempEdges[jj];
                    if(tempVector.Length() != 0)
                    {
                        edgeVectors.Add(tempVector);
                    }
                }
            }

            // aggregating all the corners and edges
            for (int ii = 0; ii < targetVertices.Count; ii++)
            {
                edgeVectors.Add(targetVertices[ii]);
            }

            // selecting the closest vertices between target and actor
            Vector3 magnitudeCheck = edgeVectors[0] - actorVertices[0];
            for(int ii = 0; ii < edgeVectors.Count; ii++)
            {
                for(int jj = 0; jj < actorVertices.Count; jj++)
                {
                    Vector3 magnitudeComparison = edgeVectors[ii] - actorVertices[jj];
                    if(magnitudeComparison.Length() < magnitudeCheck.Length())
                    {
                        magnitudeCheck = magnitudeComparison;
                        targetIndex = ii;
                        actorIndex = jj;
                    }
                }
            }

            Debug.WriteLine("target index: " + targetIndex + " actor index: " + actorIndex);

            normalVector = Vector3.Cross(edgeVectors[targetIndex], actorVertices[actorIndex]);
            //normalVector.Normalize();

            return normalVector;
        }

        // https://math.stackexchange.com/questions/633181/formula-to-project-a-vector-onto-a-plane
        // https://www.maplesoft.com/support/help/maple/view.aspx?path=MathApps%2FProjectionOfVectorOntoPlane
        // https://math.oregonstate.edu/home/programs/undergrad/CalculusQuestStudyGuides/vcalc/dotprod/dotprod.html
        public Vector3 VectorProjection(List<Vector3> targetVectors, List<Vector3> actorVectors)
        {
            Vector3 axisNormal;
            List<Vector3> edgeVector = new List<Vector3>();
            int targetIndex = 0;
            int actorIndex = 0;
            
            axisNormal = ProjectionNormal(targetVectors, actorVectors);

            for(int ii = 0; ii < targetVectors.Count - 2; ii++)
            {
                Vector3 tempEdgeOne;
                Vector3 tempEdgeTwo;
                Vector3 tempEdgeClosest;

                tempEdgeOne = targetVectors[ii] - targetVectors[ii + 1];
                tempEdgeTwo = targetVectors[ii + 1] - targetVectors[ii + 2];
                tempEdgeClosest = tempEdgeOne - tempEdgeTwo;

                edgeVector.Add(tempEdgeClosest);
            }

            for(int ii = 0; ii < targetVectors.Count; ii++)
            {
                edgeVector.Add(targetVectors[ii]);
            }

            Vector3 magnitudeCheck = edgeVector[0] - actorVectors[0];
            for(int ii = 0; ii < edgeVector.Count; ii++)
            {
                for(int jj = 0; jj < actorVectors.Count; jj++)
                {
                    Vector3 magnitudeComparison = edgeVector[ii] - actorVectors[jj];
                    if(magnitudeComparison.Length() < magnitudeCheck.Length())
                    {
                        magnitudeCheck = magnitudeComparison;
                        targetIndex = ii;
                        actorIndex = jj;
                    }
                }

            }

            float theScalar = Vector3.Dot(axisNormal, actorVectors[actorIndex]);

            float theUnitScalar = theScalar / actorVectors[actorIndex].LengthSquared();

            Vector3 theProjection = Vector3.Multiply(axisNormal, theUnitScalar);

            return theProjection;
        }

        public Vector3 ProjectionOverlap(List<Vector3> targetVectors, List<Vector3> actorVectors)
        {
            Vector3 targetProjection;
            Vector3 actorProjection;
            Vector3 theOverlap;

            targetProjection = VectorProjection(targetVectors, actorVectors);
            actorProjection = VectorProjection(actorVectors, targetVectors);
            
            theOverlap = Vector3.Subtract(targetProjection, actorProjection);

            return theOverlap;
        }

        // https://gamedev.stackexchange.com/questions/32545/what-is-the-mtv-minimum-translation-vector-in-sat-seperation-of-axis
        // https://stackoverflow.com/questions/40255953/finding-the-mtv-minimal-translation-vector-using-separating-axis-theorem
        // calculating depth penetration using SAT to find the minimum translation vector
        public Vector3 MinimumTranslationVector(List<Vector3> targetVectors, List<Vector3> actorVectors)
        {
            //Projection targetObject;
            Vector3 axisNormal;

            Vector3 theOverlap;

            float overlapDepth;
            Vector3 theMTV;

            axisNormal = ProjectionNormal(targetVectors, actorVectors);

            theOverlap = ProjectionOverlap(targetVectors, actorVectors);

            overlapDepth = theOverlap.Length();

            theMTV = Vector3.Multiply(axisNormal, overlapDepth);
            theMTV.Normalize();
            // MTV is usually the normal of the vector times the overlapdepth
            // projection normal is analogous to axis
            Debug.WriteLine("MTV:" + theMTV);
            
            
            return theMTV;
        }


    }
}
