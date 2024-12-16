using System;
using UnityEngine;

public class EventBus
{
    // 单例模式
    private static EventBus _instance;
    public static EventBus Instance => _instance ??= new EventBus();

    // 事件定义
    public event Action<int> OnLoopCountChanged;
    public event Action OnBossTriggered;
    public event Action<ResourceType, int> OnResourceChanged;
    public event Action<Vector2Int> OnTilePlaced;
    public event Action<Vector2Int> OnTileRemoved;

    // 事件触发方法
    public void TriggerLoopCountChanged(int newCount) => OnLoopCountChanged?.Invoke(newCount);
    public void TriggerBossEvent() => OnBossTriggered?.Invoke();
    public void TriggerResourceChanged(ResourceType type, int amount) => OnResourceChanged?.Invoke(type, amount);
    public void TriggerTilePlaced(Vector2Int position) => OnTilePlaced?.Invoke(position);
    public void TriggerTileRemoved(Vector2Int position) => OnTileRemoved?.Invoke(position);
} 