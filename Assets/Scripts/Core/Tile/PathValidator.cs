using UnityEngine;

public class PathValidator : MonoBehaviour
{
    [SerializeField] private LoopCounter _loopCounter;
    private TileSystem _tileSystem;

    private void Awake()
    {
        _tileSystem = GetComponent<TileSystem>();
    }

    private void OnEnable()
    {
        EventBus.Instance.OnTilePlaced += ValidatePathOnTilePlaced;
    }

    private void OnDisable()
    {
        EventBus.Instance.OnTilePlaced -= ValidatePathOnTilePlaced;
    }

    private void ValidatePathOnTilePlaced(Vector2Int position)
    {
        if (ValidatePath())
        {
            _loopCounter.Increment();
            Debug.Log("Valid path formed - Loop counter incremented");
        }
    }

    private bool ValidatePath()
    {
        // 实现路径验证逻辑
        // 例如：检查是否从起点连接到终点
        return true; // 临时返回值
    }
} 