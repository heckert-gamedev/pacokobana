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
                Vector3 myPosition = rb.position;
                Vector3 myMovement = inputHandler.MoveVector.normalized * gridLength;
                Vector2 targetPosition = new(myPosition.x + myMovement.x, myPosition.y + myMovement.y);

                if (CheckDestinationIsWalkable(targetPosition))
                {
                    rb.DOMove(targetPosition, .25f, false).OnComplete(AlignPositionToGrid);
                }
            }
        }


        private bool CheckDestinationIsWalkable(Vector2 targetPosition)
        {
            TileBase tile = pathWays.GetTile(pathWays.WorldToCell(targetPosition));
            Debug.Log($"tile found at {targetPosition}: {tile?.name}");

            return pathWays.HasTile(pathWays.WorldToCell(targetPosition));
        }

        private void AlignPositionToGrid()
        {
            Debug.Log($"Aligning position");
            rb.DOMove(rb.position, .01f, true);
        }
    }
}
