using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : MonoBehaviour, IHandleObject
{
    protected Animator Animator;
    public GameObject magazinePosition;
    public GameObject magazine;
    public float maxBullet;
    public float currentBullet;
    
    public bool Grabbed { get; set; }

    private void Start()
    {
        Grabbed = false;
        Animator = GetComponent<Animator>();
    }
    public void EnterGrabbing()
    {
        Grabbed = true;
        transform.localPosition = new Vector3(0,0,1);
        transform.localRotation = Quaternion.identity;
    }

    public void ExitGrabbing()
    {
        Grabbed = false;
    }

    public void ItemUse()
    {
        Fire();
    }

    public virtual void Fire()
    {
        Debug.Log("ItemUse");
        Animator.SetTrigger("isFire");
    }

    public virtual void ThrowAwayMagazine()
    {
        if (magazine != null)
        {
            magazine.GetComponent<Rigidbody>().isKinematic = false;
            magazine.transform.parent = null;
            magazine = null;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magazine"))
        {
            magazine = other.gameObject;
            magazine.GetComponent<Rigidbody>().isKinematic = true;
            magazine.transform.SetParent(magazinePosition.transform);
            magazine.transform.localPosition = Vector3.zero;
            magazine.transform.localRotation = Quaternion.identity;
            magazine.transform.localScale = new Vector3(1,1,1);
        }
    }
}
