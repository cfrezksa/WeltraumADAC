using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {


    static int damageIndex = 0;
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
            if (null != sel)
            {
                switch (Selectable.activeGroup)
                {
                    case SelectGroup.ROBOT:
                        SelectRobot(sel);
                        break;
                    case SelectGroup.CONTAINER:
                        SelectContainer(sel);
                        break;
                    case SelectGroup.DAMAGE_1:
                    case SelectGroup.DAMAGE_2:
                    case SelectGroup.DAMAGE_3:
                    case SelectGroup.DAMAGE_4:
                        SelectDamage(sel);
                        break;
                }
            }
        }
	}

    static RobotControl selectedRobot = null;
    static void SelectRobot(Selectable s)
    {
        if (null == s)
        {
            Debug.LogError("invalid robot selection");
            return;
        }

        Debug.Log("Robot '" + s.name + "' was selected!");

        if (selectedRobot != null) UnhighlightRobot(selectedRobot);

        selectedRobot = s.GetComponent<RobotControl>();
        if (selectedRobot != null) HighlightRobot(selectedRobot);
        
    }

    static void HighlightRobot(RobotControl robot)
    {
        var renderers = robot.GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            r.material.color = Color.green;
        }
    }

    static void UnhighlightRobot(RobotControl robot)
    {
        var renderers = robot.GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            r.material.color = Color.white;
        }
    }

    static void SelectContainer(Selectable s)
    {
        //Debug.Log("Container '" + s.name + "' was selected!");
        if (null != selectedRobot)
        {
            Waypoint wp = s.GetComponent<Waypoint>();
            if (null == wp) Debug.LogError("Missing <Waypoint> on " + s.name);
            selectedRobot.SetTarget(wp);
            UnhighlightRobot(selectedRobot);
            selectedRobot = null;
        }
    }

    static void SelectDamage(Selectable s)
    {
        //Debug.Log("Damage '" + s.name + "' was selected!");
        if (null != selectedRobot)
        {
            Waypoint wp = s.GetComponent<Waypoint>();
            if (null == wp) Debug.LogError("Missing <Waypoint> on " + s.name);
            selectedRobot.SetTarget(wp);

            UnhighlightRobot(selectedRobot);
            selectedRobot = null;
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
        if (selectedRobot != null)
        {
            if (Input.GetButton("Sel Damage"))
            {
                if (Input.GetButtonDown("Sel Damage"))
                {
                    //if (Input.GetButtonDown("bla"))
                    Selectable.activeGroup = SelectGroup.DAMAGE_1 + damageIndex;
                }
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
        else
        {
            Selectable.activeGroup = SelectGroup.ROBOT;
        }
    }
}
