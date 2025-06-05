using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    private int width, height;
    private MazeCell[,] maze;
    private System.Random rng = new();

    [SerializeField] private int numberOfExits = 2;
    [SerializeField] private int extraConnections = 15;

    public MazeCell[,] GenerateMaze(int w, int h, int swampCount = 0, int speedBoostCount = 0)
    {
        width = (w % 2 == 0) ? w - 1 : w;
        height = (h % 2 == 0) ? h - 1 : h;

        maze = new MazeCell[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                maze[x, y] = new MazeCell(x, y, TileType.Wall);

        Vector2Int start = new Vector2Int(1, 1);
        maze[start.x, start.y].Type = TileType.Floor;

        RecursiveBacktrack(start);
        AddExtraConnections(extraConnections);
        AddMultipleExits(numberOfExits);

        AddSpecialTiles(swampCount, speedBoostCount);

        return maze;
    }

    private void RecursiveBacktrack(Vector2Int current)
    {
        foreach (Vector2Int dir in GetShuffledDirections())
        {
            Vector2Int next = current + dir * 2;
            if (IsInBounds(next) && maze[next.x, next.y].Type == TileType.Wall)
            {
                Vector2Int wall = current + dir;
                maze[wall.x, wall.y].Type = TileType.Floor;
                maze[next.x, next.y].Type = TileType.Floor;
                RecursiveBacktrack(next);
            }
        }
    }

    private void AddExtraConnections(int count)
    {
        int added = 0;
        int attempts = 0;

        while (added < count && attempts < count * 10)
        {
            attempts++;

            int x = rng.Next(1, width - 1);
            int y = rng.Next(1, height - 1);

            if (x % 2 == 0 && y % 2 == 1 || x % 2 == 1 && y % 2 == 0)
            {
                Vector2Int dir = (x % 2 == 0) ? Vector2Int.right : Vector2Int.up;
                Vector2Int left = new Vector2Int(x, y) - dir;
                Vector2Int right = new Vector2Int(x, y) + dir;

                if (IsInBounds(left) && IsInBounds(right)
                    && maze[left.x, left.y].Type == TileType.Floor
                    && maze[right.x, right.y].Type == TileType.Floor
                    && maze[x, y].Type == TileType.Wall)
                {
                    maze[x, y].Type = TileType.Floor;
                    added++;
                }
            }
        }
    }

    private void AddMultipleExits(int exitCount)
    {
        List<Vector2Int> potentialExits = new();

        for (int y = 1; y < height - 1; y++)
        {
            if (maze[width - 2, y].Type == TileType.Floor)
                potentialExits.Add(new Vector2Int(width - 1, y));
        }

        for (int i = 0; i < potentialExits.Count; i++)
        {
            int j = rng.Next(i, potentialExits.Count);
            (potentialExits[i], potentialExits[j]) = (potentialExits[j], potentialExits[i]);
        }

        for (int i = 0; i < Mathf.Min(exitCount, potentialExits.Count); i++)
        {
            Vector2Int pos = potentialExits[i];
            maze[pos.x, pos.y].Type = TileType.Floor;
            maze[pos.x, pos.y].IsFinishTyle = true;
        }
    }

    private void AddSpecialTiles(int swampCount, int speedBoostCount)
    {
        List<MazeCell> floorTiles = new();

        foreach (var cell in maze)
        {
            if (cell.Type == TileType.Floor && !cell.IsFinishTyle)
                floorTiles.Add(cell);
        }

        Shuffle(floorTiles);

        for (int i = 0; i < Mathf.Min(swampCount, floorTiles.Count); i++)
        {
            floorTiles[i].Type = TileType.Swamp;
        }

        for (int i = swampCount; i < Mathf.Min(swampCount + speedBoostCount, floorTiles.Count); i++)
        {
            floorTiles[i].Type = TileType.SpeedBoost;
        }
    }

    private void Shuffle(List<MazeCell> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private bool IsInBounds(Vector2Int pos)
    {
        return pos.x > 0 && pos.x < width - 1 && pos.y > 0 && pos.y < height - 1;
    }

    private List<Vector2Int> GetShuffledDirections()
    {
        List<Vector2Int> dirs = new()
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        for (int i = 0; i < dirs.Count; i++)
        {
            int j = rng.Next(i, dirs.Count);
            (dirs[i], dirs[j]) = (dirs[j], dirs[i]);
        }

        return dirs;
    }
}
