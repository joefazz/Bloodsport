using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
	enum PlayerMovementState
	{
		Airbourne,

		Stationary,

		Walking
	}

	public class MovementController
	{
		private readonly float playerSpeed = 4f;

		private readonly float minY = -60f;

		private readonly float maxY = 60f;

		private readonly Vector2 sensitivity = new Vector2(0.25f, 0.25f);

		private readonly Transform playerTransform;

		private readonly CharacterController characterController;

		private Vector2 moveDirection;

		private float lookX;

		private float lookY;

		private PlayerMovementState currentState;

		public MovementController(Transform playerTransform, CharacterController characterController)
		{
			this.playerTransform = playerTransform;
			this.characterController = characterController;
			this.currentState = PlayerMovementState.Stationary;
		}

		public void Update(Vector3 groundValidatorPos, float groundValidatorRadius, LayerMask groundLayer)
		{
			CheckGroundCollider(groundValidatorPos, groundValidatorRadius, groundLayer);
			
			Move();
			Rotate();

			if (currentState == PlayerMovementState.Airbourne)
			{
				Gravity();
			}
			else
			{
				
			}
		}

		private void CheckGroundCollider(Vector3 groundValidatorPos, float groundValidatorRadius, LayerMask groundLayer)
		{
			if (!Physics.CheckSphere(groundValidatorPos, groundValidatorRadius, groundLayer, QueryTriggerInteraction.Ignore))
			{
				currentState = PlayerMovementState.Airbourne;
			}
		}

		private void Gravity()
		{
			Vector3 velocity = characterController.velocity;

			velocity.y += Physics.gravity.y;

			characterController.Move(velocity * Time.deltaTime);
		}

		public void SetMoveDirectionFromInput(Vector2 direction)
		{
			moveDirection = direction;
		}

		public void SetRotationXYFromInput(Vector2 lookXY)
		{
			lookX += lookXY.x * sensitivity.x;
			lookY -= lookXY.y * sensitivity.y;
		}

		private void Move()
		{
			Vector3 movement =
				(playerTransform.forward * moveDirection.y + playerTransform.right * moveDirection.x)
				* playerSpeed;

			characterController.Move(movement * Time.deltaTime);
		}

		private void Rotate()
		{
			lookY = Mathf.Clamp(lookY, minY, maxY);
			playerTransform.localEulerAngles = new Vector3(lookY, lookX, 0);
		}

		public void SetJumpFromInput()
		{
			if (currentState != PlayerMovementState.Airbourne)
			{
				Vector3 velocity = characterController.velocity;
				velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y)
			}
		}
	}
}
