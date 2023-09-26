using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LatticeMesh : MonoBehaviour
{
    [SerializeField] private Vector3[] _vertices;
    [SerializeField] private Vector2[] _uv;
    [SerializeField] private int[] _triangles;
    [SerializeField] private Vector3[] _normals;
    private Mesh _latticeShapeMesh;

    private void OnEnable()
    {
        _latticeShapeMesh = new Mesh() { name = "LatticeShapeMesh" };
        _normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, };
        _latticeShapeMesh.vertices = _vertices;
        _latticeShapeMesh.uv = _uv;
        _latticeShapeMesh.triangles = _triangles;
        _latticeShapeMesh.normals = _normals;
        GetComponent<MeshFilter>().mesh = _latticeShapeMesh;
    }

    private void Update()
    {
        _latticeShapeMesh.vertices = _vertices;
        _latticeShapeMesh.triangles = _triangles;
    }
}