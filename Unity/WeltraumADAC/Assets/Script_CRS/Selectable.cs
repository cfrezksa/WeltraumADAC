using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectGroup
{
    ROBOT,
    DAMAGE,
    CONTAINER,
}

public enum Symbol
{
    A,
    B,
    X,
    Y,
    None
};
public class Selectable : MonoBehaviour {

    public SelectGroup group;
    public Symbol symbol;

    public GameObject icon;
		
	// Update is called once per frame
	void Update () {
        icon.gameObject.SetActive(group == activeGroup);
        icon.GetComponent<Renderer>().material = buttonMaterials[(int)symbol];
	}

    public static SelectGroup activeGroup = SelectGroup.ROBOT;
    public Material[] buttonMaterials;
}
