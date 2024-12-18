using UnityEngine;

public interface ITile
{
    TileType Type { get; }
    bool IsBuilt { get; }
    bool IsWalkable { get; }
    int WalkCost { get; }
    Vector2Int Position { get; }
    
    bool CanPlace(IResourceService resourceService);
    bool OnPlaced(IResourceService resourceService);
    void OnRemoved();
    
    // 特殊效果接口
    void OnCharacterEnter(ICharacter character);
    void OnCharacterExit(ICharacter character);
    void OnTurnStart();
    void OnTurnEnd();
} 