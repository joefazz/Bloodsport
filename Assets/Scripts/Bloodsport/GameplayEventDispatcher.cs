using System;

public class GameplayEventDispatcher
{
	public event Action onPlayerKilled;

	public event Action onEnemyKilled;

	public event Action onGameOver;

	public void EnemyKilled()
	{
		onEnemyKilled?.Invoke();
	}

	public void PlayerKilled()
	{
		onPlayerKilled?.Invoke();
	}

	public void GameOver()
	{
		onGameOver?.Invoke();
	}
}
