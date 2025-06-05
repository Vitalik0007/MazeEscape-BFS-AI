using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private BestFirstSearchPathfinder pathfinder;
    private List<Vector2Int> currentPath = new();
    private MazeCell[,] maze;
    private Transform player;
    private Vector2Int lastPlayerTile;

    private int currentPathIndex = 0;

    private Vector3 offset;
    private float tileSize = 1f;

    private bool isSpeedChanged = false;

    public bool IsSpeedChanged
    {
        get { return isSpeedChanged; }
        private set { }
    }

    public void Initialize(MazeCell[,] mazeData, Transform playerTransform, Vector3 offset, float tileScale)
    {
        maze = mazeData;
        pathfinder = new BestFirstSearchPathfinder(maze);
        player = playerTransform;
        this.offset = offset;
        this.tileSize = 1f * tileScale;

        lastPlayerTile = GetCurrentPlayerTile();
        StartCoroutine(UpdatePathRoutine());
    }

    private IEnumerator UpdatePathRoutine()
    {
        while (true)
        {
            Vector2Int playerTile = GetCurrentPlayerTile();
            Vector2Int enemyTile = GetCurrentEnemyTile();

            if (playerTile != lastPlayerTile)
            {
                currentPath = pathfinder.FindPath(enemyTile, playerTile);
                currentPathIndex = 0;
                lastPlayerTile = playerTile;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Update()
    {
        if (currentPath != null && currentPathIndex < currentPath.Count)
        {
            Vector3 targetPos = TileToWorld(currentPath[currentPathIndex]);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.05f)
            {
                currentPathIndex++;
            }
        }
    }

    private Vector2Int GetCurrentPlayerTile()
    {
        return WorldToTile(player.position);
    }

    private Vector2Int GetCurrentEnemyTile()
    {
        return WorldToTile(transform.position);
    }

    private Vector2Int WorldToTile(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt((worldPos.x - offset.x) / tileSize);
        int y = Mathf.RoundToInt((worldPos.y - offset.y) / tileSize);
        return new Vector2Int(x, y);
    }

    private Vector3 TileToWorld(Vector2Int tilePos)
    {
        return new Vector3(tilePos.x * tileSize, tilePos.y * tileSize, 0f) + offset;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement player))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ChangeSpeed(float multiplier)
    {
        StartCoroutine(ChangeEnemySpeed(multiplier));
    }

    private IEnumerator ChangeEnemySpeed(float speedMultiplier)
    {
        isSpeedChanged = true;
        float startSpeed = moveSpeed;
        moveSpeed *= speedMultiplier;

        yield return new WaitForSeconds(2f);

        moveSpeed = startSpeed;
        isSpeedChanged = false;
    }
}
