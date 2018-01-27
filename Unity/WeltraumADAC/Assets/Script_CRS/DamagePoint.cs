﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    REPAIR_NONE = 0,
    REPAIR_WELD,
    REPAIR_MECHANIC,
    REPAIR_BATTERY,
    REPAIR_BERYLLIUM,
    REPAIR_FLUX,
}

public class DamagePoint : MonoBehaviour {

    static List<DamagePoint> activeDamages = new List<DamagePoint>();

	// Use this for initialization
	void Start () {

        SetDamage(DamageType.REPAIR_WELD);
	}

    private void SetDamage(DamageType dmg)
    {
        Waypoint wp = GetComponent<Waypoint>();
        if (null == wp)
        {
            Debug.LogError("missing <Waypoint> on objec " + name);
        }
        wp.goalType = Waypoint.GoalType.DAMAGE;
        wp.damage = dmg;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Activate()
    {
        DamageType dmg = DamageType.REPAIR_NONE + Random.Range(1, 6);
        SetDamage(dmg);

        //Debug.Log("Activate DamagePoint: " + name);
        int numActive = activeDamages.Count;
        int sym = numActive % 4;
        int damageSet = numActive / 4;

        activeDamages.Add(this);

        Selectable selectable = GetComponent<Selectable>();
        selectable.symbol = (Symbol) sym;
        selectable.group = (SelectGroup) (SelectGroup.DAMAGE_1 + damageSet);
    }

    public void Deactivate()
    {
        //Debug.Log("Deactivate DamagePoint: " + name);
        activeDamages.Remove(this);

        Selectable selectable = GetComponent<Selectable>();
        selectable.symbol = Symbol.None;
        selectable.group = SelectGroup.DAMAGE_4;
    }
}
