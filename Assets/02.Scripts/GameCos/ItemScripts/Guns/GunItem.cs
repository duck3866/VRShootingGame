using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : MonoBehaviour, IHandleObject
{
    protected Animator Animator;
    public GameObject magazinePosition;
    public GameObject magazine;
    public float maxBullet = 10f;
    public float currentBullet;
    public GameObject firePosition;
    public float bulletDistance;
    public ParticleSystem bulletEffect;
    public Transform bulletEffectPosition;
    protected LineRenderer Line;
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

    public virtual void ExitGrabbing()
    {
        Grabbed = false;
    }

    public virtual void ItemUse()
    {
        Animator.SetTrigger("isFire");
        // if (currentBullet > 0)
        // {
            Fire();
        //}
    }

    public virtual void InputButtonEvent()
    {
        Debug.Log("그아아악 시발!");
        if (magazine != null)
        {
            Debug.Log("그아앙하하ㅏ하");
            ThrowAwayMagazine(); 
        }
    }

    public virtual void Fire()
    {
      
    }

    public virtual void ThrowAwayMagazine()
    {
        magazine.transform.parent = null;
        magazine = null;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (magazine == null)
        {
            if (other.TryGetComponent<IHandleObject>(out IHandleObject hand))
            {
                Debug.Log("탄창 장착");
                magazine = other.gameObject;
                hand.Grabbed = true;
                magazine.transform.SetParent(magazinePosition.transform);
                magazine.transform.localPosition = Vector3.zero;
                magazine.transform.localRotation = Quaternion.identity;
                magazine.transform.localScale = new Vector3(1, 1, 1);
                ReloadBullet();
            }
        }
    }

    public virtual void ReloadBullet()
    {
        currentBullet = maxBullet;
    }
}
