using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentOne_Pigeon_Sim
{
    public abstract class Subject
    {
        public string modelPath;
        public string texturePath;
        public ContentManager Content;
        public Model subjectModel;
        public Texture2D subjectTexture;
        public Vector3 futurePosition;
        public Vector3 subjectPosition;
        public Vector3 subjectRotation;
        public float subjectScale;
        public float subjectSpeed;
        public float subjectRotateSpeed;
        public Vector3 minPoint;
        public Vector3 maxPoint;
        public Vector3 AABBOffset;
        private List<Actor> observerList= new List<Actor>();


        // http://rbwhitaker.wikidot.com/using-3d-models
        public void SubjectDraw(Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in subjectModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world * SubjectInit();
                    effect.View = view;
                    effect.Projection = projection;
                    effect.TextureEnabled = true;
                    effect.Texture = subjectTexture;
                }

                mesh.Draw();
            }
        }

        public Matrix SubjectInit()
        {

            float radianX = SubjectRadians(subjectRotation.X);
            float radianY = SubjectRadians(subjectRotation.Y);
            float radianZ = SubjectRadians(subjectRotation.Z);

            Matrix objScale = Matrix.CreateScale(subjectScale);
            Matrix objTranslate = Matrix.CreateTranslation(subjectPosition);
            Matrix objRotateX = Matrix.CreateRotationX(radianX);
            Matrix objRotateY = Matrix.CreateRotationY(radianY);
            Matrix objRotateZ = Matrix.CreateRotationZ(radianZ);

            Matrix objPosition = objScale * objRotateX * objRotateY * objRotateZ * objTranslate;

            return objPosition;
        }

        public abstract Matrix SubjectUpdate(Vector3 inputVector, float deltaTime, float fps);

        public void SetObservers(Actor targetActor)
        {
            this.observerList.Add(targetActor);
        }

        public List<Actor> GetObservers()
        {
            return observerList;
        }

        public float SubjectRadians(float inputDegrees)
        {
            return (float)(inputDegrees * (Math.PI / 180));
        }
        
        public void AABBCollider(Actor targetActor)
        {
            float intersectX = subjectPosition.X - targetActor.actorPosition.X;
            float intersectZ = subjectPosition.Z - targetActor.actorPosition.Z;

            if (Math.Abs(intersectX) > Math.Abs(intersectZ))
            {
                if (intersectX > 0)
                {
                    this.subjectPosition.X += 0.5f;
                }
                else
                {
                    this.subjectPosition.X -= 0.5f;
                }
            }
            else
            {
                if (intersectZ > 0)
                {
                    this.subjectPosition.Z += 0.5f;
                }
                else
                {
                    this.subjectPosition.Z -= 0.5f;
                }
            }
        }

        // https://gamedev.stackexchange.com/questions/44500/how-many-and-which-axes-to-use-for-3d-obb-collision-with-sat
        public void AABBResolution(Actor targetActor, float deltaTime, float fps)
        {

            List<Vector3> targetVectors = new List<Vector3>();
            List<Vector3> subjectVectors = new List<Vector3>();

            targetVectors.Add(new Vector3(targetActor.actorPosition.X - targetActor.AABBOffset.X, targetActor.actorPosition.Y - targetActor.AABBOffset.Y, targetActor.actorPosition.Z - targetActor.AABBOffset.Z));
            targetVectors.Add(new Vector3(targetActor.actorPosition.X + targetActor.AABBOffset.X, targetActor.actorPosition.Y - targetActor.AABBOffset.Y, targetActor.actorPosition.Z - targetActor.AABBOffset.Z));
            targetVectors.Add(new Vector3(targetActor.actorPosition.X + targetActor.AABBOffset.X, targetActor.actorPosition.Y + targetActor.AABBOffset.Y, targetActor.actorPosition.Z - targetActor.AABBOffset.Z));
            targetVectors.Add(new Vector3(targetActor.actorPosition.X - targetActor.AABBOffset.X, targetActor.actorPosition.Y + targetActor.AABBOffset.Y, targetActor.actorPosition.Z - targetActor.AABBOffset.Z));
            targetVectors.Add(new Vector3(targetActor.actorPosition.X - targetActor.AABBOffset.X, targetActor.actorPosition.Y - targetActor.AABBOffset.Y, targetActor.actorPosition.Z + targetActor.AABBOffset.Z));
            targetVectors.Add(new Vector3(targetActor.actorPosition.X + targetActor.AABBOffset.X, targetActor.actorPosition.Y - targetActor.AABBOffset.Y, targetActor.actorPosition.Z + targetActor.AABBOffset.Z));
            targetVectors.Add(new Vector3(targetActor.actorPosition.X + targetActor.AABBOffset.X, targetActor.actorPosition.Y + targetActor.AABBOffset.Y, targetActor.actorPosition.Z + targetActor.AABBOffset.Z));
            targetVectors.Add(new Vector3(targetActor.actorPosition.X - targetActor.AABBOffset.X, targetActor.actorPosition.Y + targetActor.AABBOffset.Y, targetActor.actorPosition.Z + targetActor.AABBOffset.Z));

            subjectVectors.Add(new Vector3(subjectPosition.X - AABBOffset.X, subjectPosition.Y - AABBOffset.Y, subjectPosition.Z - AABBOffset.Z));
            subjectVectors.Add(new Vector3(subjectPosition.X + AABBOffset.X, subjectPosition.Y - AABBOffset.Y, subjectPosition.Z - AABBOffset.Z));
            subjectVectors.Add(new Vector3(subjectPosition.X + AABBOffset.X, subjectPosition.Y + AABBOffset.Y, subjectPosition.Z - AABBOffset.Z));
            subjectVectors.Add(new Vector3(subjectPosition.X - AABBOffset.X, subjectPosition.Y + AABBOffset.Y, subjectPosition.Z - AABBOffset.Z));
            subjectVectors.Add(new Vector3(subjectPosition.X - AABBOffset.X, subjectPosition.Y - AABBOffset.Y, subjectPosition.Z + AABBOffset.Z));
            subjectVectors.Add(new Vector3(subjectPosition.X + AABBOffset.X, subjectPosition.Y - AABBOffset.Y, subjectPosition.Z + AABBOffset.Z));
            subjectVectors.Add(new Vector3(subjectPosition.X + AABBOffset.X, subjectPosition.Y + AABBOffset.Y, subjectPosition.Z + AABBOffset.Z));
            subjectVectors.Add(new Vector3(subjectPosition.X - AABBOffset.X, subjectPosition.Y + AABBOffset.Y, subjectPosition.Z + AABBOffset.Z));

            Vector3 theMTV = MinimumTranslationVector(targetVectors, subjectVectors, deltaTime, fps);

            if (theMTV.Length() > 0)
            {

                if (theMTV.Length() < 0)
                {
                    subjectPosition = Vector3.Subtract(subjectPosition, theMTV);
                }
                else
                {
                    subjectPosition = Vector3.Add(subjectPosition, theMTV);
                }
            }


        }

        // finding the closest edge normal
        // https://gamedev.stackexchange.com/questions/26951/calculating-the-2d-edge-normals-of-a-triangle
        public Vector3 ProjectionNormal(List<Vector3> targetVertices, List<Vector3> subjectVertices)
        {
            List<Vector3> tempEdges = new List<Vector3>();
            List<Vector3> edgeVectors = new List<Vector3>();
            Vector3 normalVector;
            int targetIndex = 0;
            int subjectIndex = 0;


            // getting edges
            for (int ii = 0; ii < targetVertices.Count; ii++)
            {
                for (int jj = 0; jj < targetVertices.Count; jj++)
                {
                    Vector3 tempVector;
                    tempVector = targetVertices[ii] - targetVertices[jj];
                    if (tempVector.Length() != 0)
                    {
                        tempEdges.Add(tempVector);
                    }
                }
            }

            // getting faces
            for (int ii = 0; ii < tempEdges.Count; ii++)
            {
                for (int jj = 0; jj < tempEdges.Count; jj++)
                {
                    Vector3 tempVector;
                    tempVector = tempEdges[ii] - tempEdges[jj];
                    if (tempVector.Length() != 0)
                    {
                        edgeVectors.Add(tempVector);
                    }
                }
            }

            // aggregating all the faces and edges
            for (int ii = 0; ii < targetVertices.Count; ii++)
            {
                edgeVectors.Add(targetVertices[ii]);
            }


            // selecting the closest vertices between target and subject
            Vector3 magnitudeCheck = edgeVectors[0] - subjectVertices[0];
            for (int ii = 0; ii < edgeVectors.Count; ii++)
            {
                for (int jj = 0; jj < subjectVertices.Count; jj++)
                {
                    Vector3 magnitudeComparison = edgeVectors[ii] - subjectVertices[jj];
                    if (magnitudeComparison.Length() < magnitudeCheck.Length())
                    {
                        magnitudeCheck = magnitudeComparison;
                        targetIndex = ii;
                        subjectIndex = jj;
                    }
                }
            }
            /*
            Vector3 magnitudeCheck = targetVertices[0] - subjectVertices[0];
            for (int ii = 0; ii < targetVertices.Count; ii++)
            {
                for (int jj = 0; jj < subjectVertices.Count; jj++)
                {
                    Vector3 magnitudeComparison = targetVertices[ii] - subjectVertices[jj];
                    if (magnitudeComparison.Length() < magnitudeCheck.Length())
                    {
                        magnitudeCheck = magnitudeComparison;
                        targetIndex = ii;
                        subjectIndex = jj;
                    }
                }
            }
            */

            Debug.WriteLine("target index: " + targetIndex + " subject index: " + subjectIndex);

            normalVector = Vector3.Cross(edgeVectors[targetIndex], subjectVertices[subjectIndex]);
            //normalVector.Normalize();

            float normalCheck = Vector3.Dot(normalVector, subjectVertices[subjectIndex]);

            // ensures that the normals are pointing out
            if (normalCheck > 0)
            {
                normalVector = normalVector * -1;
            }

            Debug.WriteLine("Normal Vector: " + normalVector);


            return normalVector;
        }

        // https://math.stackexchange.com/questions/633181/formula-to-project-a-vector-onto-a-plane
        // https://www.maplesoft.com/support/help/maple/view.aspx?path=MathApps%2FProjectionOfVectorOntoPlane
        // https://math.oregonstate.edu/home/programs/undergrad/CalculusQuestStudyGuides/vcalc/dotprod/dotprod.html
        public Vector3 VectorProjection(List<Vector3> targetVectors, List<Vector3> subjectVectors)
        {
            Vector3 axisNormal;
            List<Vector3> edgeVector = new List<Vector3>();
            int targetIndex = 0;
            int subjectIndex = 0;

            axisNormal = ProjectionNormal(targetVectors, subjectVectors);

            for (int ii = 0; ii < targetVectors.Count - 2; ii++)
            {
                Vector3 tempEdgeOne;
                Vector3 tempEdgeTwo;
                Vector3 tempEdgeClosest;

                tempEdgeOne = targetVectors[ii] - targetVectors[ii + 1];
                tempEdgeTwo = targetVectors[ii + 1] - targetVectors[ii + 2];
                tempEdgeClosest = tempEdgeOne - tempEdgeTwo;

                edgeVector.Add(tempEdgeClosest);
            }

            for (int ii = 0; ii < targetVectors.Count; ii++)
            {
                edgeVector.Add(targetVectors[ii]);
            }

            Vector3 magnitudeCheck = edgeVector[0] - subjectVectors[0];
            for (int ii = 0; ii < edgeVector.Count; ii++)
            {
                for (int jj = 0; jj < subjectVectors.Count; jj++)
                {
                    Vector3 magnitudeComparison = edgeVector[ii] - subjectVectors[jj];
                    if (magnitudeComparison.Length() < magnitudeCheck.Length())
                    {
                        magnitudeCheck = magnitudeComparison;
                        targetIndex = ii;
                        subjectIndex = jj;
                    }
                }

            }

            /*
            Vector3 magnitudeCheck = targetVectors[0] - subjectVectors[0];
            for (int ii = 0; ii < targetVectors.Count; ii++)
            {
                for (int jj = 0; jj < subjectVectors.Count; jj++)
                {
                    Vector3 magnitudeComparison = targetVectors[ii] - subjectVectors[jj];
                    if (magnitudeComparison.Length() < magnitudeCheck.Length())
                    {
                        magnitudeCheck = magnitudeComparison;
                        targetIndex = ii;
                        subjectIndex = jj;
                    }
                }

            }
            */

            // this is correct
            float theScalar = Vector3.Dot(axisNormal, subjectVectors[subjectIndex]);

            float theUnitScalar = theScalar / subjectVectors[subjectIndex].LengthSquared();

            Vector3 theProjection = Vector3.Multiply(axisNormal, theUnitScalar);

            return theProjection;
        }

        public Vector3 ProjectionOverlap(List<Vector3> targetVectors, List<Vector3> subjectVectors)
        {
            Vector3 targetProjection;
            Vector3 subjectProjection;
            Vector3 theOverlap;

            targetProjection = VectorProjection(targetVectors, subjectVectors);
            subjectProjection = VectorProjection(subjectVectors, targetVectors);

            theOverlap = Vector3.Subtract(targetProjection, subjectProjection);

            return theOverlap;
        }

        // https://gamedev.stackexchange.com/questions/32545/what-is-the-mtv-minimum-translation-vector-in-sat-seperation-of-axis
        // https://stackoverflow.com/questions/40255953/finding-the-mtv-minimal-translation-vector-using-separating-axis-theorem
        // calculating depth penetration using SAT to find the minimum translation vector
        public Vector3 MinimumTranslationVector(List<Vector3> targetVectors, List<Vector3> subjectVectors, float deltaTime, float fps)
        {
            //Projection targetObject;
            Vector3 axisNormal;

            Vector3 theOverlap;

            float overlapDepth;
            Vector3 theMTV;

            axisNormal = ProjectionNormal(targetVectors, subjectVectors);
            Debug.WriteLine("axis normal:" + axisNormal);
            theOverlap = ProjectionOverlap(targetVectors, subjectVectors);
            Debug.WriteLine("the overlap:" + theOverlap);
            overlapDepth = theOverlap.Length();
            Debug.WriteLine("overlap depth:" + overlapDepth);
            theMTV = Vector3.Multiply(axisNormal, overlapDepth);
            theMTV.Normalize();
            // MTV is usually the normal of the vector times the overlapdepth
            // projection normal is analogous to axis
            Debug.WriteLine("MTV:" + theMTV);

            return theMTV * deltaTime * fps * 0.5f;
        }

    }
}
