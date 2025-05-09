using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : MonoBehaviour, IHandleObject
{
   [Header("탄창 장착 위치")]
    public GameObject magazinePosition; // 탄창 장착 위치
    [HideInInspector] public GameObject magazine; // 탄창 오브젝트
    [Header("최대 탄약 개수")]
    public float maxBullet = 10f; // 최대 탄약 개수
    [Header("탄창 탄약 개수")]
    public float magazineBullet = 30f; // 탄창 탄약 개수
    [Header("현재 탄약 개수")]
    public float currentBullet; // 현재 탄약 개수
    [Header("총구")]
    public GameObject firePosition; // 총알이 나갈 위치 
    [Header("총 사거리")]
    public float bulletDistance; // Ray의 길이
    
    public ParticleSystem bulletEffect; // Ray가 적중했을때 이펙트
    public Transform bulletEffectPosition; // 파티클 시스템 위치
    
    public LayerMask layerMask;
    public float attackPower;
    [HideInInspector] public bool parentObjectIsRight; // 부모가 오른손인지 여부
    
    protected Animator Animator; // animator
    protected LineRenderer LaserSite; // lineRenderer
    public bool Grabbed { get; set; } // 잡혔는지 여부

    private void Start()
    {
        Grabbed = false;
        LaserSite = GetComponent<LineRenderer>();
        LaserSite.enabled = false;
        Animator = GetComponent<Animator>();
        if (bulletEffect == null)
        {
            GameObject gameObject = GameObject.FindGameObjectWithTag("BulletImpact");
            bulletEffect = gameObject.GetComponent<ParticleSystem>();
            bulletEffectPosition = gameObject.GetComponent<Transform>();
        }
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
