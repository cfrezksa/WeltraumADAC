using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		targetPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 dist = (targetPos - this.transform.position) ;
        float d = dist.magnitude;
        if (d > 0.01)
        {
            this.transform.position = targetPos;
        }
        else
        {
            dist = dist.normalized * Time.deltaTime;
            this.transform.position += dist;
        }
	}

    Vector3 targetPos;
    public void setTarget(Vector3 pos) { targetPos = pos; }
}


