using UnityEngine;
using System.Collections;

public class Player_PathController : PathController {

    void Awake()
    {
        PathFoundCB = OnPathFound;
        StopMovementCB = StopMovement;
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccesful)
    {
        if (pathSuccesful)
        {
            Debug.Log("Found Path!");
            // Move
            path = newPath;
            StartCoroutine("FollowPath");
        }
        else
        {
            Debug.Log("Could not get path to " + currPathTarget);
        }
    }


    IEnumerator FollowPath()
    {
        Vector3 currWayPoint = path[0];
        Debug.Log("Path: Current Waypoint is " + currWayPoint);
        while (true)
        {
            // Movement is limited to movement points available
            if (CurrMovePoints > 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, currWayPoint, 2f * Time.deltaTime);
            }
            else
            {
                // No Movement Points left on this action

                ResetMovementRange();

                TargetIndex = 0;
                path = null;

                // End this move action
                Player_BattleController.Instance.EndAction();

                yield break;
            }


            if (transform.position == currWayPoint)
            {
                TargetIndex++;

                // Once we reach the next waypoint, subtract from movement points
                CurrMovePoints--;

                if (TargetIndex >= path.Length)
                {
                    // Reached end of path, reset move points
                    ResetMovementRange();

                    TargetIndex = 0;
                    path = null;

                    // End this move action
                    Player_BattleController.Instance.EndAction();

                    yield break;
                }

                currWayPoint = path[TargetIndex];

            }




            yield return null;
        }

    }

    public void StopMovement()
    {
        StopCoroutine("FollowPath");
        TargetIndex = 0;
    }
}
