using UnityEngine;
using System.Collections.Generic;

public abstract class BaseTile : MonoBehaviour, ITile
{
    [SerializeField] protected TileType _type = TileType.Empty;
    [SerializeField] protected bool _isBuilt;
    [SerializeField] protected int _walkCost = 1;
    
    protected Vector2Int _position;
    protected Dictionary<ResourceType, int> _buildCosts;
    
    public event System.Action OnTypeChanged;
    
    public TileType Type => _type;
    public bool IsBuilt => _isBuilt;
    public virtual bool IsWalkable => _type != TileType.Empty;
    public virtual int WalkCost => _walkCost;
    public Vector2Int Position 
    { 
        get => _position;
        set => _position = value;
    }
    
    protected virtual void Awake()
    {
        InitializeBuildCosts();
    }
    
    protected abstract void InitializeBuildCosts();
    
    public virtual bool CanPlace(IResourceService resourceService)
    {
        if (_isBuilt) return false;
        if (_type == TileType.Empty) return false;
        if (_buildCosts == null) return true;
        
        foreach (var cost in _buildCosts)
        {
            if (resourceService.GetResourceCount(cost.Key) < cost.Value)
                return false;
        }
        return true;
    }
    
    public virtual bool OnPlaced(IResourceService resourceService)
    {
        if (!CanPlace(resourceService)) return false;
        
        if (_buildCosts != null)
        {
            foreach (var cost in _buildCosts)
            {
                if (!resourceService.TryConsumeResource(cost.Key, cost.Value))
                    return false;
            }
        }
        
        _isBuilt = true;
        return true;
    }
    
    public virtual void OnRemoved()
    {
        _isBuilt = false;
        _type = TileType.Empty;
        NotifyTypeChanged();
    }
    
    public virtual void OnCharacterEnter(ICharacter character) 
    {
        if (!IsWalkable)
        {
            Debug.LogWarning($"Attempted to enter an unwalkable tile of type {_type}");
        }
    }
    
    public virtual void OnCharacterExit(ICharacter character) { }
    
    public virtual void OnTurnStart() { }
    
    public virtual void OnTurnEnd() { }
    
    protected void NotifyTypeChanged()
    {
        OnTypeChanged?.Invoke();
    }
} 