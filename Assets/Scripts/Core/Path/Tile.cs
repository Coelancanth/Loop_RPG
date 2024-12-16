using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileType _type = TileType.Empty;
    [SerializeField] private bool _isBuilt;
    
    private Dictionary<TileType, Dictionary<ResourceType, int>> _buildCosts = new()
    {
        { TileType.Road, new Dictionary<ResourceType, int> { { ResourceType.Stone, 5 } } },
        { TileType.Forest, new Dictionary<ResourceType, int> { { ResourceType.Wood, 10 } } }
    };

    public TileType Type => _type;
    public bool IsBuilt => _isBuilt;

    public bool CanPlace(IResourceService resourceService)
    {
        if (_isBuilt) return false;
        if (!_buildCosts.TryGetValue(_type, out var costs)) return true;

        foreach (var cost in costs)
        {
            if (resourceService.GetResourceCount(cost.Key) < cost.Value)
                return false;
        }
        return true;
    }

    public bool OnPlaced(IResourceService resourceService)
    {
        if (!CanPlace(resourceService)) return false;
        
        if (_buildCosts.TryGetValue(_type, out var costs))
        {
            foreach (var cost in costs)
            {
                if (!resourceService.TryConsumeResource(cost.Key, cost.Value))
                    return false;
            }
        }

        _isBuilt = true;
        return true;
    }
} 