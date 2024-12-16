using UnityEngine;

[RequireComponent(typeof(Tile))]
public class TileBehaviour : MonoBehaviour
{
    private Tile _tile;
    private SpriteRenderer _spriteRenderer;
    
    [Header("Visual Settings")]
    [SerializeField] private TileVisualConfig _visualConfig;
    [SerializeField] private Sprite _defaultSprite; // 默认sprite，如果没有配置文件
    
    private void Awake()
    {
        _tile = GetComponent<Tile>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (_spriteRenderer == null)
        {
            _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        UpdateVisual();
    }
    
    private void OnEnable()
    {
        // 订阅Tile类型改变事件（如果有的话）
        if (_tile != null)
        {
            _tile.OnTypeChanged += UpdateVisual;
        }
    }
    
    private void OnDisable()
    {
        if (_tile != null)
        {
            _tile.OnTypeChanged -= UpdateVisual;
        }
    }

    public void UpdateVisual()
    {
        if (_spriteRenderer == null) return;

        if (_visualConfig != null)
        {
            // 使用配置文件的设置
            _spriteRenderer.sprite = _visualConfig.GetSprite(_tile.Type);
            _spriteRenderer.color = _visualConfig.GetColor(_tile.Type);
        }
        else
        {
            // 使用默认设置
            _spriteRenderer.sprite = _defaultSprite;
            _spriteRenderer.color = TileColors.GetColorForType(_tile.Type);
        }

        // 确保Tile在正确的排序层
        _spriteRenderer.sortingOrder = 0;
    }

#if UNITY_EDITOR
    // 编辑器方法，用于预览和设置类型
    public void SetType(TileType type)
    {
        if (_tile == null) _tile = GetComponent<Tile>();
        
        var serializedObject = new UnityEditor.SerializedObject(_tile);
        var typeProperty = serializedObject.FindProperty("_type");
        typeProperty.enumValueIndex = (int)type;
        serializedObject.ApplyModifiedProperties();
        
        UpdateVisual();
    }

    private void OnValidate()
    {
        // 确保在编辑器中修改属性时更新视觉效果
        if (_tile == null) _tile = GetComponent<Tile>();
        if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisual();
    }
#endif
} 