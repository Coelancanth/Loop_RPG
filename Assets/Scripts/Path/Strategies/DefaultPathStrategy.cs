using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Game.Path
{
    public class DefaultPathStrategy : IPathInitializationStrategy
    {
        private const int MAX_MUTATION_ATTEMPTS = 50;
        private const float MUTATION_INTENSITY = 0.3f;

        public List<Vector2Int> GeneratePath(int length, int seed)
        {
            var random = new System.Random(seed);
            UnityEngine.Debug.Log($"Generating rectangle-based path with length {length} and seed {seed}");

            // Calculate optimal rectangle size
            var (width, height) = CalculateRectangleSize(length);
            UnityEngine.Debug.Log($"Calculated rectangle size: {width}x{height}");
            //width = 4;
            //height = 4;
            // Generate base rectangle path
            var basePath = GenerateRectanglePath(width, height);
            UnityEngine.Debug.Log($"Generated base rectangle path with {basePath.Count} points");

            // Apply mutations
            var mutatedPath = ApplyMutations(basePath, random);
            //if (ValidatePath(mutatedPath))
            //{
                //UnityEngine.Debug.Log("Successfully generated and validated mutated path");
                //return basePath;
            //}

            UnityEngine.Debug.LogWarning("Mutation failed, returning base rectangle path");
            return mutatedPath;
        }

        private (int width, int height) CalculateRectangleSize(int targetLength)
        {
            // Remove 1 from target length because the last point is same as first
            targetLength = targetLength - 1;
            
            // Find the most square-like rectangle that gives us a perimeter close to targetLength
            int bestWidth = 2;
            int bestHeight = 2;
            int bestDiff = int.MaxValue;

            for (int w = 2; w <= targetLength / 2; w++)
            {
                int h = (targetLength - 2 * w) / 2;
                if (h < 2) continue;

                int currentPerimeter = 2 * (w + h);
                int diff = Math.Abs(currentPerimeter - targetLength);

                if (diff < bestDiff)
                {
                    bestDiff = diff;
                    bestWidth = w;
                    bestHeight = h;
                }
            }

            return (bestWidth, bestHeight);
        }

        private List<Vector2Int> GenerateRectanglePath(int width, int height)
        {
            var path = new List<Vector2Int>();
            var current = Vector2Int.zero;

            // Bottom edge
            for (int x = 0; x < width; x++)
            {
                path.Add(current);
                current.x++;
            }

            // Right edge
            for (int y = 0; y < height; y++)
            {
                path.Add(current);
                current.y++;
            }

            // Top edge
            for (int x = 0; x < width; x++)
            {
                path.Add(current);
                current.x--;
            }

            // Left edge
            for (int y = 0; y < height; y++)
            {
                path.Add(current);
                current.y--;
            }

            // Close the loop by adding the starting point
            path.Add(Vector2Int.zero);

            return path;
        }

        private List<Vector2Int> ApplyMutations(List<Vector2Int> basePath, System.Random random)
        {
            var mutatedPath = new List<Vector2Int>(basePath);
            int successfulMutations = 0;
            int attempts = 0;

            while (successfulMutations < MAX_MUTATION_ATTEMPTS && attempts < MAX_MUTATION_ATTEMPTS * 2)
            {
                attempts++;
                if (TryMutate(mutatedPath, random))
                {
                    successfulMutations++;
                }
            }

            UnityEngine.Debug.Log($"Applied {successfulMutations} mutations in {attempts} attempts");
            return mutatedPath;
        }

        private bool TryMutate(List<Vector2Int> path, System.Random random)
        {
            // Select a random point (excluding first/last point)
            int index = random.Next(1, path.Count - 1);
            var point = path[index];
            
            // Calculate possible mutation directions
            var directions = GetPossibleMutationDirections(path, index, random);
            if (directions.Count == 0) return false;

            // Try each direction
            foreach (var dir in directions)
            {
                var newPoint = point + dir;
                
                // Create a temporary path with the mutation
                var tempPath = new List<Vector2Int>(path);
                tempPath[index] = newPoint;

                // Validate the mutation
                if (ValidateMutation(tempPath, index))
                {
                    // Apply the mutation
                    path[index] = newPoint;
                    return true;
                }
            }

            return false;
        }

        private List<Vector2Int> GetPossibleMutationDirections(List<Vector2Int> path, int index, System.Random random)
        {
            var directions = new List<Vector2Int>
            {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.left,
                Vector2Int.right
            };

            // Shuffle directions
            directions = directions.OrderBy(x => random.Next()).ToList();

            // Filter out directions that would break path connectivity
            return directions.Where(dir => 
            {
                var newPos = path[index] + dir;
                return ArePointsAdjacent(newPos, path[index - 1]) && 
                       ArePointsAdjacent(newPos, path[index + 1]);
            }).ToList();
        }

        private bool ValidateMutation(List<Vector2Int> path, int mutatedIndex)
        {
            // Check if the mutated point maintains connectivity
            if (!ArePointsAdjacent(path[mutatedIndex], path[mutatedIndex - 1]) ||
                !ArePointsAdjacent(path[mutatedIndex], path[mutatedIndex + 1]))
                return false;

            // Check for intersections with other parts of the path
            var mutatedPoint = path[mutatedIndex];
            for (int i = 0; i < path.Count - 1; i++)
            {
                if (i == mutatedIndex || i == mutatedIndex - 1) continue;
                if (path[i] == mutatedPoint) return false;
            }

            return true;
        }

        public bool ValidatePath(List<Vector2Int> path)
        {
            if (path == null || path.Count < 4)
            {
                UnityEngine.Debug.LogWarning("Path validation failed: path is null or too short");
                return false;
            }

            // Check if path is closed (last point connects to first)
            if (!ArePointsAdjacent(path.Last(), path.First()))
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

            // Check for duplicates (except last point which should equal first point)
            var distinctPoints = new HashSet<Vector2Int>(path.Take(path.Count - 1));
            if (distinctPoints.Count != path.Count - 1)
            {
                UnityEngine.Debug.LogWarning("Path validation failed: path contains duplicate points");
                return false;
            }

            return true;
        }

        private bool ArePointsAdjacent(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) == 1;
        }
    }
}