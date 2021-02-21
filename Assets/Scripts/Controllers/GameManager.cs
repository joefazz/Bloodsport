using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float spawnRate = 5f;

        [SerializeField] private float roundLength = 30;
        
        private Coroutine roundLengthTimerCoro;
        private Coroutine enemySpawnTimerCoro;
        private UIEventDispatcher uiEventDispatcher;
        private GameObject player;

        public void InitDependencies(UIEventDispatcher uiEventDispatcher, GameObject player)
        {
            this.uiEventDispatcher = uiEventDispatcher;
            uiEventDispatcher.onPlayerKilled += GameOver;

            this.player = player;
        }

        private void OnDestroy()
        {
            uiEventDispatcher.onPlayerKilled -= GameOver;
        }

        private void Start()
        {
            roundLengthTimerCoro = StartCoroutine(RoundLengthTimer());
            enemySpawnTimerCoro = StartCoroutine(EnemySpawnTimer());
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        private void GameOver()
        {
            Cursor.lockState = CursorLockMode.None;
        }
        
        private IEnumerator RoundLengthTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(roundLength);
                StopCoroutine(enemySpawnTimerCoro);
                StopCoroutine(roundLengthTimerCoro);
            }
        }

        private IEnumerator EnemySpawnTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnRate);
                GameObject spawnedEnemy = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Enemy"));
                spawnedEnemy.GetComponent<EnemyAI>().InitDependencies(uiEventDispatcher, player);
            }
        }
    }
}