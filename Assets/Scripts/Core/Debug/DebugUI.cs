using UnityEngine;
using TMPro;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _debugText;
    private TilePlacer _tilePlacer;
    private TileSystem _tileSystem;

    private void Start()
    {
        _tilePlacer = FindObjectOfType<TilePlacer>();
        _tileSystem = FindObjectOfType<TileSystem>();
        
        if (_debugText == null)
        {
            Debug.LogError("DebugText not assigned!");
        }
    }

    private void Update()
    {
        if (_debugText == null) return;

        string debug = "";
        if (_tilePlacer != null && _tilePlacer.CurrentMouseGridPosition.HasValue)
        {
            debug += $"Mouse Grid Pos: {_tilePlacer.CurrentMouseGridPosition.Value}\n";
            debug += $"Current Tile Type: {_tilePlacer.CurrentTileType}\n";
        }
        else
        {
            debug += "TilePlacer not found or mouse position invalid\n";
        }

        if (_tileSystem != null)
        {
            debug += $"Tile Prefabs Count: {_tileSystem.TilePrefabsCount}\n";
        }
        else
        {
            debug += "TileSystem not found\n";
        }

        _debugText.text = debug;
    }
} 