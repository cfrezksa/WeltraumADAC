using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerPoint : MonoBehaviour {

    public DamageType repairType;
    public GameObject prefab;
    public GameObject container;

    public void Open()
    {
        Animator anim = container.GetComponent<Animator>();
        if (null != anim)
        {
            anim.SetTrigger("open");
        }
    }

    public void Close()
    {
        Animator anim = container.GetComponent<Animator>();
        if (null != anim)
        {
            anim.SetTrigger("close");
        }
    }
}
