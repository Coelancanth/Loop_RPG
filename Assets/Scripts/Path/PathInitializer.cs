using UnityEngine;
using System.Collections.Generic;
using System;

namespace Game.Path
{
    public class PathInitializer
    {
        private readonly Dictionary<CharacterClass, IPathInitializationStrategy> _strategies;
        private readonly RandomManager _randomManager;
        private const int DEFAULT_PATH_LENGTH = 30;
        
        public PathInitializer(RandomManager randomManager)
        {
            _randomManager = randomManager ?? throw new ArgumentNullException(nameof(randomManager));
            _strategies = new Dictionary<CharacterClass, IPathInitializationStrategy>();
            InitializeStrategies();
        }
        
        private void InitializeStrategies()
        {
            // 添加默认策略
            var simpleStrategy = new SimplePathStrategy();
            _strategies[CharacterClass.Default] = simpleStrategy;
            
            // 添加职业特定策略
            _strategies[CharacterClass.Warrior] = new WarriorPathStrategy();
            //_strategies[CharacterClass.Mage] = new MagePathStrategy();
            //_strategies[CharacterClass.Rogue] = new RoguePathStrategy();
        }
        
        /// <summary>
        /// 根据角色职业和指定长度生成初始路径
        /// </summary>
        /// <param name="characterClass">角色职业</param>
        /// <param name="customLength">可选的自定义路径长度</param>
        /// <returns>路径点列表</returns>
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
                Debug.LogError($"Generated path for {characterClass} failed validation!");
                return _strategies[CharacterClass.Default].GeneratePath(length, _randomManager.CurrentSeed);
            }
            
            return path;
        }
        
        /// <summary>
        /// 获取指定职业的路径生成策略
        /// </summary>
        /// <param name="characterClass">角色职业</param>
        /// <returns>路径生成策略</returns>
        public IPathInitializationStrategy GetStrategy(CharacterClass characterClass)
        {
            return _strategies.GetValueOrDefault(characterClass) ?? _strategies[CharacterClass.Default];
        }
        
        /// <summary>
        /// 添加或更新职业特定的路径生成策略
        /// </summary>
        /// <param name="characterClass">角色职业</param>
        /// <param name="strategy">路径生成策略</param>
        public void SetStrategy(CharacterClass characterClass, IPathInitializationStrategy strategy)
        {
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));
                
            _strategies[characterClass] = strategy;
        }
    }
} 