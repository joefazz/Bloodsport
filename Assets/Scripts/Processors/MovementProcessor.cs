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
		private readonly float playerSpeed = 8f;

		private readonly float jumpHeight = 1f;

		private readonly float dashDistance = 1f;

		private readonly Vector3 drag = new Vector3(2, 2, 2);

		private float dashTime = 0f;

		private readonly float minY = -60f;

		private readonly float maxY = 60f;

		private readonly Vector2 sensitivity = new Vector2(0.25f, 0.25f);

		private readonly Transform playerTransform;

		private readonly CharacterController characterController;

		private Vector2 moveDirection;

		private float lookX;

		private float lookY;

		private bool isDashActionQueued = false;

		private Vector3 playerVelocity;

		private PlayerMovementState currentState;

		private bool shouldJump;

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
			else
			{
				playerVelocity.x = 0;
				playerVelocity.z = 0;
			}

			if (shouldJump && currentState == PlayerMovementState.Grounded)
			{
				playerVelocity.y += Mathf.Sqrt(jumpHeight * -3f * Physics.gravity.y);
			}

			characterController.Move(playerVelocity * Time.deltaTime);
		}

		// Dash distance actually has virtually no impact really the only thing that matters is what you plug into the move function
		private void Dash()
		{
			if (dashTime <= dashDistance)
			{
				currentState = PlayerMovementState.Dashing;

				Vector3 movement = playerTransform.forward * 20f - (drag * dashTime);

				movement.y = 0;

				characterController.Move(movement * Time.deltaTime);

				dashTime += Time.deltaTime;
			}
			else
			{
				dashTime = 0;
				isDashActionQueued = false;
			}
		}

		private void CheckGroundCollider(Vector3 groundValidatorPos, float groundValidatorRadius, LayerMask groundLayer)
		{
			bool isSphereTouchingGround = Physics.CheckSphere(groundValidatorPos, groundValidatorRadius, groundLayer, QueryTriggerInteraction.Ignore);

			if (!isSphereTouchingGround)
			{
				currentState = PlayerMovementState.Airbourne;
			}
			else
			{
				currentState = PlayerMovementState.Grounded;
				playerVelocity.y = 0;
			}
		}

		private void Gravity()
		{
			playerVelocity.y += Physics.gravity.y * Time.deltaTime;
		}

		public void SetMoveDirectionFromInput(Vector2 direction)
		{
			moveDirection = direction;
		}

		public void SetRotationXYFromInput(Vector2 lookXY)
		{
			if (currentState == PlayerMovementState.Dashing) return;

			lookX += lookXY.x * sensitivity.x;
			lookY -= lookXY.y * sensitivity.y;
		}

		private void Move()
		{
			Vector3 forward = playerTransform.forward;
			Vector3 right = playerTransform.right;

			if (currentState == PlayerMovementState.Grounded)
			{
				Ray ray = new Ray(playerTransform.position, Vector3.down);

				if (Physics.Raycast(ray, out RaycastHit hitInfo))
				{
					forward *= hitInfo.normal.y;
				}

				Debug.DrawRay(ray.origin, ray.direction);
			}

			Vector3 movement =
				(forward * moveDirection.y + right * moveDirection.x)
				* playerSpeed;

			movement.y = 0;

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
			this.shouldJump = true;
		}

		public void StopJump()
		{
			this.shouldJump = false;
		}

		public void TriggerDash()
		{
			isDashActionQueued = true;
		}
	}
}
