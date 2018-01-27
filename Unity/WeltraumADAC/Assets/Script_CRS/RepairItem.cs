using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairItem : MonoBehaviour {

    public DamageType damageType;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FloatToSpace()
    {
        this.transform.parent = null;
        Destroy(this.gameObject);

    }
}
