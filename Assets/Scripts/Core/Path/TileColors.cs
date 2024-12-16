using UnityEngine;

public static class TileColors
{
    public static readonly Color Empty = new Color(0.8f, 0.8f, 0.8f, 0.5f);
    public static readonly Color Road = new Color(0.5f, 0.5f, 0.5f, 1f);
    public static readonly Color Forest = new Color(0.3f, 0.8f, 0.3f, 1f);
    public static readonly Color Mountain = new Color(0.6f, 0.4f, 0.2f, 1f);

    public static Color GetColorForType(TileType type)
    {
        return type switch
        {
            TileType.Empty => Empty,
            TileType.Road => Road,
            TileType.Forest => Forest,
            TileType.Mountain => Mountain,
            _ => Color.white
        };
    }
}