using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour {

	// Use this for initialization
	void Start () {

       DamagePoint[] damages = FindObjectsOfType<DamagePoint>();
       List<DamagePoint> listDamages = new List<DamagePoint>();

       foreach (var d in damages)
       {
           d.Deactivate();
           listDamages.Add(d);
       }

       for (int i = 0; i < 4; i++)
       {
           int index = Random.Range(0, listDamages.Count);
           DamagePoint dmg = listDamages[index];
           dmg.Activate();
           listDamages.Remove(dmg);
       }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
