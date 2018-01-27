using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    Waypoint[] waypoints;
    public float velocity = 0.1f;
    Waypoint waypoint;
    Vector3 start;
    Vector3 finish;

	// Use this for initialization
	void Start () {

        start = transform.position;

        waypoints = FindObjectsOfType<Waypoint>();

        waypoint = waypoints[16];
        waypoint.GetComponent<Renderer>().material.color = Color.red;

        finish = waypoint.transform.position;

    }
	
	// Update is called once per frame
	void Update () {

            var path = finish - transform.position;
            var distance = velocity * Time.deltaTime;

            if(path.magnitude <= distance)
            {
                Debug.Log("jump to finish");
                transform.position = finish;
            } else
            {
                Debug.Log("Move");
                var direction = path.normalized;
                transform.position += distance * direction;
            }
	}
}
