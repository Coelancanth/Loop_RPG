using UnityEngine;
using System.Collections.Generic;

namespace Game.Path
{
    public abstract class BasePathStrategy : IPathInitializationStrategy
    {
        protected const int DEFAULT_MIN_LENGTH = 4;
        protected const int DEFAULT_MAX_LENGTH = 100;
        
        public virtual int MinPathLength => DEFAULT_MIN_LENGTH;
        public virtual int MaxPathLength => DEFAULT_MAX_LENGTH;
        public abstract string Description { get; }
        
        public abstract List<Vector2Int> GeneratePath(int length, int seed);
        
        public virtual bool ValidatePath(List<Vector2Int> path)
        {
            if (path == null || path.Count < MinPathLength)
                return false;
                
            // 检查路径是否形成闭环
            return IsPathClosed(path) && IsPathContinuous(path);
        }
        
        protected bool IsPathClosed(List<Vector2Int> path)
        {
            if (path.Count < 2) return false;
            return path[0] == path[path.Count - 1];
        }
        
        protected bool IsPathContinuous(List<Vector2Int> path)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector2Int current = path[i];
                Vector2Int next = path[i + 1];
                
                // 检查相邻点是否相连（曼哈顿距离为1）
                if (Mathf.Abs(current.x - next.x) + Mathf.Abs(current.y - next.y) != 1)
                    return false;
            }
            return true;
        }
        
        protected System.Random CreateRandomFromSeed(int seed)
        {
            return new System.Random(seed);
        }
    }
} 