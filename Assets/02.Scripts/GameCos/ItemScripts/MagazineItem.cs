using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineItem : MonoBehaviour, IHandleObject
{
    private bool _isEquipped = false;
    private void Start()
    {
        _isEquipped = false;
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
        if (!_isEquipped)
        {
            transform.parent = null;
            Grabbed = false; 
            _isEquipped = false;
        }
    }

    public void ItemUse()
    {
        StartCoroutine(StartThrowAwayMagazine());
    }

    public void InputButtonEvent()
    {
        _isEquipped = true;
        // StartCoroutine(StartThrowAwayMagazine());
    }

    public IEnumerator StartThrowAwayMagazine()
    {
        yield return new WaitForSeconds(0.5f);
        Grabbed = false;
    }
}
