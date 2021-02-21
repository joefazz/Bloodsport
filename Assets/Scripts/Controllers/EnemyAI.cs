using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameObject target;
    private Coroutine shootCoro;
    private bool isShooting;

    private const float FireRate = 0.5f;

    private float health = 10;

    private UIEventDispatcher uiEventDispatcher;

    public void InitDependencies(UIEventDispatcher uiEventDispatcher, GameObject target)
    {
        this.uiEventDispatcher = uiEventDispatcher;
        uiEventDispatcher.onPlayerKilled += Cleanup;
        this.target = target;
    }

    private void OnDestroy()
    {
        uiEventDispatcher.onPlayerKilled -= Cleanup;
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
            uiEventDispatcher.EnemyKilled();
            Destroy(gameObject);
        }

        float distToTarget = Vector2.Distance(target.transform.position, transform.position);

        transform.LookAt(target.transform);

        if (distToTarget > 5)
        {
            if (shootCoro != null)
            {
                StopCoroutine(shootCoro);
            }

            transform.position += transform.forward * 1.1f * Time.deltaTime;
        }
        else
        {
            if (isShooting) return;

            isShooting = true;
            shootCoro = StartCoroutine(ShootAtTarget());
        }
    }

    private IEnumerator ShootAtTarget()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Pew");
        target.GetComponent<PlayerController>().Damage();
        isShooting = false;
    }

    public void Damage()
    {
        health -= 5;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit enemy");
    }
}