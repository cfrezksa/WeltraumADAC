using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotControl : MonoBehaviour {

    Vector3 targetPos;
    public void SetTarget(Waypoint targetWP) {
        Movement move = GetComponent<Movement>();
        if (null != move)
        {
            move.setTargetWaypoint(targetWP);
        }
    }
}


