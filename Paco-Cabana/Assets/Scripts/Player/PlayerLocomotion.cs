using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.Tilemaps;

namespace he
{
    public class PlayerLocomotion : MonoBehaviour
    {

        [SerializeField] float moveSpeed = 1f;
        [SerializeField] float gridLength = 1f;
        [SerializeField] Tilemap pathWays;
        [SerializeField] Tilemap walkabilityMap;

        [SerializeField] Tile pathWalkable;
        [SerializeField] Tile pathBlocked;
        [SerializeField] Tile pathExtra;

        [SerializeField] LayerMask layerMask;


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

                if (CheckDestinationIsWalkableAndPushCrate(targetPosition))
                {
                    rb.DOMove(targetPosition, .25f, false).OnComplete(AlignPositionToGrid);
                }
            }
        }


        private bool CheckDestinationIsWalkableAndPushCrate(Vector2 targetPosition)
        {

            TileBase tile = walkabilityMap.GetTile(walkabilityMap.WorldToCell(targetPosition));
        
            //Debug.Log($"tile found at {targetPosition}: {tile?.name}");
            if (tile.name == pathWalkable.name)
            {
                return true;
            }
            else
            if (tile.name == pathBlocked.name)
            {
                return false;
            }
            else
            {
                // case: pathExtra
                //Debug.DrawRay(targetPosition, inputHandler.MoveVector, Color.magenta, 5f);
                RaycastHit2D hit = Physics2D.CircleCast(targetPosition, .5f, inputHandler.MoveVector, 1f, layerMask);

                //Debug.Log($"hit: {hit.transform?.name}, unhit: {unhit.transform?.name}");
                if (hit)
                {
                    //Debug.Log($"Ouch! {hit.transform.name}... LM: {layerMask.value}");
                    Transform hitObject = hit.transform.parent;

                    if (hitObject.TryGetComponent<CrateMovementHandler>(out CrateMovementHandler cmhComponent))
                    {
                        //Debug.Log($"cmh {cmhComponent.name}");
                        Vector3 myMovement = inputHandler.MoveVector.normalized * gridLength;
                        Vector2 targetTargetPosition = new(targetPosition.x + myMovement.x, targetPosition.y + myMovement.y);
                        //Debug.Log($"probing ttp {targetTargetPosition}");
                        if (cmhComponent.CheckDestinationCellIsValid(walkabilityMap, targetTargetPosition))
                        {
                            return cmhComponent.PushCrate(targetTargetPosition);
                        }
                        else
                        {
                            return false;
                        }

                    }
                }

                return true;
            }

            //return pathWays.HasTile(pathWays.WorldToCell(targetPosition));
        }

        private void AlignPositionToGrid()
        {
            //Debug.Log($"Aligning position");
            rb.DOMove(rb.position, .01f, true);
        }
    }
}
