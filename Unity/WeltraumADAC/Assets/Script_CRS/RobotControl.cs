using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RobotControl : MonoBehaviour {

    const float EPSILON = 0.5f;
    Waypoint goal = null;
    Vector3 targetPos;
    RepairItem carriedItem;
    float workingTime = 0.0f;
    float totalWorkingTime = 1.0f;
    const float totalRepairTime = 5.0f;
    const float totalDamagingTime = 10.0f;

    enum RobotState
    {
        MOVE_TO_GOAL,
        MOVE_TO_DESTROY,
        MAKING_DAMAGE,
        MAKING_REPAIR,
    }

    RobotState state = RobotState.MOVE_TO_GOAL;
    public void SetTarget(Waypoint targetWP, bool isConfused = false) {

        state = (isConfused) ? RobotState.MOVE_TO_DESTROY : RobotState.MOVE_TO_GOAL;

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

            switch (state) {
                case RobotState.MAKING_DAMAGE:
                case RobotState.MAKING_REPAIR:
                    DoWork();
                    break;
                case RobotState.MOVE_TO_DESTROY:
                case RobotState.MOVE_TO_GOAL: 
                  MoveToTarget();
                  break;
            }
            
        }
    }



    private void DoWork()
    {
        workingTime += Time.deltaTime;
        if (workingTime > totalWorkingTime)
        {
            ActionFinished();
        }
    }
   public void HighlightRobot()
    {
        Colorize(Color.red);
    }

    public void UnhighlightRobot()
    {
        Colorize(Color.white);
    }

    private void Colorize(Color col)
    {
        var renderers = this.GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            r.material.color = col;
        }
    }

    private void MoveToTarget()
    {
        float dist = (this.transform.position - goal.transform.position).magnitude;
        if (dist < EPSILON)
        {
            workingTime = 0.0f;

            switch (state)
            {
                case RobotState.MOVE_TO_DESTROY:
                    totalWorkingTime = totalDamagingTime;
                    state = RobotState.MAKING_DAMAGE;
                    break;
                case RobotState.MOVE_TO_GOAL:
                    totalWorkingTime = totalRepairTime;
                    state = RobotState.MAKING_REPAIR;
                    break;

            }
        }
    }

    void ActionFinished()
    {

        Debug.Log("ExecuteAction()");
        switch (state)
        {
            case RobotState.MAKING_DAMAGE:
                FinishDamageAction();
                break;
            case RobotState.MAKING_REPAIR:
                FinishRegularAction();
                break;
        }

    }

    private void FinishRegularAction()
    {
        Debug.Log("Regular Action");
        switch (goal.goalType)
        {
            case Waypoint.GoalType.CONTAINER:
                FinishContainerAction();
                break;
            case Waypoint.GoalType.DAMAGE:
                FinishRepairAction();
                break;
            default:
                break;
        }
    }

    private void FinishDamageAction()
    {
        Debug.Log("Robot " + name + " executing damage action!");
        DamagePoint dp = goal.GetComponent<DamagePoint>();
        if (null == dp) Debug.LogError("not a ContainerPoint object!");

        dp.Activate();

        BeConfused();
    }


    private void FinishRepairAction() {
    
        Debug.Log("Robot " + name + " executing repair action!");
        DamagePoint dp = goal.GetComponent<DamagePoint>();
        if (null == dp) Debug.LogError("not a ContainerPoint object!");

        Debug.Log("Goal Damage = " + goal.damage);
        switch (goal.damage)
        {
            case DamageType.REPAIR_NONE:
                BeConfused();
                break;
            case DamageType.REPAIR_WELD:
                if (carriedItem != null)
                {
                    BeConfused();
                }
                else
                {
                    dp.Deactivate();
                    goal = null;
                }
                break;
            case DamageType.REPAIR_MECHANIC:
            case DamageType.REPAIR_BATTERY:
            case DamageType.REPAIR_BERYLLIUM:
            case DamageType.REPAIR_FLUX:
                if (carriedItem == null)
                {
                    BeConfused();
                }
                else
                {
                    if (carriedItem.damageType == goal.damage)
                    {
                        dp.Deactivate();
                        goal = null;
                    }
                    else
                    {
                        BeConfused();
                    }
                }
                break;
        }
    }

    void BeConfused()
    {
        Colorize(Color.red);
        Debug.Log("Robot " + name + " is now confused!");

        if (carriedItem != null)
        {
            carriedItem.FloatToSpace();
            carriedItem = null;
        }

        DamagePoint p = FindUnusedDamagePoint();
        if (null == p)
        {
            Debug.Log("no more damage points!");
            return;
        }

        Waypoint wp = p.GetComponent<Waypoint>();
        SetTarget(wp, true);

    }

    private static DamagePoint FindUnusedDamagePoint()
    {
        List<DamagePoint> unusedPoints = FindAllUnusedDamagePoints();

        if (unusedPoints.Count == 0)
        {
            Debug.Log("No unused damaged points!");
            return null;
        }

        int index = Random.Range(0, unusedPoints.Count);
        DamagePoint p = unusedPoints[index];
        return p;
    }

    private static List<DamagePoint> FindAllUnusedDamagePoints()
    {
        DamagePoint[] dmgPoints = FindObjectsOfType<DamagePoint>();
        List<DamagePoint> unusedPoints = new List<DamagePoint>();

        foreach (DamagePoint dmg in dmgPoints)
        {
            if (!dmg.IsActive())
            {
                unusedPoints.Add(dmg);
            }
        }
        return unusedPoints;
    }

    private void FinishContainerAction()
    {
        Debug.Log("Robot " + name + " executing container action!");
        ContainerPoint cp = goal.GetComponent<ContainerPoint>();
        if (null == cp) Debug.LogError("not a ContainerPoint object!");

        if (cp.prefab == null) return;

        GameObject item = Instantiate(cp.prefab, this.transform);
        carriedItem = item.GetComponent<RepairItem>();
    }
}


