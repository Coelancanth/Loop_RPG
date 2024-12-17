using UnityEngine;
using System.Collections.Generic;

public class TileSystem : MonoBehaviour
{
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private GameObject[] _tilePrefabs; // 每种类型对应的预制体
    [SerializeField] private Transform _tilesParent; // 用于组织层级
    [SerializeField] private Vector2 _gridSize = new Vector2(1f, 1f); // 网格大小
    
    private Dictionary<Vector2Int, Tile> _tiles = new();

    // 添加公共属性用于调试
    public int TilePrefabsCount => _tilePrefabs?.Length ?? 0;

    private void Awake()
    {
        if (_tilesParent == null)
        {
            var parent = new GameObject("Tiles");
            _tilesParent = parent.transform;
            _tilesParent.SetParent(transform);
        }
    }

    public GameObject GetTilePrefab(TileType type)
    {
        return _tilePrefabs[(int)type];
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

    public bool PlaceTile(Vector2Int position, TileType type)
    {
        Debug.Log($"PlaceTile called - Position: {position}, Type: {type}");
        
        if (_tiles.ContainsKey(position))
        {
            Debug.LogWarning($"Tile already exists at position {position}");
            return false;
        }

        var prefab = GetTilePrefab(type);
        if (prefab == null)
        {
            Debug.LogError($"No prefab found for tile type {type}. Prefabs array length: {TilePrefabsCount}");
            return false;
        }

        Debug.Log($"Creating tile at world position: {GridToWorld(position)}");
        Vector3 worldPosition = GridToWorld(position);
        var tileObj = Instantiate(prefab, worldPosition, Quaternion.identity, _tilesParent);
        var tile = tileObj.GetComponent<Tile>();

        if (tile == null)
        {
            Debug.LogError("Created object does not have Tile component!");
            Destroy(tileObj);
            return false;
        }

        if (tile.CanPlace(_resourceManager) && tile.OnPlaced(_resourceManager))
        {
            _tiles[position] = tile;
            EventBus.Instance.TriggerTilePlaced(position);
            Debug.Log($"Successfully placed tile at {position}");
            return true;
        }
        else
        {
            Debug.LogWarning($"Failed to place tile at {position} - CanPlace or OnPlaced returned false");
            Destroy(tileObj);
            return false;
        }
    }

    public bool RemoveTile(Vector2Int position)
    {
        if (!_tiles.TryGetValue(position, out var tile))
            return false;

        Destroy(tile.gameObject);
        _tiles.Remove(position);
        EventBus.Instance.TriggerTileRemoved(position);
        return true;
    }

    public void SetupReferences(ResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
    }

    // 添加调试方法
    private void OnValidate()
    {
        // 确保预制体数组大小与TileType枚举匹配
        if (_tilePrefabs == null || _tilePrefabs.Length != System.Enum.GetValues(typeof(TileType)).Length)
        {
            Debug.LogWarning("Tile预制体数组大小与TileType枚举不匹配！");
        }
    }

    // 修改网格绘制方法，使其在编辑器和运行时都可见
    private void OnDrawGizmos()
    {
        DrawGrid();
    }

    // 添加这个方法使网格在游戏中也可见
    private void OnDrawGizmosSelected()
    {
        DrawGrid();
    }

    private void DrawGrid()
    {
        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
        
        // 固定绘制范围，确保在编辑器中始终可见
        int gridSize = 10; // 显示 20x20 的网格
        
        // 绘制垂直线
        for (int x = -gridSize; x <= gridSize; x++)
        {
            Vector3 start = new Vector3(x * _gridSize.x, -gridSize * _gridSize.y, 0);
            Vector3 end = new Vector3(x * _gridSize.x, gridSize * _gridSize.y, 0);
            Gizmos.DrawLine(start, end);
        }

        // 绘制水平线
        for (int y = -gridSize; y <= gridSize; y++)
        {
            Vector3 start = new Vector3(-gridSize * _gridSize.x, y * _gridSize.y, 0);
            Vector3 end = new Vector3(gridSize * _gridSize.x, y * _gridSize.y, 0);
            Gizmos.DrawLine(start, end);
        }
    }
} 