using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairItem : MonoBehaviour {

    public DamageType damageType;
    public float velocity = 2f;
    public float maxHeight = 10f;
    public float rotationSpeed = 0.3f;
    private bool shouldFloat = false;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
		if(shouldFloat)
        {
            // move with given velocity along y-axis (up)
            transform.position += new Vector3(0, velocity * Time.deltaTime, 0);

            transform.Rotate(new Vector3(0,rotationSpeed*360*Time.deltaTime,0));

            // if maximum height is reached, destroy object
            if(transform.position.y > maxHeight)
            {
                Destroy(this.gameObject);
            }
        }
	}

    public void FloatToSpace()
    {
        this.transform.parent = null;
        this.shouldFloat = true;

    }
}
