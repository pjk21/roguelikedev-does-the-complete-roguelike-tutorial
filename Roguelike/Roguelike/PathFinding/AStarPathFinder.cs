using Priority_Queue;
using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike.PathFinding
{
    public class AStarPathFinder
    {
        public static Queue<Point> GetPath(Point start, Point destination)
        {
            var openSet = new SimplePriorityQueue<Point>();
            openSet.Enqueue(start, 0);

            var closedSet = new HashSet<Point>();

            var cameFrom = new Dictionary<Point, Point>();

            var costSoFar = new Dictionary<Point, float>();
            costSoFar[start] = 0;

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();

                if (current == destination)
                {
                    return ConstructPath(cameFrom, current);
                }

                closedSet.Add(current);

                foreach (var neighbour in GetNeighbours(current))
                {
                    if (closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    float cost = costSoFar[current] + 1f;

                    if (!costSoFar.ContainsKey(neighbour) || cost < costSoFar[neighbour])
                    {
                        costSoFar[neighbour] = cost;
                        openSet.Enqueue(neighbour, cost + Distance(current.X, current.X, neighbour.X, neighbour.Y));
                        cameFrom[neighbour] = current;
                    }
                }
            }

            return ConstructPath(cameFrom, destination);
        }

        private static float Distance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        private static IEnumerable<Point> GetNeighbours(Point current)
        {
            if (current.X > 0 && Program.Game.Map.IsWalkable(current.X - 1, current.Y))
            {
                yield return new Point(current.X - 1, current.Y);
            }

            if (current.X < Program.Game.Map.Width - 1 && Program.Game.Map.IsWalkable(current.X + 1, current.Y))
            {
                yield return new Point(current.X + 1, current.Y);
            }

            if (current.Y > 0 && Program.Game.Map.IsWalkable(current.X, current.Y - 1))
            {
                yield return new Point(current.X, current.Y - 1);
            }

            if (current.Y < Program.Game.Map.Height - 1 && Program.Game.Map.IsWalkable(current.X, current.Y + 1))
            {
                yield return new Point(current.X, current.Y + 1);
            }
        }

        private static Queue<Point> ConstructPath(Dictionary<Point, Point> cameFrom, Point current)
        {
            var path = new Queue<Point>();
            path.Enqueue(current);

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Enqueue(current);
            }

            return new Queue<Point>(path.Reverse());
        }
    }
}
