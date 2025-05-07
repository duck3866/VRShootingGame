using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : MonoBehaviour, IHandleObject
{
    protected Animator Animator;
    public GameObject magazinePosition;
    [HideInInspector] public GameObject magazine;
    public float maxBullet = 10f;
    public float magazineBullet = 30f;
    public float currentBullet;
    public GameObject firePosition;
    public float bulletDistance;
    public ParticleSystem bulletEffect;
    public Transform bulletEffectPosition;
    [HideInInspector] public bool parentObjectIsRight;
    
    protected LineRenderer LaserSite;
    public bool Grabbed { get; set; }

    private void Start()
    {
        Grabbed = false;
        LaserSite = GetComponent<LineRenderer>();
        LaserSite.enabled = false;
        Animator = GetComponent<Animator>();
    }

    public void EnterGrabbing(GameObject grabbingTransform)
    {
        transform.SetParent(grabbingTransform.transform);
        // grabbingTransform.transform.SetParent(this.transform);
        Grabbed = true;
        LaserSite.enabled = true;
        // if (transform.parent.CompareTag("Right")) parentObjectIsRight = true;
        if (grabbingTransform.gameObject.CompareTag("Right")) parentObjectIsRight = true;
        else parentObjectIsRight = false;
        transform.localPosition = new Vector3(0,0,1);
        transform.localRotation = Quaternion.identity;
    }

    public virtual void ExitGrabbing()
    {
        transform.parent = null;
        Grabbed = false;
        LaserSite.enabled = false;
    }

    public virtual void ItemUse()
    {
        Animator.SetTrigger("isFire");
        if (currentBullet > 0)
        {
            Fire();
        }
    }

    public virtual void InputButtonEvent()
    {
        Debug.Log("총 상태: 탄창 분리 시도");
        if (magazine != null)
        {
            Debug.Log("총 상태: 탄창 분리 성공");
            ThrowAwayMagazine(); 
        }
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
            if (other.CompareTag("Magazine"))
            {
                if (other.TryGetComponent<IHandleObject>(out IHandleObject hand))
                {
                    Debug.Log("탄창 장착");
                    magazine = other.gameObject;
                    hand.InputButtonEvent();
                    hand.Grabbed = true;
                    magazine.transform.SetParent(magazinePosition.transform);
                    magazine.transform.localPosition = Vector3.zero;
                    magazine.transform.localRotation = Quaternion.identity;
                    magazine.transform.localScale = new Vector3(1, 1, 1);
                    ReloadBullet();
                    if (parentObjectIsRight) UIManager.Instance.RightHandInfoUpdate(gameObject.name,$"BUlLET: {currentBullet}/{maxBullet}");
                    else UIManager.Instance.LeftHandInfoUpdate(gameObject.name,$"BUlLET: {currentBullet}/{maxBullet}");
                } 
            }
        }
    }

    public virtual void ReloadBullet()
    {
        currentBullet = maxBullet;
    }

    public void DrawLine()
    {
        LaserSite.SetPosition(0,firePosition.transform.position);
        LaserSite.SetPosition(1,DrawRay());
    }

    public Vector3 DrawRay()
    {
        Ray ray = new Ray(firePosition.transform.position, -firePosition.transform.up);
        if (Physics.Raycast(ray,out RaycastHit hitInfo, bulletDistance))
        { 
            return hitInfo.point;
        }
        else
        {
            return firePosition.transform.position + (-firePosition.transform.up * bulletDistance);
        }
    }
}
