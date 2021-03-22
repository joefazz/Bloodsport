using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class EnemyAI : MonoBehaviour
{
	private Coroutine shootCoro;

	private bool isShooting;

	private GameObject target;

	[SerializeField]
	private VisualEffect bloodSpurt;

	[SerializeField]
	private NavMeshAgent navMeshAgent;

	private const float FireRate = 0.5f;

	private float health = 10;

	private GameplayEventDispatcher gameplayEventDispatcher;

	public void InitDependencies(GameplayEventDispatcher gameplayEventDispatcher, GameObject target)
	{
		this.gameplayEventDispatcher = gameplayEventDispatcher;
		gameplayEventDispatcher.onPlayerKilled += Cleanup;
		this.target = target;
	}

	private void OnDestroy()
	{
		gameplayEventDispatcher.onPlayerKilled -= Cleanup;
	}

	private void Cleanup()
	{
		Destroy(gameObject);
	}

	// Calculate distance between enemy and character controller, move some fraction of that based on move speed, within a certain radius stop and fire

	void Update()
	{
		if (health <= 0)
		{
			gameplayEventDispatcher.EnemyKilled();
			Destroy(gameObject);
		}

		navMeshAgent.SetDestination(target.transform.position);
		
		// if (isShooting) return;
		//
		// isShooting = true;
		// shootCoro = StartCoroutine(ShootAtTarget());
	}

	private IEnumerator ShootAtTarget()
	{
		yield return new WaitForSeconds(1);
		Debug.Log("Pew");
		target.GetComponent<PlayerController>().Damage();
		isShooting = false;
	}

	public void Damage(Vector3 hitPos)
	{
		health -= 5;
		bloodSpurt.transform.position = hitPos;
		bloodSpurt.transform.LookAt(target.transform);
		bloodSpurt.Play();
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("hit enemy");
	}
}
