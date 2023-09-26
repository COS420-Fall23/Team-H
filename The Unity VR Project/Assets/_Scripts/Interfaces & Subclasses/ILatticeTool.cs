using UnityEngine;

namespace LatticeLand
{
    public interface ILatticeTool
    {
        void TriggerEnter(Collider other);

        void TriggerExit(Collider other);
    }
}