using UnityEngine;

namespace Abstraction.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public abstract class MovementBase : MonoBehaviour
    {
        protected CharacterController Controller;
        protected Vector3 Velocity;
        protected bool IsGrounded;
        [SerializeField] protected float _gravity = -9.81f;
        [SerializeField] protected float _rotationSpeed = 120f;
        [SerializeField] protected float _groundDistance = 0.4f;
        [SerializeField] protected Transform _groundCheck;
        [SerializeField] protected LayerMask _groundMask;

        protected virtual void Start()
        {
            Controller = GetComponent<CharacterController>();
        }
        
        protected virtual void Update()
        {
            IsGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

            if (IsGrounded && Velocity.y < 0f)
            {
                Velocity.y = -2f;
            }
            
            Velocity.y += _gravity;
            
            Controller.Move(Velocity * Time.deltaTime);
        }

    }
}
