using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonIcon : MonoBehaviour {

    float localTime = 0.0f;
    float bounceAmount = 0.2f;
    float bounceFreq = 5.0f;
    Vector3 origPosition;
    void Start()
    {
        origPosition = this.transform.position;
    }
	void Update () {

        localTime += Time.deltaTime;
        this.transform.position = origPosition + bounceAmount * Mathf.Abs(Mathf.Sin(bounceFreq * localTime)) * Vector3.up;
        if (Camera.current != null)
        {
            transform.LookAt(transform.position + Camera.current.transform.rotation * Vector3.forward,
               Camera.current.transform.rotation * Vector3.up);
        }
		
	}
}
