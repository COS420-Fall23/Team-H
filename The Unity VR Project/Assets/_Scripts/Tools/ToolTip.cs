using LatticeLand;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    private ILatticeTool _parentTool;

    private void Awake()
    {
        _parentTool = GetComponentInParent<ILatticeTool>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _parentTool.TriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _parentTool.TriggerExit(other);
    }
}