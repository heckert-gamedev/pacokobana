using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

namespace he
{
    //  While they are supposed to be SPA guests (Pacmans), from a game-mechanic perspective they're the
    //  classical Sokoban crates (with regard to movement in the first iteration of game-play, anyway),
    //  so for the sake of simplicity let's call them "Crates" for the moment...
    //
    public class CrateMovementHandler : MonoBehaviour
    {

        [SerializeField] bool isPushable = true;
        [SerializeField] bool isPullable = false;

        [SerializeField] Tile tileEnablesPushing;
        [SerializeField] Tile tileTargetField;

        [SerializeField] LayerMask targetsLayerMask;
        [SerializeField] LayerMask cratesLayerMask;

        public event Action ArrivalAction;


        public bool CheckDestinationCellIsValid(Tilemap map, Vector2 destinationGridPosition) //Vector3 pushDirection, Vector3 callerGridPosition)
        {

            Vector2 pushVector = (destinationGridPosition - new Vector2(transform.position.x, transform.position.y)).normalized;

            //Debug.Log($"check cell with pushVector {pushVector}");
            //Debug.DrawRay(transform.position, pushVector, Color.blue, 5f);

            //Debug.DrawRay(destinationGridPosition, pushVector, Color.cyan, 5f);
            RaycastHit2D hit = Physics2D.Raycast(destinationGridPosition, pushVector, .5f, cratesLayerMask);

            if (hit)
            {
                //Debug.Log($"{name} check on {hit.transform?.name}");
                return false;
            }
            else
            {
                Debug.Log($"{name} check no hit");
            }


            TileBase tile = map.GetTile(map.WorldToCell(destinationGridPosition));
            if (tile.name == tileEnablesPushing.name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool PushCrate(Vector2 targetPosition)
        {
            if (DOTween.IsTweening(transform)) return false;

            if (isPushable)
            {

                transform.DOMove(targetPosition, .20f, false);

                Collider2D collider2D = Physics2D.OverlapCircle(targetPosition, 1.1f, targetsLayerMask);

                //Debug.Log($"PushCrate: collider found: '{collider2D?.transform.name}' ... {collider2D?.transform.position}");
                if (collider2D)
                {
                    Vector2 targetEndPosition = new(collider2D.transform.position.x, collider2D.transform.position.y);
                    //Debug.Log($"**PUSH OFF**");
                    isPushable = false;
                    transform.DOMove(targetEndPosition, 1.2f, false);
                    collider2D.enabled = false;

                    ArrivalAction?.Invoke();
                }

                return true;
            }
            else return false;
        }

    }
}
