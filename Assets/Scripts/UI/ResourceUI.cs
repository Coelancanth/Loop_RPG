using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _woodText;
    [SerializeField] private TextMeshProUGUI _stoneText;
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _foodText;
    [SerializeField] private ResourceManager _resourceManager;
    
    private bool _isInitialized = false;
    
    private void Awake()
    {
        ValidateReferences();
    }
    
    private void OnEnable()
    {
        if (_isInitialized)
        {
            SubscribeToEvents();
        }
    }
    
    private void Start()
    {
        if (!_isInitialized)
        {
            ValidateReferences();
        }
        UpdateAllResourceTexts();
    }
    
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    
    private void ValidateReferences()
    {
        if (_resourceManager == null)
        {
            _resourceManager = FindObjectOfType<ResourceManager>();
        }
        
        if (_resourceManager != null)
        {
            _isInitialized = true;
            SubscribeToEvents();
            UpdateAllResourceTexts();
        }
        else
        {
            Debug.LogError("ResourceManager reference is missing in ResourceUI!");
        }
    }
    
    private void SubscribeToEvents()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.OnResourceChanged += HandleResourceChanged;
        }
    }
    
    private void UnsubscribeFromEvents()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.OnResourceChanged -= HandleResourceChanged;
        }
    }
    
    private void HandleResourceChanged(ResourceType type, int amount)
    {
        if (_resourceManager != null)
        {
            UpdateResourceText(type, _resourceManager.GetResourceCount(type));
        }
    }
    
    private void UpdateAllResourceTexts()
    {
        if (_resourceManager == null) return;
        
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