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
        // 实现测试逻辑
    }

    [Test]
    public void TryConsumeResource_WithSufficientResources_ReturnsTrue()
    {
        // 实现测试逻辑
    }
} 