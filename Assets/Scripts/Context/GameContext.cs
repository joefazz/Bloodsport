using System;
using Controllers;
using UnityEditor;
using UnityEngine;

namespace Context
{
    public class GameContext: MonoBehaviour
    {
        private GameManager gameManager;
        private GameObject player;
        private GameObject uiCanvas;

        [SerializeField]
        private Transform playerSpawnOrigin;

        private void Start()
        {
            GameplayEventDispatcher gameplayEventDispatcher = new GameplayEventDispatcher();
            
            uiCanvas = (GameObject) PrefabUtility.InstantiatePrefab(Resources.Load("UICanvas"));
            uiCanvas.GetComponent<UIController>().InitDependencies(gameplayEventDispatcher);
            
            player = (GameObject) PrefabUtility.InstantiatePrefab(Resources.Load("Player"));
            player.transform.position = playerSpawnOrigin.position;
            player.GetComponent<PlayerController>().InitDependencies(gameplayEventDispatcher);
            
            gameManager = new GameObject("GameManager").AddComponent<GameManager>();
            gameManager.InitDependencies(gameplayEventDispatcher);

        }
    }
}