using Player.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Input
{
    public class PlayerInput : MonoBehaviour, PlayerControls.IPlayerActions
    {
        [Header("References")]
        [SerializeField] private PlayerMovement _playerMovement;
        
        private PlayerControls m_controls;
        
        private void BindInput()
        {
            if (m_controls == null)
            {
                m_controls = new PlayerControls();
                m_controls.Player.SetCallbacks(this);
            }
            
            m_controls.Player.Enable();
        }
        
        private void TearInput()
        {
            m_controls.Player.Disable();
        }
        
        public void OnMovement(InputAction.CallbackContext context)
        {
            _playerMovement.OnMovement(context);
        }
        
        private void OnEnable()
        {
            BindInput();
        }

        private void OnDisable()
        {
            TearInput();
        }

        
    }
}