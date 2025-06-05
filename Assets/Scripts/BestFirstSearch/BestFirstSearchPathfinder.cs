using System.Collections.Generic;
using UnityEngine;

public class BestFirstSearchPathfinder
{
    private MazeCell[,] maze;
    private int width;
    private int height;

    public BestFirstSearchPathfinder(MazeCell[,] maze)
    {
        this.maze = maze;
        width = maze.GetLength(0);
        height = maze.GetLength(1);
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        if (!IsWithinBounds(start) || !IsWithinBounds(goal))
        {
            return new List<Vector2Int>();
        }

        if (maze[start.x, start.y].Type == TileType.Wall || maze[goal.x, goal.y].Type == TileType.Wall)
        {
            return new List<Vector2Int>();
        }

        var frontier = new PriorityQueue<Vector2Int>();
        frontier.Enqueue(start, Heuristic(start, goal));

        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var visited = new HashSet<Vector2Int> { start };

        while (frontier.Count > 0)
        {
            Vector2Int current = frontier.Dequeue();

            if (current == goal)
                break;

            foreach (var dir in GetNeighbours(current))
            {
                if (!visited.Contains(dir) && maze[dir.x, dir.y].Type != TileType.Wall)
                {
                    visited.Add(dir);
                    cameFrom[dir] = current;
                    frontier.Enqueue(dir, Heuristic(dir, goal));
                }
            }
        }

        return ReconstructPath(cameFrom, start, goal);
    }

    private bool IsWithinBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }

    private int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private IEnumerable<Vector2Int> GetNeighbours(Vector2Int pos)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (var dir in directions)
        {
            Vector2Int newPos = pos + dir;
            if (IsWithinBounds(newPos))
                yield return newPos;
        }
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int start, Vector2Int goal)
    {
        List<Vector2Int> path = new();
        Vector2Int current = goal;

        if (!cameFrom.ContainsKey(goal))
            return path;

        while (current != start)
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Add(start);
        path.Reverse();
        return path;
    }
}
