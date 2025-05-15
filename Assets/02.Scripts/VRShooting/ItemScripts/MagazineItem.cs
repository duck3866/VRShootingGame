using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineItem : MonoBehaviour, IHandleObject
{
    [field : SerializeField]
    public bool Grabbed { get; set; }

    public bool parentObjectIsRight { get; set; }
    private bool _isEquipped = false;
    private void Start()
    {
        _isEquipped = false;
        Grabbed = false;
    }
   
    public void EnterGrabbing(GameObject grabbingTransform)
    {
        transform.SetParent(grabbingTransform.transform);
        parentObjectIsRight = grabbingTransform.gameObject.CompareTag("Right");
        Grabbed = true;
        if (parentObjectIsRight) UIManager.Instance.RightHandInfoUpdate(gameObject.name, "");
        else UIManager.Instance.LeftHandInfoUpdate(gameObject.name, "");
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
    public bool IsCanGrab()
    {
        if (Grabbed)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
