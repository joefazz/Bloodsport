﻿using System;
using System.Collections;
using Controllers;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField]
	private float spawnRate = 5f;

	private Coroutine enemySpawnTimerCoro;

	private GameplayEventDispatcher gameplayEventDispatcher;

	public void InitDependencies(GameplayEventDispatcher gameplayEventDispatcher)
	{
		this.gameplayEventDispatcher = gameplayEventDispatcher;
		gameplayEventDispatcher.onGameOver += Cleanup;
	}

	private void Cleanup()
	{
		StopCoroutine(enemySpawnTimerCoro);
	}

	private void Start()
	{
		enemySpawnTimerCoro = StartCoroutine(EnemySpawnTimer());
	}

	private IEnumerator EnemySpawnTimer()
	{
		while (true)
		{
			yield return new WaitForSeconds(spawnRate);
			GameObject spawnedEnemy = (GameObject) PrefabUtility.InstantiatePrefab(Resources.Load("Enemy"));
			spawnedEnemy.GetComponent<EnemyAI>().InitDependencies(gameplayEventDispatcher);
		}
	}
}