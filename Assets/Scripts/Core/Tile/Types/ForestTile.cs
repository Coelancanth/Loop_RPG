using UnityEngine;
using System.Collections.Generic;

public class ForestTile : BaseTile
{
    [SerializeField] private float _evasionBonus = 0.2f; // 20% 闪避加成
    
    protected override void InitializeBuildCosts()
    {
        _type = TileType.Forest;
        _walkCost = 2; // 森林移动消耗更高
        _buildCosts = new Dictionary<ResourceType, int>
        {
            { ResourceType.Wood, 10 }
        };
    }
    
    public override bool IsWalkable => true;
    
    public override void OnCharacterEnter(ICharacter character)
    {
        base.OnCharacterEnter(character);
        if (character != null)
        {
            // 增加角色闪避率
            character.AddEvasionBonus(_evasionBonus);
        }
    }
    
    public override void OnCharacterExit(ICharacter character)
    {
        base.OnCharacterExit(character);
        if (character != null)
        {
            // 移除角色闪避率加成
            character.RemoveEvasionBonus(_evasionBonus);
        }
    }
    
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        // 每回合开始时产��少量木材
        var resourceManager = FindObjectOfType<ResourceManager>();
        if (resourceManager != null)
        {
            resourceManager.AddResource(ResourceType.Wood, 1);
        }
    }
} 