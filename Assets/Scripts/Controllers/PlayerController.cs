﻿using System;
using Processors;
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
		private Transform groundValidator;

		[SerializeField]
		private float health;

		[SerializeField]
		private float groundValidatorRadius;

		[SerializeField]
		private LayerMask groundLayer;

		private int layerMask;

		private GameplayEventDispatcher gameplayEventDispatcher;

		private MovementProcessor movementProcessor;

		public float Health => health;

		public void InitDependencies(GameplayEventDispatcher gameplayEventDispatcher)
		{
			this.gameplayEventDispatcher = gameplayEventDispatcher;
		}

		private void Start()
		{
			this.layerMask = 1 << 8;
			movementProcessor = new MovementProcessor(transform, characterController);
		}

		private void Update()
		{
			if (health > 0)
			{
				movementProcessor.Update(groundValidator.position, groundValidatorRadius, groundLayer);
			}
			else
			{
				gameplayEventDispatcher.PlayerKilled();
			}
		}

		private void OnDrawGizmosSelected()
		{
			// Draw a yellow sphere at the transform's position
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(groundValidator.position, groundValidatorRadius);

		}

		// Triggered from InputSystem
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

		// Triggered from InputSystem
		public void OnMove(InputAction.CallbackContext ctx)
		{
			movementProcessor.SetMoveDirectionFromInput(ctx.ReadValue<Vector2>());
		}

		public void OnJump(InputAction.CallbackContext ctx)
		{
			if (!ctx.performed) return;

			movementProcessor.AttemptJump();
		}

		public void OnDash(InputAction.CallbackContext ctx)
		{
			if (!ctx.performed) return;
			
			movementProcessor.TriggerDash();
		}

		// Triggered from InputSystem
		public void OnLook(InputAction.CallbackContext ctx)
		{
			movementProcessor.SetRotationXYFromInput(ctx.ReadValue<Vector2>());
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
