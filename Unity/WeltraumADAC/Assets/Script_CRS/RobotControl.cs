using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RobotControl : MonoBehaviour {

    public GameObject confusedSystem;
    const float EPSILON = 0.5f;
    Waypoint goal = null;
    Vector3 targetPos;
    RepairItem carriedItem;
    float workingTime = 0.0f;
    float totalWorkingTime = 1.0f;
    const float totalRepairTime = 5.0f;
    const float totalContainerTime = 3.0f;
    const float totalDamagingTime = 10.0f;

    enum Speech
    {
        DAMAGE_DETECTED = 0,
        BERYLLIUMKUGEL,
        FLUX_COMPENSATOR,
        WERKZEUG,
        BATTERIE,
        BERYLLIUMKUGEL_ERHALTEN,
        FLUX_COMPENSATOR_ERHALTEN,
        WERKZEUG_ERHALTEN,
        BATTERIE_ERHALTEN,
        BERYLLIUMKUGEL_INSTALLIERT,
        FLUX_COMPENSATOR_INSTALLIERT,
        WERKZEUG_INSTALLIERT,
        BATTERIE_INSTALLIERT,
        SCHWEISSGERAET,
        KAPUTT_GEMACHT,
        I_AM_CONFUSED,
        BEFEHL_ERHALTEN,
        BEREIT_FUER_BEFEHLE,
        OUCH
    }
    public AudioClip[] speech;

    enum RobotState
    {
        IDLE,
        MOVE_TO_CONTAINER,
        WAITING_FOR_CONTAINER,
        MOVE_TO_DAMAGE,
        MOVE_TO_DESTROY,
        MAKING_DAMAGE,
        MAKING_REPAIR,
    }


    enum AnimState
    {
        IDLE,
        STUNNED,
        MOVE,
        PICKUP,
        THROW,
        WELD,
        SCREW,
    }

    RobotState state = RobotState.IDLE;
    Animator animator;

    void PlaySound(Speech s)
    {
        int soundIndex = (int)s;
        if (soundIndex >= speech.Length)
        {
            Debug.Log("Missing sound: " + s);
            return;
        }
        AudioClip clip = speech[soundIndex];
        var audiosource = GetComponent<AudioSource>();
        audiosource.PlayOneShot(clip);
    }

    public void SetTarget(Waypoint targetWP, bool isConfused = false) {

        PlaySound( (isConfused)?  Speech.I_AM_CONFUSED : Speech.BEFEHL_ERHALTEN);

        if (null != confusedSystem)
        {
            confusedSystem.SetActive(isConfused);
        }
        
        bool isTargetDamage = (targetWP.GetComponent<DamagePoint>() != null);

        state = (isConfused) ? RobotState.MOVE_TO_DESTROY : (isTargetDamage) ? RobotState.MOVE_TO_DAMAGE : RobotState.MOVE_TO_CONTAINER;

        goal = targetWP;

        Movement move = GetComponent<Movement>();
        if (null != move)
        {
            move.setTargetWaypoint(targetWP);
        }

        SetAnimState(AnimState.MOVE);
    }

    void Start()
    {
        if (null != confusedSystem)
        {
            confusedSystem.SetActive(false);
        }
    }

    void Update()
    {
        if (null != goal)
        {

            switch (state) {
                case RobotState.MAKING_DAMAGE:
                case RobotState.MAKING_REPAIR:
                case RobotState.WAITING_FOR_CONTAINER:
                    DoWork();
                    break;
                case RobotState.MOVE_TO_DESTROY:
                case RobotState.MOVE_TO_DAMAGE:
                case RobotState.MOVE_TO_CONTAINER: 
                  MoveToTarget();
                  break;
            }
            
        }
    }

    AnimState animState = AnimState.IDLE;
    void SetAnimState(AnimState newState)
    {
        if (animState == newState) return;

        Animator anim = GetComponentInChildren<Animator>();
        if (null == anim) Debug.LogError("Missing Animator");

        switch (newState)
        {
            case AnimState.IDLE:
                anim.SetTrigger("idle");
                break;
            case AnimState.MOVE:
                anim.SetTrigger("move");
                break;
            case AnimState.STUNNED:
                anim.SetTrigger("stunned");
                break;
            case AnimState.PICKUP:
                anim.SetTrigger("pickup");
                break;
            case AnimState.THROW:
                anim.SetTrigger("throw");
                break;
            case AnimState.WELD:
                anim.SetTrigger("weld");
                break;
            case AnimState.SCREW:
                anim.SetTrigger("screw");
                break;
        }

        animState = newState;
    }
    private void DoWork()
    {
        // hier unterschiede machen
        SetAnimState(AnimState.WELD);


        workingTime += Time.deltaTime;
        if (workingTime > totalWorkingTime)
        {
            ActionFinished();
        }
    }
   public void HighlightRobot()
    {
        PlaySound(Speech.BEREIT_FUER_BEFEHLE);
        //Colorize(Color.green);
    }

    public void UnhighlightRobot()
    {
        //Colorize(Color.white);
    }

    /*
    private void Colorize(Color col)
    {
        var renderers = this.GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            r.material.color = col;
        }
    }
    */
    private void MoveToTarget()
    {
        float dist = (this.transform.position - goal.transform.position).magnitude;
        if (dist < EPSILON)
        {
            workingTime = 0.0f;

            switch (state)
            {
                case RobotState.MOVE_TO_DESTROY:
                    SetAnimState(AnimState.WELD);
                    totalWorkingTime = totalDamagingTime;
                    state = RobotState.MAKING_DAMAGE;
                    break;
                case RobotState.MOVE_TO_DAMAGE:
                    totalWorkingTime = totalRepairTime;
                    state = RobotState.MAKING_REPAIR;
                    SetAnimState(AnimState.SCREW);
                    break;
                case RobotState.MOVE_TO_CONTAINER:
                    StartContainerAction();
                    totalWorkingTime = totalContainerTime;
                    state = RobotState.WAITING_FOR_CONTAINER;
                    SetAnimState(AnimState.PICKUP);
                    break;

            }
        }
    }

    void ActionFinished()
    {

        //Debug.Log("ExecuteAction()");
        switch (state)
        {
            case RobotState.MAKING_DAMAGE:
                FinishDamageAction();
                break;
            case RobotState.MAKING_REPAIR:
                FinishRepairAction();
                break;
            case RobotState.WAITING_FOR_CONTAINER:
                FinishContainerAction();
                break;
        }

    }
    
    private void FinishDamageAction()
    {
        PlaySound(Speech.KAPUTT_GEMACHT);

        //Debug.Log("Robot " + name + " executing damage action!");
        DamagePoint dp = goal.GetComponent<DamagePoint>();
        if (null == dp) Debug.LogError("not a ContainerPoint object!");
        dp.Activate();

        BeConfused();
        SetAnimState(AnimState.STUNNED);
    }


    private void FinishRepairAction() {
    
        //Debug.Log("Robot " + name + " executing repair action!");
        DamagePoint dp = goal.GetComponent<DamagePoint>();
        if (null == dp) Debug.LogError("not a ContainerPoint object!");

        //Debug.Log("Goal Damage = " + goal.damage);
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
                    RepairDone(dp);
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
                        RepairDone(dp);
                    }
                    else
                    {
                        BeConfused();
                    }
                }
                break;
        }
    }

    private void RepairDone(DamagePoint dp)
    {
        if (carriedItem != null)
        {
            DestroyImmediate(carriedItem.gameObject);
            carriedItem = null;
        }

        Waypoint wp = dp.GetComponent<Waypoint>();
        switch (wp.damage)
        {
            case DamageType.REPAIR_WELD:
                PlaySound(Speech.SCHWEISSGERAET);
                break;
            case DamageType.REPAIR_MECHANIC:
                PlaySound(Speech.WERKZEUG_INSTALLIERT);
                break;
            case DamageType.REPAIR_BATTERY:
                PlaySound(Speech.BATTERIE_INSTALLIERT);
                break;
            case DamageType.REPAIR_BERYLLIUM:
                PlaySound(Speech.BERYLLIUMKUGEL_INSTALLIERT);
                break;
            case DamageType.REPAIR_FLUX:
                PlaySound(Speech.FLUX_COMPENSATOR_INSTALLIERT);
                break;
            default:
            case DamageType.REPAIR_NONE:
                break;
        }
        
        dp.Deactivate();
        state = RobotState.IDLE;
        goal = null;
    }
   
    void BeConfused()
    {
        //PlaySound(Speech.I_AM_CONFUSED);

        if (carriedItem != null)
        {
            SetAnimState(AnimState.THROW);
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

    private void StartContainerAction() {

        ContainerPoint cp = goal.GetComponent<ContainerPoint>();
        cp.Open();
        if (null == cp) Debug.LogError("not a ContainerPoint object!");

        if (cp.prefab == null) return;

        if (carriedItem != null)
        {
            DestroyImmediate(carriedItem.gameObject);
            carriedItem = null;
        }

        Waypoint wp = goal.GetComponent<Waypoint>();
        switch (wp.damage)
        {
            case DamageType.REPAIR_MECHANIC:
                PlaySound(Speech.WERKZEUG);
                break;
            case DamageType.REPAIR_BATTERY:
                PlaySound(Speech.BATTERIE);
                break;
            case DamageType.REPAIR_BERYLLIUM:
                PlaySound(Speech.BERYLLIUMKUGEL);
                break;
            case DamageType.REPAIR_FLUX:
                PlaySound(Speech.FLUX_COMPENSATOR);
                break;
            default:
            case DamageType.REPAIR_WELD:
            case DamageType.REPAIR_NONE:
                break;
        }
    }

    private void FinishContainerAction()
    {
        ContainerPoint cp = goal.GetComponent<ContainerPoint>();
        cp.Close();
        if (null == cp) Debug.LogError("not a ContainerPoint object!");

        if (cp.prefab == null) return;

        if (carriedItem != null)
        {
            DestroyImmediate(carriedItem.gameObject);
            carriedItem = null;
        }

        Waypoint wp = goal.GetComponent<Waypoint>();
        switch (wp.damage)
        {
            case DamageType.REPAIR_MECHANIC:
                PlaySound(Speech.WERKZEUG_ERHALTEN);
                break;
            case DamageType.REPAIR_BATTERY:
                PlaySound(Speech.BATTERIE_ERHALTEN);
                break;
            case DamageType.REPAIR_BERYLLIUM:
                PlaySound(Speech.BERYLLIUMKUGEL_ERHALTEN);
                break;
            case DamageType.REPAIR_FLUX:
                PlaySound(Speech.FLUX_COMPENSATOR_ERHALTEN);
                break;
            default:
            case DamageType.REPAIR_WELD:
            case DamageType.REPAIR_NONE:
                break;
        }


        GameObject item = Instantiate(cp.prefab, this.transform);
        carriedItem = item.GetComponent<RepairItem>();
        
        state = RobotState.MOVE_TO_DAMAGE;
        goal = null;
    }
}


