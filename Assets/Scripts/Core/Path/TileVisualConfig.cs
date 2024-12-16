using UnityEngine;

[CreateAssetMenu(fileName = "TileVisualConfig", menuName = "Game/Tile Visual Config")]
public class TileVisualConfig : ScriptableObject
{
    [System.Serializable]
    public class TileVisualData
    {
        public TileType type;
        public Sprite sprite;
        public Color tint = Color.white;
    }

    [SerializeField] private TileVisualData[] _tileVisuals;

    public Sprite GetSprite(TileType type)
    {
        var data = System.Array.Find(_tileVisuals, x => x.type == type);
        return data?.sprite;
    }

    public Color GetColor(TileType type)
    {
        var data = System.Array.Find(_tileVisuals, x => x.type == type);
        return data?.tint ?? Color.white;
    }
} 