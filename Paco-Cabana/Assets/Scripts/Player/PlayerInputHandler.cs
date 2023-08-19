using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace he
{
    public class PlayerInputHandler : MonoBehaviour
    {

        [SerializeField] float deadZone = 0.1f;

        InputMapping inputActions;
        Vector3 moveVector;

        public Vector3 MoveVector { get => moveVector; private set => moveVector = value; }


        private void Awake()
        {
            inputActions = new InputMapping();
        }


        private void OnEnable()
        {
            inputActions.Movement.UpDown.performed += _ => OnUpDownAction();
            inputActions.Movement.LeftRight.performed += _ => OnLeftRightAction();

            inputActions.Movement.UpDown.canceled += _ => OnStopMovementAction();
            inputActions.Movement.LeftRight.canceled += _ => OnStopMovementAction();

            inputActions.Enable();
        }


        private void OnDisable()
        {
            inputActions.Disable();

            inputActions.Movement.UpDown.performed -= _ => OnUpDownAction();
            inputActions.Movement.LeftRight.performed -= _ => OnLeftRightAction();

            inputActions.Movement.UpDown.canceled -= _ => OnStopMovementAction();
            inputActions.Movement.LeftRight.canceled -= _ => OnStopMovementAction();
        }


        void OnStopMovementAction()
        {
            moveVector = Vector3.zero;
        }


        void OnLeftRightAction()
        {
            float moveInput = inputActions.Movement.LeftRight.ReadValue<float>();
            if (Mathf.Abs(moveInput) > deadZone)
            {
                moveVector.x = moveInput;
                moveVector.y = 0f;
            }
        }


        void OnUpDownAction()
        {
            float moveInput = inputActions.Movement.UpDown.ReadValue<float>();
            if (Mathf.Abs(moveInput) > deadZone)
            {
                moveVector.x = 0f;
                moveVector.y = moveInput;
            }

        }

    }
}
