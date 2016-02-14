using UnityEngine;
using System.Collections;
using System;

public class PathController : MonoBehaviour {
    public Vector3[] path { get; protected set; }

    int targetIndex = 0;
    public int TargetIndex { get { return targetIndex; } set { targetIndex = value; } }

    public Vector3 currPathTarget { get; protected set; }

    int maxMovePoints, curMovePoints;
    public int CurrMovePoints { get { return curMovePoints; } set { curMovePoints = value; } }

    Action GetRangeCB;

    public Action<Vector3[], bool> PathFoundCB { get; protected set; }

    public Action StopMovementCB { get; protected set; }

    public void RegisterGetRangeCallback(Action cb)
    {
        GetRangeCB = cb;
    }

    public void SetRange(int maxPoints)
    {
        maxMovePoints = maxPoints;
    }

    void Start()
    {

        GetRangeCB();

        curMovePoints = maxMovePoints;
    }

    //void Update()
    //{
    //    Debug.Log("Current Move Points: " + curMovePoints);
    //}

    public void RequestPath(Vector3 pathPosition)
    {
        currPathTarget = pathPosition;
        PathRequestManager.RequestPath(transform.position, currPathTarget, gameObject, PathFoundCB);
    }

    public void StopMovement()
    {
        StopMovementCB();
    }


    // Movement Range will need to be reset after each turn
    public void ResetMovementRange()
    {
        curMovePoints = maxMovePoints;
    }

}
