using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Game.Path
{
    public class DefaultPathStrategy : IPathInitializationStrategy
    {
        private const int MIN_DISTANCE_TO_CLOSE = 3; // Minimum distance to consider closing the loop
        private const int MAX_ATTEMPTS = 100; // Maximum attempts to generate a valid path

        public List<Vector2Int> GeneratePath(int length, int seed)
        {
            var random = new System.Random(seed);
            var path = new List<Vector2Int>();
            var currentPos = Vector2Int.zero;
            path.Add(currentPos);

            for (int attempt = 0; attempt < MAX_ATTEMPTS; attempt++)
            {
                path.Clear();
                path.Add(currentPos);

                // Generate path points
                for (int i = 1; i < length - 1; i++)
                {
                    var nextPos = GenerateNextPosition(currentPos, path, random);
                    if (nextPos == null)
                    {
                        break; // Path generation failed, try again
                    }

                    path.Add(nextPos.Value);
                    currentPos = nextPos.Value;
                }

                // Try to close the loop
                if (TryClosePath(path, random))
                {
                    return path;
                }
            }

            throw new InvalidOperationException("Failed to generate a valid closed path after maximum attempts");
        }

        public bool ValidatePath(List<Vector2Int> path)
        {
            if (path == null || path.Count < 4)
                return false;

            // Check if path is closed (last point connects to first)
            if (!ArePointsAdjacent(path.Last(), path.First()))
                return false;

            // Check if all points are connected
            for (int i = 0; i < path.Count - 1; i++)
            {
                if (!ArePointsAdjacent(path[i], path[i + 1]))
                    return false;
            }

            // Check for duplicates
            return path.Distinct().Count() == path.Count - 1; // Last point can be same as first
        }

        private Vector2Int? GenerateNextPosition(Vector2Int current, List<Vector2Int> existingPath, System.Random random)
        {
            var possibleMoves = new List<Vector2Int>
            {
                new Vector2Int(1, 0),   // Right
                new Vector2Int(-1, 0),  // Left
                new Vector2Int(0, 1),   // Up
                new Vector2Int(0, -1)   // Down
            };

            // Shuffle possible moves
            possibleMoves = possibleMoves.OrderBy(x => random.Next()).ToList();

            foreach (var move in possibleMoves)
            {
                var newPos = current + move;
                if (!existingPath.Contains(newPos))
                {
                    return newPos;
                }
            }

            return null;
        }

        private bool TryClosePath(List<Vector2Int> path, System.Random random)
        {
            if (path.Count < 3)
                return false;

            var start = path.First();
            var current = path.Last();

            // Check if we can close the path
            if (ArePointsAdjacent(current, start))
            {
                path.Add(start);
                return true;
            }

            // Try to find a path back to start
            var visited = new HashSet<Vector2Int>(path);
            var pathToStart = FindPathToStart(current, start, visited, random);
            
            if (pathToStart != null)
            {
                path.AddRange(pathToStart);
                return true;
            }

            return false;
        }

        private List<Vector2Int> FindPathToStart(Vector2Int current, Vector2Int start, HashSet<Vector2Int> visited, System.Random random)
        {
            if (ArePointsAdjacent(current, start))
            {
                return new List<Vector2Int> { start };
            }

            var possibleMoves = new List<Vector2Int>
            {
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, -1)
            }.OrderBy(x => random.Next()).ToList();

            foreach (var move in possibleMoves)
            {
                var next = current + move;
                if (!visited.Contains(next))
                {
                    visited.Add(next);
                    var pathToStart = FindPathToStart(next, start, visited, random);
                    if (pathToStart != null)
                    {
                        pathToStart.Insert(0, next);
                        return pathToStart;
                    }
                }
            }

            return null;
        }

        private bool ArePointsAdjacent(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) == 1;
        }
    }
} 