using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private PathSystem _pathSystem;
    [SerializeField] private LoopCounter _loopCounter;
    [SerializeField] private TilePlacer _tilePlacer;
    
    private void Start()
    {
        // 初始化资源
        _resourceManager.AddResource(ResourceType.Wood, 100);
        _resourceManager.AddResource(ResourceType.Stone, 100);
        _resourceManager.AddResource(ResourceType.Gold, 50);
        _resourceManager.AddResource(ResourceType.Food, 200);
        
        // 订阅事件
        EventBus.Instance.OnBossTriggered += HandleBossTriggered;
    }
    
    private void HandleBossTriggered()
    {
        Debug.Log("Boss战触发！");
    }
} 