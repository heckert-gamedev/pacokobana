using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace he
{
    public class PlayerLocomotion : MonoBehaviour
    {

        [SerializeField] float moveSpeed = 1f;

        Rigidbody2D rb;
        PlayerInputHandler inputHandler;


        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            inputHandler = GetComponent<PlayerInputHandler>();
        }


        void Update()
        {
            Vector3 myPosition = rb.position;
            rb.MovePosition(myPosition + (moveSpeed * Time.deltaTime * inputHandler.MoveVector));
        }

    }
}
