﻿using System;
using Player;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;
using UnityEngine.VFX;

namespace Controllers
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private Camera camera;

		[SerializeField]
		private CharacterController characterController;

		[SerializeField]
		private float health;

		private int layerMask;

		private GameplayEventDispatcher gameplayEventDispatcher;

		private MovementController movementController;

		public float Health => health;

		public void InitDependencies(GameplayEventDispatcher gameplayEventDispatcher)
		{
			this.gameplayEventDispatcher = gameplayEventDispatcher;
		}

		private void Start()
		{
			this.layerMask = 1 << 8;
			movementController = new MovementController(transform, characterController);
		}

		private void Update()
		{
			if (health > 0)
			{
				movementController.Update();
			}
			else
			{
				gameplayEventDispatcher.PlayerKilled();
			}
		}

		public void OnFire(InputAction.CallbackContext ctx)
		{
			if (!ctx.performed) return;

			GetComponentInChildren<VisualEffect>().Play();
			if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
			{
				Debug.Log("Hit thing");
				hit.collider.gameObject.GetComponent<EnemyAI>().Damage();
			}
		}

		public void OnMove(InputAction.CallbackContext ctx)
		{
			movementController.SetMoveDirectionFromInput(ctx.ReadValue<Vector2>());
		}

		public void OnLook(InputAction.CallbackContext ctx)
		{
			movementController.SetRotationXYFromInput(ctx.ReadValue<Vector2>());
		}

		private void OnCollisionEnter(Collision other)
		{
			Damage();
		}

		public void Damage()
		{
			health -= 5;
		}
	}
}