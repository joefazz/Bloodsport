using UnityEngine;
using UnityEngine.InputSystem;

namespace Processors
{
	enum PlayerMovementState
	{
		Airbourne,

		Grounded,

		Dashing
	}

	public class MovementProcessor
	{
		private readonly float playerSpeed = 4f;

		private readonly float jumpHeight = 2f;

		private readonly float dashDistance = 1f;

		private readonly Vector3 drag = new Vector3(5, 5, 5);

		private readonly float minY = -60f;

		private readonly float maxY = 60f;

		private readonly Vector2 sensitivity = new Vector2(0.25f, 0.25f);

		private readonly Transform playerTransform;

		private readonly CharacterController characterController;

		private Vector2 moveDirection;

		private float lookX;

		private float lookY;

		private bool isDashActionQueued = false;

		private PlayerMovementState currentState;

		public MovementProcessor(Transform playerTransform, CharacterController characterController)
		{
			this.playerTransform = playerTransform;
			this.characterController = characterController;
			this.currentState = PlayerMovementState.Grounded;
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

			if (isDashActionQueued)
			{
				Dash();
			}
		}

		private void CheckGroundCollider(Vector3 groundValidatorPos, float groundValidatorRadius, LayerMask groundLayer)
		{
			if (!Physics.CheckSphere(groundValidatorPos, groundValidatorRadius, groundLayer, QueryTriggerInteraction.Ignore))
			{
				currentState = PlayerMovementState.Airbourne;
			}
			else
			{
				currentState = PlayerMovementState.Grounded;
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

		// Needs some work also
		public void AttemptJump()
		{
			if (currentState == PlayerMovementState.Grounded)
			{
				Vector3 velocity = characterController.velocity;

				velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);

				characterController.Move(velocity);
			}
		}

		public void TriggerDash()
		{
			isDashActionQueued = true;
		}

		// Needs some work
		private void Dash()
		{
			Vector3 velocity = characterController.velocity;

			velocity += Vector3.Scale(playerTransform.forward,
				dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * drag.x + 1)) / -Time.deltaTime),
					0,
					(Mathf.Log(1f / (Time.deltaTime * drag.z + 1)) / -Time.deltaTime)));

			characterController.Move(velocity);
		}
	}
}
