using System;
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
		private CharacterController characterController;

		[SerializeField]
		private Transform groundValidator;

		[SerializeField]
		private float health;

		[SerializeField]
		private float groundValidatorRadius;

		[SerializeField]
		private LayerMask groundLayer;

		[SerializeField]
		private LayerMask enemyLayer;

		[SerializeField]
		private Transform headTransform;
		
		private GameplayEventDispatcher gameplayEventDispatcher;

		private MovementProcessor movementProcessor;

		private Camera playerCamera;

		public float Health => health;

		public void InitDependencies(GameplayEventDispatcher gameplayEventDispatcher, Camera playerCamera)
		{
			this.gameplayEventDispatcher = gameplayEventDispatcher;
			this.playerCamera = playerCamera;
		}

		private void Start()
		{
			movementProcessor = new MovementProcessor(headTransform, characterController);
		}

		private void Update()
		{
			if (health <= 0)
			{
				gameplayEventDispatcher.PlayerKilled();
			}
		}

		private void FixedUpdate()
		{
			if (health > 0)
			{
				movementProcessor.Update(groundValidator.position, groundValidatorRadius, groundLayer);
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

			Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

			if (Physics.Raycast(ray, out RaycastHit hit, enemyLayer))
			{
				Debug.Log("Hit thing");
				hit.collider.gameObject.GetComponent<EnemyAI>().Damage(hit.point);
			}
		}

		// Triggered from InputSystem
		public void OnMove(InputAction.CallbackContext ctx)
		{
			movementProcessor.SetMoveDirectionFromInput(ctx.ReadValue<Vector2>());
		}

		public void OnJump(InputAction.CallbackContext ctx)
		{
			if (ctx.performed)
			{
				movementProcessor.AttemptJump();
			}
			else if (ctx.canceled)
			{
				movementProcessor.StopJump();
			}
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
			// health -= 5;
		}
	}
}
