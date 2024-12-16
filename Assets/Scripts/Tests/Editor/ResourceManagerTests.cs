using NUnit.Framework;
using UnityEngine;

public class ResourceManagerTests
{
    private ResourceManager _resourceManager;

    [SetUp]
    public void Setup()
    {
        var go = new GameObject();
        _resourceManager = go.AddComponent<ResourceManager>();
    }

    [Test]
    public void AddResource_WithinMaxStorage_IncreasesResourceCount()
    {
        int initialAmount = 100;
        _resourceManager.AddResource(ResourceType.Wood, initialAmount);
        
        Assert.AreEqual(initialAmount, _resourceManager.GetResourceCount(ResourceType.Wood));
    }

    [Test]
    public void TryConsumeResource_WithSufficientResources_ReturnsTrue()
    {
        int initialAmount = 100;
        int consumeAmount = 50;
        
        _resourceManager.AddResource(ResourceType.Stone, initialAmount);
        bool result = _resourceManager.TryConsumeResource(ResourceType.Stone, consumeAmount);
        
        Assert.IsTrue(result);
        Assert.AreEqual(initialAmount - consumeAmount, _resourceManager.GetResourceCount(ResourceType.Stone));
    }
} 