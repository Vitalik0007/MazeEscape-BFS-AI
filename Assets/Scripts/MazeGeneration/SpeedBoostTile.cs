using UnityEngine;

public class SpeedBoostTile : MazeTile
{
    [SerializeField] private float speedBoostMultiplier;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMovement playerMovement))
        {
            if (!playerMovement.IsSpeedChanged)
                playerMovement.ChangeSpeed(speedBoostMultiplier);
        }
        else if (collision.TryGetComponent(out EnemyAI enemyAI))
        {
            if (!enemyAI.IsSpeedChanged)
                enemyAI.ChangeSpeed(speedBoostMultiplier);
        }
    }
}