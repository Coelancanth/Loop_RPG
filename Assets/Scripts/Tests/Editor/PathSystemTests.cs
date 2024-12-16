using NUnit.Framework;
using UnityEngine;

public class PathSystemTests
{
    private PathSystem _pathSystem;
    private ResourceManager _resourceManager;

    [SetUp]
    public void Setup()
    {
        var go = new GameObject();
        _pathSystem = go.AddComponent<PathSystem>();
        _resourceManager = go.AddComponent<ResourceManager>();
    }

    [Test]
    public void PlaceTile_WithValidPosition_ReturnsTrue()
    {
        var position = new Vector2Int(0, 0);
        var tileObj = new GameObject().AddComponent<Tile>();
        
        bool result = _pathSystem.PlaceTile(position, tileObj.Type);
        
        Assert.IsTrue(result);
    }

    [Test]
    public void RemoveTile_WithExistingTile_ReturnsTrue()
    {
        var position = new Vector2Int(0, 0);
        var tileObj = new GameObject().AddComponent<Tile>();
        _pathSystem.PlaceTile(position, tileObj.Type);
        
        bool result = _pathSystem.RemoveTile(position);
        
        Assert.IsTrue(result);
    }
} 