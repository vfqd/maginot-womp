using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Library.Grid
{
    public static class GridDrawingAlgorithms
    {
        public static List<T> GetFilledCircle<T>(this Grid<T> grid, T origin, float radius) where T : ITile, new()
        {
            List<T> res = new List<T>();
            for (float y = -radius; y <= radius; ++y)
            {
                for (float x = -radius; x <= radius; ++x)
                {
                    if(x*x+y*y < radius*radius + radius)
                    {
                        if (grid.GetTile(origin.X+x, origin.Y+y, out T tile)) res.Add(tile);
                    }
                }
            }
            return res;
        }
        
        public static List<T> GetEmptyCircle<T>(this Grid<T> grid, ITile origin, int radius) where T : ITile, new()
        {
            int d = (5 - radius * 4) / 4;
            int x = 0;
            int y = radius;
 
            List<T> res = new List<T>();
            do
            {
                if (grid.GetTile(origin.X + x, origin.Y + y, out T t1)) res.Add(t1);
                if (grid.GetTile(origin.X + x, origin.Y - y, out T t2)) res.Add(t2);
                if (grid.GetTile(origin.X - x, origin.Y + y, out T t3)) res.Add(t3);
                if (grid.GetTile(origin.X - x, origin.Y - y, out T t4)) res.Add(t4);
                if (grid.GetTile(origin.X + y, origin.Y + x, out T t5)) res.Add(t5);
                if (grid.GetTile(origin.X + y, origin.Y - x, out T t6)) res.Add(t6);
                if (grid.GetTile(origin.X - y, origin.Y + x, out T t7)) res.Add(t7);
                if (grid.GetTile(origin.X - y, origin.Y - x, out T t8)) res.Add(t8);
                if (d < 0)
                {
                    d += 2 * x + 1;
                }
                else
                {
                    d += 2 * (x - y) + 1;
                    y--;
                }
                x++;
            } while (x <= y);

            return res;
        }
        
        
        public static List<T> GetClusterAt<T>(this Grid<T> grid, T origin, float falloff, int minRadius, int maxRadius) where T : ITile, new()
        {
            var cluster = new List<T>();
            var queue = new Queue<(T cell, float randomVal, float distanceElapsed)>();
            queue.Enqueue((cell: origin, randomVal: 1, distanceElapsed: 0));
            while (queue.Count > 0)
            {
                var tuple = queue.Dequeue();
                var c = tuple.cell;
                if (tuple.distanceElapsed >= maxRadius) continue;

                if (Random.value < tuple.randomVal)
                {
                    if (!cluster.Contains(c)) cluster.Add(c);
                    if (grid.GetTile(c.X,c.Y+1,out T u) && !cluster.Contains(u)) queue.Enqueue((u,tuple.randomVal/falloff,tuple.distanceElapsed+1));
                    if (grid.GetTile(c.X,c.Y-1,out T d) && !cluster.Contains(d)) queue.Enqueue((d,tuple.randomVal/falloff,tuple.distanceElapsed+1));
                    if (grid.GetTile(c.X-1,c.Y,out T l) && !cluster.Contains(l)) queue.Enqueue((l,tuple.randomVal/falloff,tuple.distanceElapsed+1));
                    if (grid.GetTile(c.X+1,c.Y,out T r) && !cluster.Contains(r)) queue.Enqueue((r,tuple.randomVal/falloff,tuple.distanceElapsed+1));
                }
            }
            
            foreach (var min in GetFilledCircle(grid,origin,minRadius))
            {
                if (!cluster.Contains(min)) cluster.Add(min);
            }
            
            return cluster;
        }

        public static T GetRandomPointInDonut<T>(this Grid<T> grid, T origin, float r1, float r2) where T : ITile, new()
        {
            var t = 2 * Mathf.PI * Random.value;
            var u = Random.value + Random.value;
            var r = u > 1 ? 2 - u : u;
            r = r < r2 ? r2+r*((r1-r2)/r2) : r;
            return grid.GetTile(
                origin.X + r * Mathf.Cos(t),
               origin.Y + r * Mathf.Sin(t)
            );
        }

        public static List<T> GetRandomNPointsWithinRect<T>(this Grid<T> grid, T bottomLeft, int w, int h, int n) where T : ITile, new()
        {
            List<T> points = GetRect(grid, bottomLeft, w, h, n);
            points.Shuffle();
            return points.Take(n).ToList();
        }
        
        public static List<T> GetRect<T>(this Grid<T> grid, T bottomLeft, int w, int h, int n) where T : ITile, new()
        {
            List<T> points = new List<T>();
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    int x = bottomLeft.X + i;
                    int y = bottomLeft.Y + j;
                    if (grid.GetTile(x,y, out T tile)) points.Add(tile);
                }
            }
            return points;
        }
        
        public static T GetPointOnCircle<T>(this Grid<T> grid, T origin, float radius, float angle) where T : ITile, new()
        {
            float x = Mathf.Cos(angle*Mathf.Deg2Rad)*radius;
            float y = Mathf.Sin(angle*Mathf.Deg2Rad)*radius;
            return grid.GetTile(origin.X+x,origin.Y+y);
        }
        
        public static bool IsPointInsideCircle<T>(T point, T center, float radius) where T : ITile, new() 
        {
            // (x - center_x)² + (y - center_y)² < radius²
            return Mathf.Pow(point.X - center.X, 2) + Mathf.Pow(point.Y - center.Y, 2) < Mathf.Pow(radius, 2);
        }
        
        public static List<T> GetLine<T>(this Grid<T> grid, T start, T end) where T : ITile, new()
        {
            List<T> resultCoordinates = new List<T> ();
            int x = start.X;
            int y = start.Y;
            int x2 = end.X;
            int y2 = end.Y;

            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;

            if (w < 0)
                dx1 = -1;
            else if (w > 0)
                dx1 = 1;
		
            if (h < 0)
                dy1 = -1;
            else if (h > 0)
                dy1 = 1;
		
            if (w < 0)
                dx2 = -1;
            else if (w > 0)
                dx2 = 1;
		
            int longest = Mathf.Abs (w);
            int shortest = Mathf.Abs (h);
            if (!(longest > shortest)) {
                longest = Mathf.Abs (h);
                shortest = Mathf.Abs (w);
                if (h < 0)
                    dy2 = -1;
                else if (h > 0)
                    dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++) {
                if (grid.GetTile(x, y, out T tile)) resultCoordinates.Add(tile);
                numerator += shortest;
                if (!(numerator < longest)) {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                } else {
                    x += dx2;
                    y += dy2;
                }
            }

            return resultCoordinates;
        }
        
        public static bool DoLinesIntersect<T>(T p1, T q1, T p2, T q2) where T : ITile, new()
        {
            // Find the four orientations needed for general and
            // special cases
            int o1 = Orientation(p1, q1, p2);
            int o2 = Orientation(p1, q1, q2);
            int o3 = Orientation(p2, q2, p1);
            int o4 = Orientation(p2, q2, q1);
            
            int Orientation(T p, T q, T r)
            {
                // See https://www.geeksforgeeks.org/orientation-3-ordered-points/
                // for details of below formula.
                int val = (q.Y - p.Y) * (r.X - q.X) -
                          (q.X - p.X) * (r.Y - q.Y);
 
                if (val == 0) return 0; // collinear
 
                return (val > 0)? 1: 2; // clock or counterclock wise
            }
 
            // General case
            if (o1 != o2 && o3 != o4)
                return true;
 
            // Special Cases
            // p1, q1 and p2 are collinear and p2 lies on segment p1q1
            if (o1 == 0 && OnSegment(p1, p2, q1)) return true;
 
            // p1, q1 and q2 are collinear and q2 lies on segment p1q1
            if (o2 == 0 && OnSegment(p1, q2, q1)) return true;
 
            // p2, q2 and p1 are collinear and p1 lies on segment p2q2
            if (o3 == 0 && OnSegment(p2, p1, q2)) return true;
 
            // p2, q2 and q1 are collinear and q1 lies on segment p2q2
            if (o4 == 0 && OnSegment(p2, q1, q2)) return true;
 
            // Given three collinear points p, q, r, the function checks if
            // point q lies on line segment 'pr'
            bool OnSegment(T p, T q, T r)
            {
                return q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                       q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y);
            }
            
            return false; // Doesn't fall in any of the above cases
        }
        
        public static List<T> GetFloodFillArea<T>(this Grid<T> grid, T origin, Func<T, bool> isValid) where T : ITile, new()
        {
            var floodArea = new List<T>();
            var queue = new Queue<T>();
            queue.Enqueue(origin);
            while (queue.Count > 0)
            {
                var c = queue.Dequeue();
                if (isValid.Invoke(c) == false) continue;
                if (floodArea.Contains(c)) continue;
                floodArea.Add(c);
                
                if (grid.GetTile(c.X,c.Y+1,out T u)) queue.Enqueue(u);
                if (grid.GetTile(c.X,c.Y-1,out T d)) queue.Enqueue(d);
                if (grid.GetTile(c.X-1,c.Y,out T l)) queue.Enqueue(l);
                if (grid.GetTile(c.X+1,c.Y,out T r)) queue.Enqueue(r);
            }

            return floodArea;
        }
        
        public static List<T> GetFilledEllipse<T>(this Grid<T> grid, T origin, float minorRadius, float majorRadius) where T : ITile, new() 
        {
            var result = new List<T>();
            float dx, dy, d1, d2, x, y;
            x = 0;
            y = majorRadius;
         
            // Initial decision parameter of region 1
            d1 = (majorRadius * majorRadius) - (minorRadius * minorRadius * majorRadius) +
                            (0.25f * minorRadius * minorRadius);
            dx = 2 * majorRadius * majorRadius * x;
            dy = 2 * minorRadius * minorRadius * y;
             
            // For region 1
            while (dx < dy)
            {
                // Fill in spans 
                var span = new List<T>();
                for (int i = Mathf.RoundToInt(-x)+origin.X; i <= Mathf.RoundToInt(x)+origin.X; i++)
                {
                    span.Add(grid.GetTile(i,Mathf.RoundToInt(y)+origin.Y));
                }
                result.AddRange(span);
                span = new List<T>();
                for (int i = Mathf.RoundToInt(-x)+origin.X; i <= Mathf.RoundToInt(x)+origin.X; i++)
                {
                    span.Add(grid.GetTile(i,Mathf.RoundToInt(-y)+origin.Y));
                }
                result.AddRange(span);


                // Checking and updating value of
                // decision parameter based on algorithm
                if (d1 < 0)
                {
                    x++;
                    dx += (2 * majorRadius * majorRadius);
                    d1 = d1 + dx + (majorRadius * majorRadius);
                }
                else
                {
                    x++;
                    y--;
                    dx += (2 * majorRadius * majorRadius);
                    dy -= (2 * minorRadius * minorRadius);
                    d1 = d1 + dx - dy + (majorRadius * majorRadius);
                }
            }
         
            // Decision parameter of region 2
            d2 = ((majorRadius * majorRadius) * ((x + 0.5f) * (x + 0.5f)))
                + ((minorRadius * minorRadius) * ((y - 1) * (y - 1)))
                - (minorRadius * minorRadius * majorRadius * majorRadius);
         
            // Plotting points of region 2
            while (y >= 0)
            {
                // Fill in spans 
                var span = new List<T>();
                for (int i = Mathf.RoundToInt(-x)+origin.X; i <= Mathf.RoundToInt(x)+origin.X; i++)
                {
                    span.Add(grid.GetTile(i,Mathf.RoundToInt(y)+origin.Y));
                }
                result.AddRange(span);
                span = new List<T>();
                for (int i = Mathf.RoundToInt(-x)+origin.X; i <= Mathf.RoundToInt(x)+origin.X; i++)
                {
                    span.Add(grid.GetTile(i,Mathf.RoundToInt(-y)+origin.Y));
                }
                result.AddRange(span);
         
                // Checking and updating parameter
                // value based on algorithm
                if (d2 > 0)
                {
                    y--;
                    dy -= (2 * minorRadius * minorRadius);
                    d2 = d2 + (minorRadius * minorRadius) - dy;
                }
                else
                {
                    y--;
                    x++;
                    dx += (2 * majorRadius * majorRadius);
                    dy -= (2 * minorRadius * minorRadius);
                    d2 = d2 + dx - dy + (minorRadius * minorRadius);
                }
            }
            return result;
        }
        
        public static bool IsPointInTriangle<T>(T p, T v0, T v1, T v2) where T : ITile, new()
        {
            var s = v0.Y * v2.X - v0.X * v2.Y + (v2.Y - v0.Y) * p.X + (v0.X - v2.X) * p.Y;
            var t = v0.X * v1.Y - v0.Y * v1.X + (v0.Y - v1.Y) * p.X + (v1.X - v0.X) * p.Y;

            if ((s < 0) != (t < 0))
                return false;

            var a = -v1.Y * v2.X + v0.Y * (v2.X - v1.X) + v0.X * (v1.Y - v2.Y) + v1.X * v2.Y;

            return a < 0 ?
                (s <= 0 && s + t >= a) :
                (s >= 0 && s + t <= a);
        }
        
        public static List<T> GetFilledCircleSector<T>(this Grid<T> grid, T origin, int radius, float startAngle, float endAngle) where T : ITile, new()
        {
            startAngle = NormaliseAngle(startAngle);
            endAngle = NormaliseAngle(endAngle);
            var angleDiff = NormaliseAngle(endAngle - startAngle);
            if (startAngle > endAngle)
                startAngle -= 2 * Mathf.PI;

            List<T> res = new List<T>();
            res.Add(origin);
            for (int y = -radius; y <= radius; ++y)
            {
                for (int x = -radius; x <= radius; ++x)
                {
                    var tileAngle = NormaliseAngle(Mathf.Atan2(y, x));
                    var tileAngleFromStart = NormaliseAngle(tileAngle - startAngle);
                    if (x * x + y * y < radius * radius + radius && 
                        (tileAngle >= startAngle || Mathf.Approximately(tileAngle, startAngle)) && 
                        (tileAngleFromStart <= angleDiff || Mathf.Approximately(tileAngleFromStart, angleDiff)))
                    {
                        if (grid.GetTile(origin.X + x, origin.Y + y, out T tile)) res.Add(tile);
                    }
                }
            }
            
            float NormaliseAngle(float angle)
            {
                return ((angle % (2 * Mathf.PI)) + (2 * Mathf.PI)) % (2 * Mathf.PI);
            }

            return res;
        }
        
        // Uses the 'winding number' algorithm
        public static bool IsPointInsidePolygon<T>(this Grid<T> grid, T p, List<T> points) where T : ITile, new()
        {
            // Polygons require at least 3 points
            Assert.IsTrue(points.Count >= 3);

            // This needs to be a point guaranteed to be outside of the polygon
            var endLineTest = grid.GetTile(0, 0);
            
            // Get number of intersections
            int intersectCount = 0;
            for (int i = 1; i < points.Count; i++)
            {
                if (DoLinesIntersect(points[i - 1], points[i], p, endLineTest))
                {
                    intersectCount++;
                } 
            }
            // Close the shape
            if (DoLinesIntersect(points[^1], points[0], p, endLineTest))
            {
                intersectCount++;
            } 
            
            return intersectCount % 2 != 0;
        }
    }
}