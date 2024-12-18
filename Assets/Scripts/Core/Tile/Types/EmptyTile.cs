using UnityEngine;
using System.Collections.Generic;

public class EmptyTile : BaseTile
{
    protected override void InitializeBuildCosts()
    {
        _type = TileType.Empty;
        _walkCost = 1;
        _buildCosts = new Dictionary<ResourceType, int>();
    }
    
    public override bool IsWalkable => false;
    
    public override void OnCharacterEnter(ICharacter character)
    {
        // 空地块不允许进入
        Debug.LogWarning("Attempted to enter an empty tile!");
    }
} 