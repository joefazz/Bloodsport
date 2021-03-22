using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Controllers
{
	public class GameManager : MonoBehaviour
	{
		private GameplayEventDispatcher gameplayEventDispatcher;

		public void InitDependencies(GameplayEventDispatcher gameplayEventDispatcher)
		{
			this.gameplayEventDispatcher = gameplayEventDispatcher;
			gameplayEventDispatcher.onPlayerKilled += GameOver;
		}

		private void OnDestroy()
		{
			gameplayEventDispatcher.onPlayerKilled -= GameOver;
		}

		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
		}

		private void GameOver()
		{
			gameplayEventDispatcher.GameOver();
			Cursor.lockState = CursorLockMode.None;
		}
	}
}
