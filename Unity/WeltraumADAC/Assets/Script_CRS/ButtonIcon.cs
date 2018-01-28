using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonIcon : MonoBehaviour {

    float localTime = 0.0f;
    public float bounceAmount = 0.2f;
    public float bounceFreq = 5.0f;
    Vector3 basePosition;
    void Start()
    {
        basePosition = this.transform.localPosition;
    }
	void Update () {

        localTime += Time.deltaTime;
        basePosition.x = this.transform.localPosition.x;
        basePosition.z = this.transform.localPosition.z;
        this.transform.localPosition = basePosition + bounceAmount * Mathf.Abs(Mathf.Sin(bounceFreq * localTime)) * Vector3.up;
        if (Camera.current != null)
        {
            transform.LookAt(transform.position + Camera.current.transform.rotation * Vector3.forward,
               Camera.current.transform.rotation * Vector3.up);
        }
		
	}
}
