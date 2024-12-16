using UnityEngine;

public class LoopRewardSystem : MonoBehaviour
{
    [SerializeField] private ResourceManager _resourceManager;
    
    private void OnEnable()
    {
        EventBus.Instance.OnLoopCountChanged += HandleLoopCount;
    }
    
    private void OnDisable()
    {
        EventBus.Instance.OnLoopCountChanged -= HandleLoopCount;
    }
    
    private void HandleLoopCount(int count)
    {
        // 每完成一次循环给予奖励
        _resourceManager.AddResource(ResourceType.Wood, 10);
        _resourceManager.AddResource(ResourceType.Stone, 10);
    }
} 