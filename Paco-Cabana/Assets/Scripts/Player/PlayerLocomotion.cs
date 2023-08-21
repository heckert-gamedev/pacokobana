using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEditor.Compilation;

namespace he
{
    public class PlayerLocomotion : MonoBehaviour
    {

        [SerializeField] float moveSpeed = 1f;
        [SerializeField] float gridLength = 1f;
        [SerializeField] Tilemap pathWays;


        Rigidbody2D rb;
        PlayerInputHandler inputHandler;


        private void Awake()
        {
            DOTween.Init();
            rb = GetComponentInChildren<Rigidbody2D>();
            inputHandler = GetComponent<PlayerInputHandler>();
        }


        void Update()
        {
            if (DOTween.IsTweening(rb)) return;

            if (Vector3.zero != inputHandler.MoveVector)    // enforces dropping out of tween at end of move
            {

                //rb.DOMove(transform.position + inputHandler.MoveVector, .25f, false);
                //rb.DOMove(transform.position + (inputHandler.MoveVector * moveSpeed * Time.deltaTime), .25f, true);
                Vector3 myPosition = transform.position;
                Vector3 myMovement = inputHandler.MoveVector.normalized * gridLength;
                Vector2 targetPosition = new(myPosition.x + myMovement.x, myPosition.y + myMovement.y);


                //Debug.Log($"moving from {myPosition} by {myMovement}, target is {targetPosition}");

                if (CheckDestinationIsWalkable(targetPosition))
                {
                    rb.DOMove(targetPosition, .25f, false).OnComplete(AlignPositionToGrid);
                }
            }
            //Debug.DrawLine(rb.position, rb.position);
            //Vector3 myPosition = rb.position;
            //rb.MovePosition(myPosition + (moveSpeed * Time.deltaTime * inputHandler.MoveVector));
        }


        private bool CheckDestinationIsWalkable(Vector2 targetPosition)
        {
            TileBase tile = pathWays.GetTile(pathWays.WorldToCell(targetPosition));
            Debug.Log($"tile found at {targetPosition}: {tile?.name}");

            //Bounds bounds = pathWays.GetBoundsLocal(pathWays.WorldToCell(targetPosition));

            return pathWays.HasTile(pathWays.WorldToCell(targetPosition));
        }

        private void AlignPositionToGrid()
        {
            Debug.Log($"Aligning position");
            rb.DOMove(rb.position, .01f, true);
        }
    }
}
