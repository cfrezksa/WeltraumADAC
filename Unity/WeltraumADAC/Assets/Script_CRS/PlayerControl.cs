using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
        Selectable.activeGroup = SelectGroup.ROBOT;
	}
	
	// Update is called once per frame
	void Update () {

        UpdateActiveGroup();

        Symbol button = GetSymbol();

        if (button != Symbol.None)
        {
            Selectable sel = GetSelected(button);

            switch (Selectable.activeGroup)
            {
                case SelectGroup.ROBOT:
                    SelectRobot(sel);
                    break;
                case SelectGroup.CONTAINER:
                    SelectContainer(sel);
                    break;
                case SelectGroup.DAMAGE:
                    SelectDamage(sel);
                    break;
            }
        }
	}

    RobotControl robot = null;
    void SelectRobot(Selectable s)
    {
        Debug.Log("Robot '" + s.name + "' was selected!");
        if (robot != null)
        {
            robot.GetComponent<Renderer>().material.color = Color.white;
        }
        s.GetComponent<Renderer>().material.color = Color.blue;
        robot = s.GetComponent<RobotControl>();
        
    }

    void SelectContainer(Selectable s)
    {
        Debug.Log("Container '" + s.name + "' was selected!");
        if (null != robot)
        {
            robot.setTarget(s.transform.position);
        }
    }

    void SelectDamage(Selectable s)
    {
        Debug.Log("Damage '" + s.name + "' was selected!");
        if (null != robot)
        {
            robot.setTarget(s.transform.position);
        }
    }

    private static Selectable GetSelected(Symbol button)
    {
        Selectable selectedObject = null;
        Selectable[] all = FindObjectsOfType<Selectable>();
        foreach (Selectable a in all)
        {
            if (a.group != Selectable.activeGroup) continue;
            if (a.symbol == button)
            {
                selectedObject = a;
                break;
            }
        }

        return selectedObject;
    }

    private static Symbol GetSymbol()
    {
        Symbol button = Symbol.None;
        if (Input.GetButtonDown("A"))
        {
            button = Symbol.A;
        }
        else if (Input.GetButtonDown("B"))
        {
            button = Symbol.B;
        }
        else if (Input.GetButtonDown("X"))
        {
            button = Symbol.X;
        }
        else if (Input.GetButtonDown("Y"))
        {
            button = Symbol.Y;
        }
        return button;
    }

    private static void UpdateActiveGroup()
    {
        if (Input.GetButton("Sel Damage"))
        {
            Selectable.activeGroup = SelectGroup.DAMAGE;
        }
        else if (Input.GetButton("Sel Container"))
        {
            Selectable.activeGroup = SelectGroup.CONTAINER;
        }
        else
        {
            Selectable.activeGroup = SelectGroup.ROBOT;
        }
    }
}
