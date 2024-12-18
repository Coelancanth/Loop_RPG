using UnityEngine;
using System.Collections.Generic;

public class MountainTile : BaseTile
{
    [SerializeField] private float _defenseBonus = 0.3f; // 30% 防御加成
    
    protected override void InitializeBuildCosts()
    {
        _type = TileType.Mountain;
        _walkCost = 3; // 山地移动消耗最高
        _buildCosts = new Dictionary<ResourceType, int>
        {
            { ResourceType.Stone, 15 }
        };
    }
    
    public override bool IsWalkable => true;
    
    public override void OnCharacterEnter(ICharacter character)
    {
        base.OnCharacterEnter(character);
        if (character != null)
        {
            // 增加角色防御
            character.AddDefenseBonus(_defenseBonus);
        }
    }
    
    public override void OnCharacterExit(ICharacter character)
    {
        base.OnCharacterExit(character);
        if (character != null)
        {
            // 移除角色防御加成
            character.RemoveDefenseBonus(_defenseBonus);
        }
    }
    
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        // 每回合开始时产生少量石材
        var resourceManager = FindObjectOfType<ResourceManager>();
        if (resourceManager != null)
        {
            resourceManager.AddResource(ResourceType.Stone, 1);
        }
    }
} 