using UnityEngine;

public class MazeCell
{
    public Vector2Int Position { get; }
    public TileType Type { get; set; }
    public bool IsFinishTyle { get; set; }

    public MazeCell(int x, int y, TileType type)
    {
        Position = new Vector2Int(x, y);
        Type = type;
    }
}
