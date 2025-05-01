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
    public void EnterGrabbing(GameObject grabbingTransform)
    {
        transform.SetParent(grabbingTransform.transform);
        Grabbed = true;
    }

    public void ExitGrabbing()
    {
        transform.parent = null;
        Grabbed = false;
    }

    public void ItemUse()
    {
       
    }

    public void InputButtonEvent()
    {
        StartCoroutine(StartThrowAwayMagazine());
    }

    public IEnumerator StartThrowAwayMagazine()
    {
        yield return new WaitForSeconds(0.5f);
        Grabbed = false;
    }
}
