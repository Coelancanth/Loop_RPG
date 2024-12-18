using UnityEngine;
using System.Collections.Generic;

public class RoadTile : BaseTile
{
    protected override void InitializeBuildCosts()
    {
        _type = TileType.Road;
        _walkCost = 1;
        _buildCosts = new Dictionary<ResourceType, int>
        {
            { ResourceType.Stone, 5 }
        };
    }
    
    public override bool IsWalkable => true;
    
    public override void OnCharacterEnter(ICharacter character)
    {
        // 基础道路无特殊效果
        base.OnCharacterEnter(character);
    }
} 