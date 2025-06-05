using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject swampPrefab;
    [SerializeField] private GameObject speedBoostPrefab;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;

    [Header("Maze Config")]
    [SerializeField] private float tileScale = 1f;

    [Header("Levels")]
    [SerializeField] private LevelDatabase levelDatabase;
    private int currentLevelIndex = 0;

    private MazeGenerator generator;
    private MazeCell[,] maze;

    private GameObject player;
    private Vector3 offset;

    private List<MazeTile> allTiles = new List<MazeTile>();
    private List<GameObject> enemies = new List<GameObject>();

    private LevelConfig currentLevel;

    private void Start()
    {
        generator = GetComponent<MazeGenerator>();
        LoadLevel(currentLevelIndex);
    }

    public void LoadLevel(int levelIndex)
    {
        if (player != null)
            Destroy(player);

        foreach (GameObject enemy in enemies)
            Destroy(enemy);
        enemies.Clear();

        currentLevelIndex = levelIndex;
        currentLevel = levelDatabase.GetLevel(currentLevelIndex);

        allTiles.Clear();
        maze = generator.GenerateMaze(
            currentLevel.width,
            currentLevel.height,
            currentLevel.swampTileCount,
            currentLevel.speedBoostTileCount
        );

        foreach (Transform child in transform)
            Destroy(child.gameObject);

        DrawMaze(maze);
        PlacePlayerAndEnemies();
    }

    public void NextLevel()
    {
        int nextLevel = currentLevelIndex + 1;
        if (nextLevel < levelDatabase.levels.Count)
        {
            LoadLevel(nextLevel);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void DrawMaze(MazeCell[,] maze)
    {
        float tileSize = tileScale;
        offset = new Vector3(-(maze.GetLength(0) - 1) / 2f * tileSize, -(maze.GetLength(1) - 1) / 2f * tileSize, 0);

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                MazeCell cell = maze[x, y];
                GameObject prefab = GetPrefabForTile(cell.Type);

                /*if (cell.Type == TileType.Swamp && !currentLevel.useSwamp)
                    prefab = floorPrefab;
                if (cell.Type == TileType.SpeedBoost && !currentLevel.useSpeedBoost)
                    prefab = floorPrefab;*/

                Vector3 position = new Vector3(x * tileSize, y * tileSize, 0) + offset;
                GameObject tileObject = Instantiate(prefab, position, Quaternion.identity, transform);

                MazeTile mazeTile = tileObject.GetComponent<MazeTile>();
                if (cell.IsFinishTyle) mazeTile.isFinishTile = true;

                allTiles.Add(mazeTile);
            }
        }
    }

    private GameObject GetPrefabForTile(TileType type)
    {
        return type switch
        {
            TileType.Floor => floorPrefab,
            TileType.Wall => wallPrefab,
            TileType.Swamp => swampPrefab,
            TileType.SpeedBoost => speedBoostPrefab,
            _ => floorPrefab,
        };
    }

    private void PlacePlayerAndEnemies()
    {
        MazeTile playerStartTile = null;

        foreach (MazeTile tile in allTiles)
        {
            if (tile.type == TileType.Floor)
            {
                playerStartTile = tile;
                break;
            }
        }

        if (playerStartTile == null)
        {
            Debug.LogError("Не знайдено стартовий тайл для гравця!");
            return;
        }

        Vector3 playerPosition = playerStartTile.transform.position;
        player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
        player.GetComponent<PlayerMovement>().Instantiate(this);

        List<MazeTile> validEnemyTiles = new();
        float minDistance = 5f;

        foreach (MazeTile tile in allTiles)
        {
            if (tile.type != TileType.Floor) continue;

            float dist = Vector3.Distance(tile.transform.position, playerPosition);
            if (dist >= minDistance)
            {
                validEnemyTiles.Add(tile);
            }
        }

        if (validEnemyTiles.Count == 0)
        {
            Debug.LogWarning("Не знайдено віддалених тайлів для ворогів. Встановлюємо поруч.");
            validEnemyTiles.Add(playerStartTile);
        }

        for (int i = 0; i < currentLevel.enemyCount; i++)
        {
            MazeTile enemyTile = validEnemyTiles[Random.Range(0, validEnemyTiles.Count)];
            Vector3 enemyPos = enemyTile.transform.position;
            GameObject enemy = Instantiate(enemyPrefab, enemyPos, Quaternion.identity);

            enemy.GetComponent<EnemyAI>().Initialize(maze, player.transform, offset, tileScale);

            enemies.Add(enemy);
        }
    }
}
