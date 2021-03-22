using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Controllers;
using UnityEditor;
using UnityEngine;

public class NonCombatContext : MonoBehaviour
{
    [SerializeField]
    private Transform playerSpawnOrigin;

    [SerializeField]
    private CinemachineBrain cameraBrain;
    
    private GameObject player;
    
    // Start is called before the first frame update
    private void Start()
    {
        GameplayEventDispatcher gameplayEventDispatcher = new GameplayEventDispatcher();
        
        player = (GameObject) PrefabUtility.InstantiatePrefab(Resources.Load("Player"));
        player.transform.position = playerSpawnOrigin.position;
        player.GetComponent<PlayerController>().InitDependencies(gameplayEventDispatcher, cameraBrain.OutputCamera, false);
    }
}
