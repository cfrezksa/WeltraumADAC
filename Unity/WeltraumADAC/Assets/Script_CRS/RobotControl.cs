using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RobotControl : MonoBehaviour {

    const float EPSILON = 0.1f;
    Waypoint goal = null;
    Vector3 targetPos;
    RepairItem carriedItem;

    public void SetTarget(Waypoint targetWP) {
        goal = targetWP;

        Movement move = GetComponent<Movement>();
        if (null != move)
        {
            move.setTargetWaypoint(targetWP);
        }
    }

    void Update()
    {
        if (null != goal)
        {

            float dist = (this.transform.position - goal.transform.position).magnitude;
            if (dist < EPSILON)
            {
                ExecuteAction();
            }
        }
    }

    void ExecuteAction()
    {
        switch (goal.goalType) { 
            case Waypoint.GoalType.CONTAINER:
                ExecuteContainerAction();
                break;

            case Waypoint.GoalType.DAMAGE:
                ExecuteRepairAction();
                break;
            default: 
            break;
        }

        goal = null;
    }

    private void ExecuteRepairAction()
    {
        DamagePoint dp = goal.GetComponent<DamagePoint>();
        if (null == dp) Debug.LogError("not a ContainerPoint object!");

        switch (goal.damage)
        {
            case DamageType.REPAIR_NONE:
            case DamageType.REPAIR_WELD:
                if (carriedItem != null)
                {
                    carriedItem.FloatToSpace();
                    carriedItem = null;
                    BeConfused();
                }
                break;
            case DamageType.REPAIR_MECHANIC:
            case DamageType.REPAIR_BATTERY:
            case DamageType.REPAIR_BERYLLIUM:
            case DamageType.REPAIR_FLUX:
                if (carriedItem.damageType == goal.damage)
                {
                    dp.Deactivate();
                }
                else
                {
                    carriedItem.FloatToSpace();
                    carriedItem = null;
                    BeConfused();
                }
                break;
        }
    }

    void BeConfused()
    {

    }

    private void ExecuteContainerAction()
    {
        Debug.Log("Robot " + name + " executing container action!");
        ContainerPoint cp = goal.GetComponent<ContainerPoint>();
        if (null == cp) Debug.LogError("not a ContainerPoint object!");

        if (cp.prefab == null) return;

        GameObject item = Instantiate(cp.prefab, this.transform);
        carriedItem = item.GetComponent<RepairItem>();
    }
}


