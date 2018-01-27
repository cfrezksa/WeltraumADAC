using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

    public Waypoint[] connections;


	// Use this for initialization
	void Start () {
		
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
}
