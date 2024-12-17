using UnityEngine;
using System.Collections.Generic;

namespace Game.Path
{
    public interface IPathInitializationStrategy
    {
        /// <summary>
        /// Generates a closed loop path based on the given length and seed
        /// </summary>
        /// <param name="length">The desired length of the path</param>
        /// <param name="seed">Random seed for reproducible generation</param>
        /// <returns>A list of Vector2Int positions representing the path</returns>
        List<Vector2Int> GeneratePath(int length, int seed);
        
        /// <summary>
        /// Validates if the generated path is a valid closed loop
        /// </summary>
        /// <param name="path">The path to validate</param>
        /// <returns>True if the path is valid, false otherwise</returns>
        bool ValidatePath(List<Vector2Int> path);
    }
} 