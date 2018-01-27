using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectGroup
{
    ROBOT,
    CONTAINER,
    DAMAGE_1,
    DAMAGE_2,
    DAMAGE_3,
    DAMAGE_4,
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

        bool symbolVisible = false;
        if (activeGroup == SelectGroup.ROBOT)
        {
            PlayerMove player = FindObjectOfType<PlayerMove>();
            if (player.IsRobotVisible(this.transform.position)) {
                 symbolVisible = (group == activeGroup) && (symbol != Symbol.None);
            }
        } else {       
            symbolVisible = (group == activeGroup) && (symbol != Symbol.None);
        }
        icon.gameObject.SetActive(symbolVisible);
        if (symbolVisible)
        {
            icon.GetComponent<Renderer>().material = buttonMaterials[(int)symbol];
        }
	}

    public static SelectGroup activeGroup = SelectGroup.ROBOT;
    public Material[] buttonMaterials;
}
