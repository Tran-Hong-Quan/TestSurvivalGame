using UnityEngine;
using UnityEngine.Events;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool defense;
        public bool attack;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        [Header("Events")]
        public UnityEvent onStartAttack;
        public UnityEvent<bool> onAttack;
        public UnityEvent onStopAttack;

        public PlayerInput inputs;

#if ENABLE_INPUT_SYSTEM

        private void Awake()
        {
            inputs = GetComponent<PlayerInput>();
        }

        private void Start()
        {
            var atkAction = inputs.actions.FindAction("Attack");
            atkAction.started += OnStartAttack;
            atkAction.canceled += OnStopAttack;          
        }
        

        private void OnDestroy()
        {
            var atkAction = inputs.actions.FindAction("Attack");
            atkAction.started -= OnStartAttack;
            atkAction.canceled -= OnStopAttack;
        }

        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        private void OnStartAttack(InputAction.CallbackContext context)
        {
            StartAttackInput();
        }

        private void OnStopAttack(InputAction.CallbackContext context)
        {
            StopAttackInput();
        }

        private void OnAttack(InputValue value)
        {
          
        }

        private void OnDefense(InputValue value)
        {
            DefenseInput(value.isPressed);
        }

#endif

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        public void DefenseInput(bool newDefenseState)
        {
            defense = newDefenseState;
        }

        public void StartAttackInput()
        {
            attack = true;
            onStartAttack?.Invoke();
        }

        public void StopAttackInput()
        {
            attack = false; 
            onStopAttack?.Invoke();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

}