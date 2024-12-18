using UnityEngine;
using System.Collections.Generic;

public class TileSystem : MonoBehaviour
{
    [Header("Prefab References")]
    [SerializeField] private GameObject _roadTilePrefab;
    [SerializeField] private GameObject _forestTilePrefab;
    [SerializeField] private GameObject _mountainTilePrefab;
    [SerializeField] private GameObject _emptyTilePrefab;
    
    [Header("Settings")]
    [SerializeField] private Transform _tilesParent;
    [SerializeField] private Vector2 _gridSize = new Vector2(1f, 1f);
    [SerializeField] private ResourceManager _resourceManager;
    
    private Dictionary<Vector2Int, BaseTile> _tiles = new();
    private Dictionary<TileType, GameObject> _prefabMap;
    
    private void Awake()
    {
        InitializePrefabMap();
        EnsureTilesParent();
    }
    
    private void InitializePrefabMap()
    {
        _prefabMap = new Dictionary<TileType, GameObject>
        {
            { TileType.Empty, _emptyTilePrefab },
            { TileType.Road, _roadTilePrefab },
            { TileType.Forest, _forestTilePrefab },
            { TileType.Mountain, _mountainTilePrefab }
        };
    }
    
    private void EnsureTilesParent()
    {
        if (_tilesParent == null)
        {
            var parent = new GameObject("Tiles");
            _tilesParent = parent.transform;
            _tilesParent.SetParent(transform);
        }
    }
    
    public bool PlaceTile(Vector2Int position, TileType type)
    {
        Debug.Log($"PlaceTile called - Position: {position}, Type: {type}");
        
        // 如果位置已经有Tile，先移除它
        if (_tiles.ContainsKey(position))
        {
            RemoveTile(position);
        }
        
        // 获取对应的预制体
        if (!_prefabMap.TryGetValue(type, out GameObject prefab))
        {
            Debug.LogError($"No prefab found for tile type {type}");
            return false;
        }
        
        // 创建Tile实例
        Vector3 worldPosition = GridToWorld(position);
        var tileObj = Instantiate(prefab, worldPosition, Quaternion.identity, _tilesParent);
        var tile = tileObj.GetComponent<BaseTile>();
        
        if (tile == null)
        {
            Debug.LogError($"Prefab for {type} does not have a BaseTile component!");
            Destroy(tileObj);
            return false;
        }
        
        // 设置Tile的位置属性
        tile.Position = position;
        
        // 尝试放置Tile
        if (tile.CanPlace(_resourceManager) && tile.OnPlaced(_resourceManager))
        {
            _tiles[position] = tile;
            EventBus.Instance.TriggerTilePlaced(position);
            Debug.Log($"Successfully placed {type} tile at {position}");
            return true;
        }
        
        Debug.LogWarning($"Failed to place {type} tile at {position} - CanPlace or OnPlaced returned false");
        Destroy(tileObj);
        return false;
    }
    
    public bool RemoveTile(Vector2Int position)
    {
        if (!_tiles.TryGetValue(position, out var tile))
            return false;
            
        tile.OnRemoved();
        Destroy(tile.gameObject);
        _tiles.Remove(position);
        EventBus.Instance.TriggerTileRemoved(position);
        return true;
    }
    
    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        return new Vector3(
            gridPosition.x * _gridSize.x,
            gridPosition.y * _gridSize.y,
            0
        );
    }
    
    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPosition.x / _gridSize.x),
            Mathf.RoundToInt(worldPosition.y / _gridSize.y)
        );
    }
    
    public void SetupReferences(ResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
    }
    
    private void OnValidate()
    {
        // 验证所有预制体都有正确的组件
        ValidatePrefab(_roadTilePrefab, typeof(RoadTile), "Road");
        ValidatePrefab(_forestTilePrefab, typeof(ForestTile), "Forest");
        ValidatePrefab(_mountainTilePrefab, typeof(MountainTile), "Mountain");
        ValidatePrefab(_emptyTilePrefab, typeof(EmptyTile), "Empty");
    }
    
    private void ValidatePrefab(GameObject prefab, System.Type expectedType, string typeName)
    {
        if (prefab == null)
        {
            Debug.LogError($"{typeName} tile prefab is missing!");
            return;
        }
        
        var tileComponent = prefab.GetComponent(expectedType);
        if (tileComponent == null)
        {
            Debug.LogError($"{typeName} tile prefab is missing {expectedType.Name} component!");
        }
        
        var spriteRenderer = prefab.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError($"{typeName} tile prefab is missing SpriteRenderer component!");
        }
    }
    
    // 在编辑器中绘制网格
    private void OnDrawGizmos()
    {
        DrawGrid();
    }
    
    private void DrawGrid()
    {
        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
        int gridSize = 10;
        
        for (int x = -gridSize; x <= gridSize; x++)
        {
            Vector3 start = new Vector3(x * _gridSize.x, -gridSize * _gridSize.y, 0);
            Vector3 end = new Vector3(x * _gridSize.x, gridSize * _gridSize.y, 0);
            Gizmos.DrawLine(start, end);
        }
        
        for (int y = -gridSize; y <= gridSize; y++)
        {
            Vector3 start = new Vector3(-gridSize * _gridSize.x, y * _gridSize.y, 0);
            Vector3 end = new Vector3(gridSize * _gridSize.x, y * _gridSize.y, 0);
            Gizmos.DrawLine(start, end);
        }
    }
}
