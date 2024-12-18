using UnityEngine;
using Game.Path;
using System.Collections.Generic;

public class GameInitializer : MonoBehaviour
{
    [Header("Path Generation")]
    [SerializeField] private int _initialPathLength = 30;
    [SerializeField] private int _randomSeed = 42;
    
    [Header("References")]
    [SerializeField] private TileSystem _tileSystem;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private ResourceUI _resourceUI;
    
    private PathInitializer _pathInitializer;
    private List<Vector2Int> _initialPath;
    
    private void Awake()
    {
        // 确保有随机数管理器
        RandomManager.Instance.Initialize(_randomSeed);
        
        // 初始化路径生成器
        _pathInitializer = new PathInitializer(RandomManager.Instance);
        
        // 验证必要的引用
        ValidateReferences();
    }
    
    private void Start()
    {
        // 确保所有必要组件都已初始化
        if (!ValidateReferences())
        {
            Debug.LogError("Cannot start game initialization due to missing references!");
            return;
        }
        
        // 生成初始路径
        GenerateInitialPath();
        
        // 在路径上放置基础道路
        PlaceInitialRoads();
    }
    
    private bool ValidateReferences()
    {
        bool isValid = true;
        
        if (_tileSystem == null)
        {
            _tileSystem = FindObjectOfType<TileSystem>();
            if (_tileSystem == null)
            {
                Debug.LogError("TileSystem reference is missing!");
                isValid = false;
            }
        }
        
        if (_resourceManager == null)
        {
            _resourceManager = FindObjectOfType<ResourceManager>();
            if (_resourceManager == null)
            {
                Debug.LogError("ResourceManager reference is missing!");
                isValid = false;
            }
        }
        
        if (_resourceUI == null)
        {
            _resourceUI = FindObjectOfType<ResourceUI>();
            if (_resourceUI == null)
            {
                Debug.LogWarning("ResourceUI reference is missing - UI updates may not work correctly");
            }
        }
        
        return isValid;
    }
    
    private void GenerateInitialPath()
    {
        try
        {
            // 使用SimplePathStrategy生成初始路径
            _initialPath = _pathInitializer.GenerateInitialPath(CharacterClass.Default, _initialPathLength);
            Debug.Log($"Successfully generated initial path with {_initialPath.Count} points");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to generate initial path: {e.Message}");
        }
    }
    
    private void PlaceInitialRoads()
    {
        if (_initialPath == null || _initialPath.Count == 0)
        {
            Debug.LogError("No initial path to place roads on!");
            return;
        }
        
        // 确保ResourceManager已经完全初始化
        if (_resourceManager == null)
        {
            Debug.LogError("Cannot place initial roads: ResourceManager is not initialized!");
            return;
        }
        
        try
        {
            // 给予足够的初始资源来建造道路
            _resourceManager.AddResource(ResourceType.Stone, _initialPath.Count * 5); // 每个道路需要5个石头
            
            // 在路径上放置道路
            foreach (var position in _initialPath)
            {
                if (!_tileSystem.PlaceTile(position, TileType.Road))
                {
                    Debug.LogWarning($"Failed to place road at position {position}");
                }
            }
            
            Debug.Log("Initial roads placed successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error during initial road placement: {e.Message}");
        }
    }
    
    // 提供公共访问方法以获取初始路径
    public List<Vector2Int> GetInitialPath()
    {
        return new List<Vector2Int>(_initialPath ?? new List<Vector2Int>());
    }
    
    // 在Unity编辑器中可视化初始路径
    private void OnDrawGizmos()
    {
        if (_initialPath == null || _initialPath.Count == 0) return;
        
        Gizmos.color = Color.yellow;
        for (int i = 0; i < _initialPath.Count - 1; i++)
        {
            Vector3 start = new Vector3(_initialPath[i].x, _initialPath[i].y, 0);
            Vector3 end = new Vector3(_initialPath[i + 1].x, _initialPath[i + 1].y, 0);
            Gizmos.DrawLine(start, end);
        }
    }
} 