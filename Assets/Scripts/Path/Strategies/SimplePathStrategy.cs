using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Game.Path
{
    public class SimplePathStrategy : IPathInitializationStrategy
    {
        private const int DEFAULT_WIDTH = 4;
        private const int DEFAULT_HEIGHT = 4;

        public List<Vector2Int> GeneratePath(int length, int seed)
        {
            // For simple strategy, we ignore the length parameter and use fixed dimensions
            return GenerateRectanglePath(DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        private List<Vector2Int> GenerateRectanglePath(int width, int height)
        {
            var path = new List<Vector2Int>();
            var current = Vector2Int.zero;

            // Bottom edge (left to right)
            for (int x = 0; x < width; x++)
            {
                path.Add(current);
                current.x++;
            }

            // Right edge (bottom to top)
            for (int y = 0; y < height; y++)
            {
                path.Add(current);
                current.y++;
            }

            // Top edge (right to left)
            for (int x = 0; x < width; x++)
            {
                path.Add(current);
                current.x--;
            }

            // Left edge (top to bottom)
            for (int y = 0; y < height; y++)
            {
                path.Add(current);
                current.y--;
            }

            // Close the loop by adding the starting point
            path.Add(Vector2Int.zero);

            UnityEngine.Debug.Log($"Generated simple rectangular path with dimensions {width}x{height}, total points: {path.Count}");
            return path;
        }

        public bool ValidatePath(List<Vector2Int> path)
        {
            if (path == null || path.Count < 4)
            {
                UnityEngine.Debug.LogWarning("Path validation failed: path is null or too short");
                return false;
            }

            // Check if path is closed (last point connects to first)
            if (path.Last() != path.First())
            {
                UnityEngine.Debug.LogWarning("Path validation failed: path is not closed");
                return false;
            }

            // Check if all points are connected
            for (int i = 0; i < path.Count - 1; i++)
            {
                if (!ArePointsAdjacent(path[i], path[i + 1]))
                {
                    UnityEngine.Debug.LogWarning($"Path validation failed: points at indices {i} and {i+1} are not adjacent");
                    return false;
                }
            }

            return true;
        }

        private bool ArePointsAdjacent(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) == 1;
        }
    }
} 