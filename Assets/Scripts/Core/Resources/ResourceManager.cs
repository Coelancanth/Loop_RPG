using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour, IResourceService
{
    private Dictionary<ResourceType, int> _resources = new();
    private Dictionary<ResourceType, int> _maxStorage = new();

    private void Awake()
    {
        InitializeResources();
    }

    private void InitializeResources()
    {
        foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
        {
            _resources[type] = 0;
            _maxStorage[type] = 1000; // 默认上限，可通过配置文件调整
        }
    }

    public int GetResourceCount(ResourceType type)
    {
        return _resources.TryGetValue(type, out int count) ? count : 0;
    }

    public bool TryConsumeResource(ResourceType type, int amount)
    {
        if (amount < 0) return false;
        if (GetResourceCount(type) < amount) return false;

        _resources[type] -= amount;
        EventBus.Instance.TriggerResourceChanged(type, -amount);
        return true;
    }

    public void AddResource(ResourceType type, int amount)
    {
        if (amount < 0) return;

        int newAmount = Mathf.Min(GetResourceCount(type) + amount, GetMaxStorage(type));
        _resources[type] = newAmount;
        EventBus.Instance.TriggerResourceChanged(type, amount);
    }

    public int GetMaxStorage(ResourceType type)
    {
        return _maxStorage.TryGetValue(type, out int max) ? max : 0;
    }
} 