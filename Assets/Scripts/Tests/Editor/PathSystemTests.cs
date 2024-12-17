using NUnit.Framework;
using UnityEngine;

public class PathSystemTests
{
    private TileSystem _tileSystem;
    private ResourceManager _resourceManager;

    [SetUp]
    public void Setup()
    {
        var go = new GameObject();
        _tileSystem = go.AddComponent<TileSystem>();
        _resourceManager = go.AddComponent<ResourceManager>();
    }

    [Test]
    public void PlaceTile_WithValidPosition_ReturnsTrue()
    {
        var position = new Vector2Int(0, 0);
        var tileObj = new GameObject().AddComponent<Tile>();
        
        bool result = _tileSystem.PlaceTile(position, tileObj.Type);
        
        Assert.IsTrue(result);
    }

    [Test]
    public void RemoveTile_WithExistingTile_ReturnsTrue()
    {
        var position = new Vector2Int(0, 0);
        var tileObj = new GameObject().AddComponent<Tile>();
        _tileSystem.PlaceTile(position, tileObj.Type);
        
        bool result = _tileSystem.RemoveTile(position);
        
        Assert.IsTrue(result);
    }
} 