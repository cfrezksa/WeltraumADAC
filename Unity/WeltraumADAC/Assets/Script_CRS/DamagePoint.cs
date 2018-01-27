using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    REPAIR_NONE,
    REPAIR_WELD,
    REPAIR_MECHANIC,
    REPAIR_BATTERY,
    REPAIR_BERYLLIUM,
    REPAIR_FLUX,
}

public class DamagePoint : MonoBehaviour {

    static List<DamagePoint> activeDamages = new List<DamagePoint>();

    Selectable selectable = null;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Activate()
    {
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
