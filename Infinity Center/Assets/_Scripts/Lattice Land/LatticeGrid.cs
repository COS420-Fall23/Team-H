using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Mathematics;
using UnityEditor;

public class LatticeGrid : MonoBehaviour
{
    #region Generate Grid Variables
    [Header("Generate Grid Variables")]

    [SerializeField] private GameObject _gridPointPrefab;
    [SerializeField] private int3 _gridDimensions;
    [SerializeField] private float _gapBetweenGridPoints;
    [SerializeField] private float _gridPointScale;
    [SerializeField] private float _heightOfGridStart;

    #endregion

    private void Start()
    {
        GenerateLatticeGrid_Default();
    }

    #region Functions

    public void GenerateLatticeGrid_Default()
    {
        // Attempted to Auto-center grid in world space:
        // gameObject.transform.position = new Vector3(-(((_gridDimensions.x * _gapBetweenGridPoints) * _gridPointScale)/2), _heightOfGridStart, -(((_gridDimensions.z * _gapBetweenGridPoints ) * _gridPointScale)/2));
        
        for (int x = 0; x < _gridDimensions.x; x++)
        {
            for (int y = 0; y < _gridDimensions.y; y++)
            {
                for (int z = 0; z < _gridDimensions.z; z++)
                {
                    Vector3 calcSpawnPoint = new Vector3(x * _gapBetweenGridPoints, y * _gapBetweenGridPoints,
                        z * _gapBetweenGridPoints);
                    GameObject newGridPoint = Instantiate(_gridPointPrefab, gameObject.transform);
                    newGridPoint.transform.localPosition = calcSpawnPoint;
                    GridPoint gridPointComponent = newGridPoint.GetComponent<GridPoint>();
                    gridPointComponent.SetGridPos(x,y,z);
                    gridPointComponent.SetIdleStateScale(_gridPointScale);
                    newGridPoint.transform.localScale = new Vector3(_gridPointScale, _gridPointScale, _gridPointScale);
                }
            }
        }
    }

    #endregion
    
}

#region Debugging Custom Inspector

/*[CustomEditor(typeof(LatticeGrid))]
public class LatticeGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LatticeGrid latticeGrid = (LatticeGrid)target;
        if (GUILayout.Button("Generate Lattice Grid"))
        {
            latticeGrid.GenerateLatticeGrid_Default();
        }
    }
}*/
#endregion
