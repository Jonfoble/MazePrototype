using Abstraction.Movement;
using DG.Tweening;
using Player.Animation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Movement
{
	public class PlayerMovement : MovementBase
	{
		[Header("References")]
		[SerializeField] private PlayerAnimator _playerAnimator;

		[Header("Input Settings")]
		[SerializeField] private float _minMovementThreshold;
		[SerializeField] private float _runningThreshold;

		[Header("Movement Settings")]
		[SerializeField] private float _walkSpeed;
		[SerializeField] private float _runSpeed;

		[Header("WIP")]
		[SerializeField] private float _speedTransitionDuration;
		[SerializeField] private Ease _speedTransitionEase;

		private Vector3 _inputVelocity;
		private float _animationValue;

		public void OnMovement(InputAction.CallbackContext context)
		{
			Vector2 input = context.ReadValue<Vector2>();
			_inputVelocity = new Vector3(input.x, 0, input.y);

			UpdateVelocityAndAnimation(input.magnitude);
			UpdateRotation(input);
		}

		private void UpdateVelocityAndAnimation(float inputMagnitude)
		{
			if (inputMagnitude < _minMovementThreshold)
			{
				SetMovement(Vector3.zero, 0);
			}
			else if (inputMagnitude < _runningThreshold)
			{
				SetMovement(_inputVelocity.normalized * _walkSpeed, 0.5f);
			}
			else
			{
				SetMovement(_inputVelocity.normalized * _runSpeed, 1f);
			}
		}

		private void SetMovement(Vector3 velocity, float animationValue)
		{
			Velocity = velocity;
			_animationValue = animationValue;
			_playerAnimator.UpdateBlendTree(_animationValue);
		}

		private void UpdateRotation(Vector2 input)
		{
			Vector3 targetDir = input == Vector2.zero ? transform.forward : new Vector3(input.x, 0, input.y);
			Quaternion targetRotation = Quaternion.LookRotation(targetDir);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
		}
	}
}
