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

        private Vector3 m_inputVelocity;
        private float m_animationValue;
        
        public void OnMovement(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            m_inputVelocity.x = input.x;
            m_inputVelocity.z = input.y;

            if (input.magnitude < _minMovementThreshold)
            {
                Velocity = Vector3.zero;
                m_animationValue = 0;
            }
            else if (_minMovementThreshold <= input.magnitude && input.magnitude < _runningThreshold)
            {
                Velocity = m_inputVelocity.normalized * _walkSpeed;
                m_animationValue = .5f;
            }
            else if (input.magnitude >= _runningThreshold)
            {
                Velocity = m_inputVelocity.normalized * _runSpeed;
                m_animationValue = 1f;
            }

            _playerAnimator.UpdateBlendTree(m_animationValue);
            
            CalculateRotation(input);
        }

        private void CalculateRotation(Vector2 input)
        {
            Vector3 targetDir = Vector3.zero;
            targetDir.x = input.x;
            targetDir.z = input.y;
            targetDir.y = 0f;

            if (targetDir == Vector3.zero)
                targetDir = transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            transform.rotation = newRotation;
        }
    }
}