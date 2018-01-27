using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour {

	// Use this for initialization
	void Start () {

       DamagePoint[] damages = FindObjectsOfType<DamagePoint>();
       for (int i = 0; i < 2; i++)
       {
           DamagePoint dmg = damages[Random.Range(0, damages.Length)];
           dmg.Activate();
       }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
