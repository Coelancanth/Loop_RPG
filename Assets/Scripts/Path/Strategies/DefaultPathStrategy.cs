using UnityEngine;
using System.Collections.Generic;

namespace Game.Path
{
    public class DefaultPathStrategy : BasePathStrategy
    {
        public override string Description => "默认路径生成策略，生成一个简单的矩形路径";
        
        public override List<Vector2Int> GeneratePath(int length, int seed)
        {
            if (length < MinPathLength)
                throw new System.ArgumentException($"Path length must be at least {MinPathLength}");
                
            var random = CreateRandomFromSeed(seed);
            var path = new List<Vector2Int>();
            
            // 计算矩形的宽和高
            int width = Mathf.Max(2, length / 4);
            int height = Mathf.Max(2, (length - 2 * width) / 2);
            
            Vector2Int current = Vector2Int.zero;
            
            // 生成右侧边
            for (int i = 0; i < width; i++)
            {
                path.Add(current);
                current.x++;
            }
            
            // 生成上边
            for (int i = 0; i < height; i++)
            {
                path.Add(current);
                current.y++;
            }
            
            // 生成左侧边
            for (int i = 0; i < width; i++)
            {
                path.Add(current);
                current.x--;
            }
            
            // 生成下边
            for (int i = 0; i < height; i++)
            {
                path.Add(current);
                current.y--;
            }
            
            // 确保路径闭合
            if (path[0] != path[path.Count - 1])
            {
                path.Add(path[0]);
            }
            
            return path;
        }
    }
}