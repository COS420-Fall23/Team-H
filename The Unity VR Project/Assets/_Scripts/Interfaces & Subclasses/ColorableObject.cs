using UnityEngine;

namespace LatticeLand
{
    public class ColorableObject : MonoBehaviour, IInteractableObject
    {
        protected InteractableGridObjectType _objectType;

        public virtual void ApplyColorMaterial(Material newColor)
        {
            Debug.Log("Apply Color Is Triggering at ColorableObject Level");
        }

        public virtual void ResetColorMaterial(){}

        public virtual void SetStateConditionally(InteractableObjectState newState){}

        public InteractableGridObjectType GetObjectType()
        {
            return _objectType;
        }
    }
}