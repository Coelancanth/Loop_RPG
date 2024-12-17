using UnityEngine;
using UnityEngine.InputSystem;

public class TilePlacer : MonoBehaviour
{
    [SerializeField] private TileSystem _tileSystem;
    [SerializeField] private Camera _camera;
    [SerializeField] private TileType _currentTileType = TileType.Road;

    // 添加公共属性用于调试
    public Vector2Int? CurrentMouseGridPosition { get; private set; }
    public TileType CurrentTileType => _currentTileType;

    private void Awake()
    {
        if (_camera == null)
            _camera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        // 获取鼠标位置
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = _camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
        CurrentMouseGridPosition = _tileSystem.WorldToGrid(worldPosition);

        // 添加调试日志
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log($"Attempting to place {_currentTileType} at {CurrentMouseGridPosition}");
            bool success = _tileSystem.PlaceTile(CurrentMouseGridPosition.Value, _currentTileType);
            Debug.Log($"Placement success: {success}");
        }
        else if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Debug.Log($"Attempting to remove tile at {CurrentMouseGridPosition}");
            bool success = _tileSystem.RemoveTile(CurrentMouseGridPosition.Value);
            Debug.Log($"Remove success: {success}");
        }
    }

    // 用于在Inspector中切换当前要放置的Tile类型
    public void SetTileType(TileType type)
    {
        _currentTileType = type;
    }

    public void SetupReferences(TileSystem tileSystem, Camera camera)
    {
        _tileSystem = tileSystem;
        _camera = camera;
    }

    // 添加调试可视化
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || _tileSystem == null || _camera == null) return;

        // 显示当前网格位置
        Vector2 mousePosition = Mouse.current?.position.ReadValue() ?? Vector2.zero;
        Vector3 worldPosition = _camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
        Vector2Int gridPosition = _tileSystem.WorldToGrid(worldPosition);
        Vector3 gridWorldPos = _tileSystem.GridToWorld(gridPosition);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(gridWorldPos, Vector3.one);
    }
} 