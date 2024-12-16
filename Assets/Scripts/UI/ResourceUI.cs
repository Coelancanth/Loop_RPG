using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _woodText;
    [SerializeField] private TextMeshProUGUI _stoneText;
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _foodText;
    
    private ResourceManager _resourceManager;

    private void Start()
    {
        _resourceManager = FindObjectOfType<ResourceManager>();
        if (_resourceManager != null)
        {
            // 更新所有资源的初始值
            UpdateAllResourceTexts();
        }
    }
    
    private void OnEnable()
    {
        EventBus.Instance.OnResourceChanged += HandleResourceChanged;
    }
    
    private void OnDisable()
    {
        EventBus.Instance.OnResourceChanged -= HandleResourceChanged;
    }
    
    private void HandleResourceChanged(ResourceType type, int amount)
    {
        UpdateResourceText(type, _resourceManager.GetResourceCount(type));
    }

    private void UpdateAllResourceTexts()
    {
        UpdateResourceText(ResourceType.Wood, _resourceManager.GetResourceCount(ResourceType.Wood));
        UpdateResourceText(ResourceType.Stone, _resourceManager.GetResourceCount(ResourceType.Stone));
        UpdateResourceText(ResourceType.Gold, _resourceManager.GetResourceCount(ResourceType.Gold));
        UpdateResourceText(ResourceType.Food, _resourceManager.GetResourceCount(ResourceType.Food));
    }

    private void UpdateResourceText(ResourceType type, int value)
    {
        var text = type switch
        {
            ResourceType.Wood => _woodText,
            ResourceType.Stone => _stoneText,
            ResourceType.Gold => _goldText,
            ResourceType.Food => _foodText,
            _ => null
        };
        
        if (text != null)
        {
            text.text = $"{type}: {value}";
        }
    }
} 