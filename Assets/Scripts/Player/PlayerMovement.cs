using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private LayerMask tileLayer;
    [SerializeField] private float rayLength = 0.6f;

    private Vector2Int currentDirection = Vector2Int.zero;
    private GridManager gridManager;

    private bool isSpeedChanged = false;

    public bool IsSpeedChanged
    {
        get { return isSpeedChanged; }
        private set { }
    }

    public void Instantiate(GridManager gridManager)
    {
        this.gridManager = gridManager;
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            currentDirection = Vector2Int.up;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            currentDirection = Vector2Int.down;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            currentDirection = Vector2Int.left;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            currentDirection = Vector2Int.right;
        }
    }

    private void MovePlayer()
    {
        Vector3 direction = new Vector3(currentDirection.x, currentDirection.y, 0f).normalized;

        if (IsWalkable(direction))
        {
            transform.position += direction * moveSpeed * Time.fixedDeltaTime;
        }
    }

    private bool IsWalkable(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + direction * 0.25f, direction, rayLength, tileLayer);

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out MazeTile mazeTile))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out MazeTile mazeTile))
        {
            if (mazeTile.isFinishTile)
            {
                gridManager.NextLevel();
            }
        }
    }

    public void ChangeSpeed(float multiplier)
    {
        StartCoroutine(ChangePlayerSpeed(multiplier));
    }

    private IEnumerator ChangePlayerSpeed(float speedMultiplier)
    {
        isSpeedChanged = true;
        float startSpeed = moveSpeed;
        moveSpeed *= speedMultiplier;

        yield return new WaitForSeconds(2f);

        moveSpeed = startSpeed;
        isSpeedChanged = false;
    }
}
