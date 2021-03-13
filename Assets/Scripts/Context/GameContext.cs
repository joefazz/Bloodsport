using System;
using Cinemachine;
using Controllers;
using UnityEditor;
using UnityEngine;

namespace Context
{
    public class GameContext: MonoBehaviour
    {
        private GameObject player;
        private GameObject uiCanvas;

        [SerializeField]
        private Transform playerSpawnOrigin;

        [SerializeField]
        private GameManager gameManager;

        [SerializeField]
        private EnemySpawner enemySpawner;

        [SerializeField]
        private CinemachineBrain cameraBrain;
        
        private void Start()
        {
            GameplayEventDispatcher gameplayEventDispatcher = new GameplayEventDispatcher();
            
            uiCanvas = (GameObject) PrefabUtility.InstantiatePrefab(Resources.Load("UICanvas"));
            uiCanvas.GetComponent<UIController>().InitDependencies(gameplayEventDispatcher);
            
            player = (GameObject) PrefabUtility.InstantiatePrefab(Resources.Load("Player"));
            player.transform.position = playerSpawnOrigin.position;
            player.GetComponent<PlayerController>().InitDependencies(gameplayEventDispatcher, cameraBrain.OutputCamera);
            
            gameManager.InitDependencies(gameplayEventDispatcher);

            enemySpawner.InitDependencies(gameplayEventDispatcher, player);
        }
    }
}