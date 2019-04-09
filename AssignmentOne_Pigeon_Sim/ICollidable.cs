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
        /** 
        *   @brief Interface function implementing observer pattern
        *   @see 
        *	@param targetCollider the object to test against
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
        bool AABBtoAABB(T targetCollider);
       

    }
}
