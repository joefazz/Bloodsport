using System;

public class UIEventDispatcher
{
    public event Action onPlayerKilled;
    public event Action onEnemyKilled;

    public void EnemyKilled()
    {
        onEnemyKilled?.Invoke();
    }

    public void PlayerKilled()
    {
        onPlayerKilled?.Invoke();
    }
}