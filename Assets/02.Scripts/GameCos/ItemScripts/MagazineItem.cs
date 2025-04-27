using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineItem : MonoBehaviour, IHandleObject
{
    private void Start()
    {
        Grabbed = false;
    }
    public bool Grabbed { get; set; }
    public void EnterGrabbing()
    {
        Grabbed = true;
    }

    public void ExitGrabbing()
    {
        Grabbed = false;
    }

    public void ItemUse()
    {
       
    }
}
