using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;

namespace LatticeLand
{
    public class Polygon : MonoBehaviour
    {
        public Vector3[] positions;

        public bool equilateral;
        public bool equiangular;
        public bool regular;
        public int nverts;
        
        private bool _equilateral
        {
            get
            {
                return sideLengths().Distinct().ToArray().Length == 1;
            }
        }
        
        private bool _equiangular
        {
            get
            {
                return angleMeasures().Distinct().ToArray().Length == 1;
            }
        }

        private bool _regular
        {
            get
            {
                return _equilateral && _equiangular;
            }
        }

        private int _nverts
        {
            get
            {
                return positions.Length;
            }
        }
        
        //TODO add method for convex
        public int[] refs;

        public void Start()
        {
            InitializeValues();
            MeshFilter mf = GetComponent<MeshFilter>();
            mf.mesh = InitializeMesh();
        }

        private void InitializeValues()
        {
            equiangular = _equiangular;
            equilateral = _equilateral;
            regular = _regular;
            nverts = _nverts;
        }

        private Mesh InitializeMesh()
        {
            //assumes convex polygon, see algorithm here for generalizing
            //http://www.cs.unc.edu/~dm/CODE/GEM/chapter.html
            Mesh m = new Mesh();
            m.vertices = positions;
            m.triangles = tris();

            return m;
        }
        
        private int[] tris()
        {
            int[] tris = new int[(positions.Length - 2) * 3];
            for(int i = 0; i < positions.Length - 2; i++){
                tris[i * 3 + 0] = positions.Length -1;
                tris[i * 3 + 1] = i +1;
                tris[i * 3 + 2] = i;
            }
            return tris;
        }

        private float[] sideLengths()
        {
            float[] l = new float[positions.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                l[i] = Vector3.Distance(positions[i], positions[(i - 1 + positions.Length) % positions.Length]);
            }

            return l;
        }

        private float[] angleMeasures()
        {
            float[] m = new float[positions.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                //TODO check to see if this angle calculates correctly.
                m[i] = Vector3.Angle(positions[i] - positions[(i + 1) % positions.Length],
                    positions[i] - positions[(i - 1 + positions.Length) % positions.Length]);
            }

            return m;
        }
    }
}