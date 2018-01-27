using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public float velocity = 0.1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("Vertical");

        Debug.Log("moveX =" + moveX + " moveY = " + moveY);

        Vector3 move = new Vector3(-moveY, 0.0f, moveX);

        Vector3 newPos = transform.position + velocity * move;
        if (newPos.x < -10.0) newPos.x = -10.0f;
        if (newPos.x > 10.0) newPos.x = 10.0f;
        if (newPos.z < -10.0) newPos.z = -10.0f;
        if (newPos.z > 10.0) newPos.z = 10.0f;
        this.transform.position = newPos;

        RobotControl[] robots = FindObjectsOfType<RobotControl>();
        foreach (var rob in robots) {
            Debug.DrawLine(this.transform.position, rob.transform.position, Color.blue);
        }
		
	}
}
