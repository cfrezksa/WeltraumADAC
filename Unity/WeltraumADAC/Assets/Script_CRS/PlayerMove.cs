using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public float range = 5.0f;
    public float velocity = 0.1f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 newPos = MoveByInput();

        this.transform.position = newPos;

        UpdateRobots();
		
	}

    private Vector3 MoveByInput()
    {
        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(-moveY, 0.0f, moveX);

        Vector3 newPos = transform.position + velocity * move;
        if (newPos.x < -10.0) newPos.x = -10.0f;
        if (newPos.x > 10.0) newPos.x = 10.0f;
        if (newPos.z < -10.0) newPos.z = -10.0f;
        if (newPos.z > 10.0) newPos.z = 10.0f;
        return newPos;
    }

    void OnCollisionEnter(Collision other)
    {
        //this.transform.position = prevPos;
        //Debug.Log("Collision!");
    }

    private void UpdateRobots()
    {
        RobotControl[] robots = FindObjectsOfType<RobotControl>();
        foreach (var rob in robots)
        {
            Vector3 robotPos = rob.transform.position;
           
            if (IsRobotVisible(robotPos)){          
                Debug.DrawLine(this.transform.position, robotPos, Color.blue);
            }
        }
    }

    public bool IsRobotVisible(Vector3 robotPos)
    {
        Vector3 playerPos = this.transform.position;
        float dist = (robotPos - playerPos).magnitude;
        return (dist < range);
    }
}
