using UnityEngine;
using System.Collections.Generic;

namespace Game.Path
{
    public interface IPathInitializationStrategy
    {
        /// <summary>
        /// 生成初始路径
        /// </summary>
        /// <param name="length">路径长度</param>
        /// <param name="seed">随机种子</param>
        /// <returns>路径点列表</returns>
        List<Vector2Int> GeneratePath(int length, int seed);
        
        /// <summary>
        /// 获取该策略支持的最小路径长度
        /// </summary>
        int MinPathLength { get; }
        
        /// <summary>
        /// 获取该策略支持的最大路径长度
        /// </summary>
        int MaxPathLength { get; }
        
        /// <summary>
        /// 获取该策略的描述
        /// </summary>
        string Description { get; }
        
        /// <summary>
        /// 验证路径是否有效
        /// </summary>
        /// <param name="path">要验证的路径</param>
        /// <returns>路径是否有效</returns>
        bool ValidatePath(List<Vector2Int> path);
    }
} 