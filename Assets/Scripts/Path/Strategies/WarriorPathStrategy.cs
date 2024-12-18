using UnityEngine;
using System.Collections.Generic;

namespace Game.Path
{
    public class WarriorPathStrategy : BasePathStrategy
    {
        public override string Description => "生成一个适合战士的紧凑型路径，倾向于生成更多战斗点";
        
        public override List<Vector2Int> GeneratePath(int length, int seed)
        {
            if (length < MinPathLength)
                throw new System.ArgumentException($"Path length must be at least {MinPathLength}");
                
            var random = CreateRandomFromSeed(seed);
            var path = new List<Vector2Int>();
            
            // 战士路径特点：
            // 1. 更紧凑的布局，便于防御
            // 2. 倾向于生成更多的战斗点
            
            int size = Mathf.CeilToInt(Mathf.Sqrt(length));
            Vector2Int current = Vector2Int.zero;
            
            // 生成螺旋形状的路径
            int direction = 0; // 0: 右, 1: 上, 2: 左, 3: 下
            int steps = size;
            int stepCount = 0;
            
            while (path.Count < length)
            {
                path.Add(current);
                
                if (stepCount >= steps)
                {
                    direction = (direction + 1) % 4;
                    if (direction % 2 == 0)
                        steps--;
                    stepCount = 0;
                }
                
                switch (direction)
                {
                    case 0: current.x++; break;
                    case 1: current.y++; break;
                    case 2: current.x--; break;
                    case 3: current.y--; break;
                }
                
                stepCount++;
            }
            
            // 确保路径闭合
            if (path[0] != path[path.Count - 1])
            {
                // 找到最短的返回路径
                var returnPath = FindReturnPath(path[path.Count - 1], path[0]);
                if (returnPath != null)
                {
                    path.AddRange(returnPath);
                }
            }
            
            return path;
        }
        
        private List<Vector2Int> FindReturnPath(Vector2Int start, Vector2Int end)
        {
            var path = new List<Vector2Int>();
            var current = start;
            
            while (current != end)
            {
                path.Add(current);
                
                if (current.x != end.x)
                    current.x += (current.x < end.x) ? 1 : -1;
                else if (current.y != end.y)
                    current.y += (current.y < end.y) ? 1 : -1;
            }
            
            path.Add(end);
            return path;
        }
    }
} 