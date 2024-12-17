using UnityEngine;
using System.Collections.Generic;
using System;

namespace Game.Path
{
    public class PathInitializer
    {
        private readonly Dictionary<CharacterClass, IPathInitializationStrategy> _strategies;
        private readonly RandomManager _randomManager;
        private const int DEFAULT_PATH_LENGTH = 10;

        public PathInitializer(RandomManager randomManager)
        {
            _randomManager = randomManager ?? throw new ArgumentNullException(nameof(randomManager));
            _strategies = new Dictionary<CharacterClass, IPathInitializationStrategy>();
            InitializeStrategies();
        }

        private void InitializeStrategies()
        {
            // Add simple strategy as the default
            var simpleStrategy = new SimplePathStrategy();
            _strategies[CharacterClass.Default] = simpleStrategy;
            
            // Add character-specific strategies
            _strategies[CharacterClass.Warrior] = new DefaultPathStrategy(); // More complex strategy for specific classes
            _strategies[CharacterClass.Mage] = new DefaultPathStrategy();
            _strategies[CharacterClass.Rogue] = new DefaultPathStrategy();
        }

        /// <summary>
        /// Generates an initial path based on character class and optional custom length
        /// </summary>
        /// <param name="characterClass">The character class to generate the path for</param>
        /// <param name="customLength">Optional custom path length. If null, uses default length</param>
        /// <returns>A list of Vector2Int positions representing the path</returns>
        public List<Vector2Int> GenerateInitialPath(CharacterClass characterClass, int? customLength = null)
        {
            var length = customLength ?? DEFAULT_PATH_LENGTH;
            
            if (length < 4)
            {
                throw new ArgumentException("Path length must be at least 4 to form a valid loop", nameof(customLength));
            }

            var strategy = _strategies.GetValueOrDefault(characterClass) ?? _strategies[CharacterClass.Default];
            var path = strategy.GeneratePath(length, _randomManager.CurrentSeed);

            if (!strategy.ValidatePath(path))
            {
                UnityEngine.Debug.LogWarning("Generated path validation failed, falling back to simple strategy");
                path = _strategies[CharacterClass.Default].GeneratePath(length, _randomManager.CurrentSeed);
            }

            return path;
        }
    }
} 