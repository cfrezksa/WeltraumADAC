using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

    public enum GoalType
    {
        NONE,
        DAMAGE,
        CONTAINER,
    }

    public GoalType goalType = GoalType.NONE;
    public DamageType damage = DamageType.REPAIR_NONE;
    public Waypoint[] connections;


	// Use this for initialization
	void Start () {
        Renderer r = GetComponent<Renderer>();
        if (null != r) r.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

        foreach(var wp in connections)
        {
            if(wp != null)
            {
                Debug.DrawLine(wp.transform.position, this.transform.position, Color.red);
            }
        }
		
	}

    public void Reached(GameObject robot)
    {

    }
}
