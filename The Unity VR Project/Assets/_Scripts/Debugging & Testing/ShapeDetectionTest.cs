using System.Collections.Generic;
using System.IO;
using System.Linq;
using LatticeLand.Utils;
using Unity.Mathematics;
using UnityEngine;

namespace LatticeLand
{
    public class ShapeDetectionTest : MonoBehaviour
    {
        public int2[] graph;
        public Dictionary<int2, LineSegment> segments = new Dictionary<int2, LineSegment>();
        public Dictionary<int, GridPoint> points = new Dictionary<int, GridPoint>();
        public List<int[]> cycles;

        public List<int[]> polys;

        public Vector3Int a;
        public Vector3Int b;
        public Vector3Int c;
        public Vector3Int d;
        public Vector3Int e;
        public Vector3Int f;
        public Vector3Int g;
        public Vector3Int h;
        public Vector3Int i;

        // Start is called before the first frame update
        void Awake()
        {
            LatticeGrid lg = GetComponent<LatticeGrid>();
            lg.GenerateLatticeGrid();
        }

        void Start()
        {
            LatticeGrid lg = GetComponent<LatticeGrid>();

            DrawLine(lg, a, b);
            DrawLine(lg, b, c);
            DrawLine(lg, c, a);
            DrawLine(lg, c, d);
            DrawLine(lg, b, d);
            DrawLine(lg, d, e);
            DrawLine(lg, e, c);
            DrawLine(lg, e, a);

            DrawLine(lg, f, g);
            DrawLine(lg, g, h);
            DrawLine(lg, h, f);
            DrawLine(lg, h, i);

            LineSegment[] segs = FindObjectsOfType<LineSegment>();
            List<int> graphNodesList = new List<int>();
            for (int i = 0; i < segs.Length; i++)
            {
                segments.Add(new int2(segs[i].PointAReference, segs[i].PointBReference), segs[i]);
                graphNodesList.Add(segs[i].PointAReference);
                graphNodesList.Add(segs[i].PointBReference);
            }

            int[] graphNodes = graphNodesList.Distinct().ToArray();

            GridPoint[] gps = FindObjectsOfType<GridPoint>();
            for (int i = 0; i < gps.Length; i++)
            {
                points.Add(gps[i].referenceInt, gps[i]);
            }

            graph = new List<int2>(segments.Keys).ToArray();
            DebugGraph(graph, "Logs/graph.log");
            print("searching for cycles");
            cycles = GraphUtils.FindAllCycles(graph, graphNodes);

            print("Cycle Debug : see Logs/cycles.log");
            DebugPolys(cycles, "Logs/cycles.log");
            polys = GraphUtils.FindAllPolygons(graph, cycles, ref points);
            print("Poly Debug : see Logs/polys.log");
            DebugPolys(polys, "Logs/polys.log");

            GeneratePolys(polys, ref points);

            //TODO this is running too early, the values have not been set.
            Polygon[] polyObjs = GameObject.FindObjectsOfType<Polygon>();
            print("Polygons: " + PolygonClassesToString(ListPolygonClasses(polyObjs)));
            print("Regular Polygons: " + PolygonClassesToString(ListRegularPolygonClasses(polyObjs)));
            print("Equilateral Polygons: " + PolygonClassesToString(ListEquilateralPolygonClasses(polyObjs)));
            print("Equiangular Polygons: " + PolygonClassesToString(ListEquiangularPolygonClasses(polyObjs)));
        }

        void DrawLine(LatticeGrid lg, Vector3Int a, Vector3Int b)
        {
            lg.StartLineDraw($"Line: {UnityEngine.Random.value.GetHashCode()}", lg.GetTargetGridPoint(a));
            lg.EndLineDraw(lg.GetTargetGridPoint(b));
        }

        private void DebugGraph(int2[] g, string path)
        {
            File.Delete(path);
            StreamWriter writer = new StreamWriter(path, true);

            for (int i = 0; i < g.Length; i++)
            {
                writer.WriteLine(g[i].x + " <==> " + g[i].y);
            }

            writer.Close();
        }

        private void DebugPolys(List<int[]> polys, string path)
        {
            File.Delete(path);
            StreamWriter writer = new StreamWriter(path, true);
            for (int i = 0; i < polys.Count; i++)
            {
                int[] poly = polys[i];
                string s = poly[0].ToString();
                for (int j = 1; j < poly.Length; j++)
                {
                    s += "," + poly[j];
                }

                writer.WriteLine(s);
            }

            writer.Close();
        }

        private void GeneratePolys(List<int[]> polys, ref Dictionary<int, GridPoint> points)
        {
            LatticeGrid lg = GetComponent<LatticeGrid>();
            for (int i = 0; i < polys.Count; i++)
            {
                int[] poly = polys[i];
                Vector3[] pos = new Vector3[poly.Length];
                for (int j = 0; j < polys[i].Length; j++)
                {
                    pos[j] = points[poly[j]].GetGridWorldPosition();
                }

                lg.GeneratePolygon(poly, pos);
            }
        }

        private static string PolygonClassesToString(int[] classes)
        {
            string s = "";
            foreach (int ngon in classes)
            {
                s += ngon + "-gon, ";
            }

            if (s == "")
            {
                s = "no polygon classes found meeting the criteria.";
            }

            return s;
        }

        private static int[] ListPolygonClasses(Polygon[] polys)
        {
            List<int> ngons = new List<int>();
            foreach (Polygon poly in polys)
            {
                ngons.Add(poly.nverts);
            }

            return ngons.Distinct().ToArray();
        }

        private static int[] ListRegularPolygonClasses(Polygon[] polys)
        {
            List<int> ngons = new List<int>();
            foreach (Polygon poly in polys)
            {
                if (poly.regular)
                {
                    ngons.Add(poly.nverts);
                }
            }

            return ngons.Distinct().ToArray();
        }

        private static int[] ListEquilateralPolygonClasses(Polygon[] polys)
        {
            List<int> ngons = new List<int>();
            foreach (Polygon poly in polys)
            {
                if (poly.equilateral)
                {
                    ngons.Add(poly.nverts);
                }
            }

            return ngons.Distinct().ToArray();
        }

        private static int[] ListEquiangularPolygonClasses(Polygon[] polys)
        {
            List<int> ngons = new List<int>();
            foreach (Polygon poly in polys)
            {
                if (poly.equiangular)
                {
                    ngons.Add(poly.nverts);
                }
            }

            return ngons.Distinct().ToArray();
        }
    }
}