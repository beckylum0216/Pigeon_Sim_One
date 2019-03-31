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
        


    }
}
