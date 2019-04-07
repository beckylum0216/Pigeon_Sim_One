using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentOne_Pigeon_Sim
{
    public interface ICollidable<T>
    {

        bool AABBtoAABB(T targetCollider);
        void AABBResolution(T targetCollider, float deltaTime, float fps);
        Vector3 ProjectionNormal(List<Vector3> targetVectors, List<Vector3> actorVectors);
        Vector3 VectorProjection(List<Vector3> targetVectors, List<Vector3> actorVectors);
        Vector3 ProjectionOverlap(List<Vector3> targetVectors, List<Vector3> actorVectors);
        Vector3 MinimumTranslationVector(List<Vector3> targetVectors, List<Vector3> actorVectors, float deltaTime, float fps);

    }
}
