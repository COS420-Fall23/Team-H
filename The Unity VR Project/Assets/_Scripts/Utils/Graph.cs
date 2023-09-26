using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

// cycle detection based on solution at https://stackoverflow.com/questions/12367801/finding-all-cycles-in-undirected-graphs
namespace LatticeLand.Utils
{
    public static class GraphUtils
    {
        public static List<int[]> FindAllPolygons(int2[] graph, List<int[]> cycles,
            ref Dictionary<int, GridPoint> points)
        {
            List<int[]> polygonCycles = new List<int[]>();
            for (int i = 0; i < cycles.Count; i++)
            {
                int[] cycle = cycles[i];
                float3[] pointArray = new float3[cycles[i].Length];
                for (int j = 0; j < cycle.Length; j++)
                {
                    pointArray[j] = points[cycle[j]].GetGridWorldPosition();
                }

                //if the points are not coplanar, it does not form a polygon.
                if (!CoplanarPoints(pointArray)) continue;
                //otherwise add it to the polygon list
                polygonCycles.Add(cycles[i]);
            }

            return polygonCycles;
        }

        public static List<int[]> FindAllRegularPolygons(int2[] graph, List<int[]> cycles,
            LineSegment[] segments)
        {
            List<int[]> regularPolygonCycles = new List<int[]>();
            for (int i = 0; i < cycles.Count; i++)
            {
                float3[] pointArray = new float3[cycles[i].Length];
                for (int j = 0; j < cycles[i].Length; j++)
                {
                    pointArray[j] =
                        segments[j]
                            .PosA; //we get the second position as the start of the next segment (since they are ordered).
                }

                //if the points are not coplanar, it does not form a regular polygon.
                if (!RegularPolygonSegmentCycle(pointArray)) continue;
                //otherwise add it to the regular polygon list
                regularPolygonCycles.Add(cycles[i]);
            }

            return regularPolygonCycles;
        }

        public static List<int[]> FindAllCycles(int2[] graph, int[] points)
        {
			List<int[]> cycles = new List<int[]>();
            for (int i = 0; i < points.Length; i++)
            {
				int[] path = new int[1];
				path[0] = points[i];
                FindNewCycles(ref graph, path, ref cycles);
            }

            return cycles;
        }

        private static void FindNewCycles(ref int2[] graph, int[] path, ref List<int[]> cycles)
        {
            int currentNode = path[path.Length - 1];
            int nextNode = new int();
            int[] subpath = new int[path.Length + 1];
            
            for (int i = 0; i < graph.Length; i++)
            {
				if(graph[i].x == currentNode || graph[i].y == currentNode){
                	if (graph[i].x == currentNode) nextNode = graph[i].y;
                	else if (graph[i].y == currentNode) nextNode = graph[i].x;
	
	                // if the candidate node is already in the path, see if it completes the cycle, or skip it.
	                if (VisitedInPath(nextNode, path))
	                {
		                //check if the cycle is complete, if not skip this edge.
        	            if (path[0] == nextNode){
            	    	    //otherwise, check if the cycle is in fact a new find!
            		        int[] path_candidate = RotateCycleToSmallest(path.ToArray());
							//prefer better ordering
							if(path_candidate[1] > path_candidate[^1]){
								path_candidate = InverseCycle(path.ToArray());
							}
    	    	            if (path_candidate.Length > 2 && IsNew(cycles, path_candidate))
    		                {
	        	        		cycles.Add(path_candidate);
                	    	}
						}else{
							nextNode = new int();
						}
            	    }
                	else
	                {
    	                //if not already int he path, add it to subpath
						subpath[^1] = nextNode;
            	        Array.Copy(path, 0, subpath, 0, path.Length);
                	    FindNewCycles(ref graph, subpath, ref cycles);
                	}
				}
            }

            return;
        }

        private static bool VisitedInPath(int node, int[] path)
        {
            return path.ToList().Contains(node);
        }

        private static bool IsNew(List<int[]> cycles, int[] path)
        {
			if(cycles.Count == 0) return true;

            int[] inverse = InverseCycle(path);
            for (int i = 0; i < cycles.Count; i++)
            {
                if(IsEqual(cycles[i], path)) return false;
                if (IsEqual(cycles[i], inverse)) return false;
            }
            return true;
        }

		private static bool IsEqual(int[] path1, int[] path2){
			if(path1.Length != path2.Length)
				return false;
			for(int i = 0; i < path1.Length; i ++){
				// need to check values not references.
				if(path1[i].CompareTo(path2[i]) != 0) return false;
			}
			return true;
		}

		private static int SmallestIdx(int[] cycle){
			List<int> cycleList = cycle.ToList(); 
			int min = cycleList.Min();
			return cycleList.FindIndex(x => x == min);
		}

        private static int[] RotateCycleToSmallest(int[] cycle)
        {
            int smallIdx = SmallestIdx(cycle);
	        int[] newCycle = new int[cycle.Length];

            if (smallIdx != 0){
	            // if rotation is necessary, 
	            int n;
	
    	        Array.Copy(cycle, 0, newCycle, 0, cycle.Length);

				for(int i = 0; i < smallIdx; i++){
	                n = newCycle[0];
	                Array.Copy(newCycle, 1, newCycle, 0, newCycle.Length - 1);
	                newCycle[newCycle.Length - 1] = n;
	            }
			}else{
				newCycle = cycle;
			}
            return newCycle;
        }

        private static int[] InverseCycle(int[] cycle)
        {
            return RotateCycleToSmallest(cycle.Reverse().ToArray());
        }

        public static bool RegularPolygonSegmentCycle(float3[] pointArray)
        {
            return pointArray.Length >= 3 &&
                   CoplanarPoints(pointArray) &&
                   EquilateralSegmentCycle(pointArray) &&
                   EquiangularSegmentCycle(pointArray);
        }

        public static bool EquiangularSegmentCycle(float3[] pointArray)
        {
            int length = pointArray.Length;
            if (length < 3) return true; //there is only one angle

            // find the angle for the first pair
            float angle = InteriorAngle(pointArray[length - 1] - pointArray[0], pointArray[0] - pointArray[1]);
            for (int i = 0; i < length; i++)
            {
                if (!Mathf.Approximately(angle,
                        InteriorAngle(pointArray[(i - 1) % length] - pointArray[i],
                            pointArray[i] - pointArray[(i + 1) % length])))
                    return false;
            }

            return true;
        }

        public static bool EquilateralSegmentCycle(float3[] pointArray)
        {
            //assumes that segments are arranged in the order of the nodes in the point array positions.
            int length = pointArray.Length;
            if (length < 3) return true; // two points are equidistant.

            float distsq = math.distancesq(pointArray[0], pointArray[1]); //find the distance for the first pair
            for (int i = 2; i < length; i++)
            {
                // if one segment fails the equidistant test, return false
                if (!Mathf.Approximately(distsq, math.distance(pointArray[i], pointArray[i - 1]))) return false;
            }

            //if all segments pass, return true
            return true;
        }

        public static bool CoplanarPoints(float3[] pointArray)
        {
            int length = pointArray.Length;
            if (length < 4) return true; //any collection of 0, 1, 2, or 3 points are coplanar.
            // if four or more points, define the plane.
            float3 position = pointArray[0];
            float3 normal = math.normalize(math.cross(pointArray[0] - pointArray[1], pointArray[0] - pointArray[2]));
            for (int i = 3; i < length; i++)
            {
                // if one point fails the coplanar test, return false
                if (!CoplanarPoint(pointArray[i], position, normal)) return false;
            }

            // if all points pass, return true
            return true;
        }

        private static bool CoplanarPoint(float3 point, float3 planePosition, float3 planeNormal)
        {
            return Mathf.Approximately(math.dot(point - planePosition, planeNormal), 0f);
        }

        private static float InteriorAngle(float3 direction1, float3 direction2)
        {
            //assume that the two directions are coplanar and meet at a point
            //by taking the absolute value of the dot product, we find the interior angle.
            return Mathf.Acos(Mathf.Abs(math.dot(math.normalize(direction1), math.normalize(direction2))));
        }
    }
}