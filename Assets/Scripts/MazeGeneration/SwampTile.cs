using UnityEngine;

public class SwampTile : MazeTile
{
    [SerializeField] private float swampMultiplier;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMovement playerMovement))
        {
            if (!playerMovement.IsSpeedChanged)
                playerMovement.ChangeSpeed(swampMultiplier);
        }
        else if (collision.TryGetComponent(out EnemyAI enemyAI))
        {
            if (!enemyAI.IsSpeedChanged)
                enemyAI.ChangeSpeed(swampMultiplier);
        }
    }
}