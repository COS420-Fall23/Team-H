using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LatticeLand
{
    public interface IInteractableObject
    {
        public void SetStateConditionally(InteractableObjectState newState);

        public InteractableGridObjectType GetObjectType();
    }    
}

