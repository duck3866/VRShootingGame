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
    // [HideInInspector]
    public bool parentObjectIsRight { get; set; } // 부모가 오른손인지 여부
    [Header("총기 사운드")]
    [SerializeField] protected AudioClip fireSound;
    [SerializeField] protected AudioClip misFireSound;
    [SerializeField] protected AudioClip reloadSound;
    [Header("총기 사운드 볼륨")]
    [SerializeField] [Range(0, 1)] protected float fireSoundVolume;
    [SerializeField] [Range(0, 1)] protected float misFireSoundVolume;
    [SerializeField] [Range(0, 1)] protected float reloadSoundVolume;
    // [SerializeField] protected bool isShooting = false;
    [SerializeField] protected bool isReloading = false;
    protected Animator Animator; // animator
    protected LineRenderer LaserSite; // lineRenderer
    
    [field : SerializeField]
    public bool Grabbed { get; set; } // 잡혔는지 여부

    protected virtual void Start()
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
    
    public virtual void EnterGrabbing(GameObject grabbingTransform)
    {
        Debug.Log("잡힘 초기화");
        Grabbed = true;
        // isShooting = false;
        isReloading = false;
        transform.SetParent(grabbingTransform.transform);
        // grabbingTransform.transform.SetParent(this.transform);
        LaserSite.enabled = true;
        // if (transform.parent.CompareTag("Right")) parentObjectIsRight = true;
        if (grabbingTransform.gameObject.CompareTag("Right"))
        {
            parentObjectIsRight = true;
        }
        else
        {
            parentObjectIsRight = false;
        }
        transform.localPosition = new Vector3(0,0,1);
        transform.localRotation = Quaternion.identity;
        if (magazine != null)
        {
            if (parentObjectIsRight) UIManager.Instance.RightHandInfoUpdate(gameObject.name,$"BUlLET: {currentBullet}/{magazineBullet}");
            else UIManager.Instance.LeftHandInfoUpdate(gameObject.name,$"BUlLET: {currentBullet}/{magazineBullet}");
        }
        else
        {
            if (parentObjectIsRight) UIManager.Instance.RightHandInfoUpdate(gameObject.name, "");
            else UIManager.Instance.LeftHandInfoUpdate(gameObject.name, "");
        }
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
        if (currentBullet > 0 && !isReloading)
        {
            Fire();
        }
        else if(currentBullet < 1)
        {
            if (magazine != null && !isReloading)
            {
                if (magazineBullet >= maxBullet)
                {
                    StartCoroutine(Reload());
                    return;
                }
            }
            GameManager.AudioManager.PlaySoundEffect(misFireSound, firePosition.transform.position, misFireSoundVolume);
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
    public virtual IEnumerator Reload()
    {
        isReloading = true;
        Animator.SetTrigger("isReload");
        GameManager.AudioManager.PlaySoundEffect(reloadSound,transform.position, reloadSoundVolume);
        yield return new WaitForSecondsRealtime(2f);
        currentBullet = maxBullet;
        magazineBullet -= maxBullet;
        if (Grabbed)
        {
            if (parentObjectIsRight)
            {
                UIManager.Instance.RightHandInfoUpdate(gameObject.name,$"BUlLET: {currentBullet}/{magazineBullet}");
            }
            else
            {
                UIManager.Instance.LeftHandInfoUpdate(gameObject.name,$"BUlLET: {currentBullet}/{magazineBullet}");
            }
        }
        isReloading = false;
    }

    public virtual void ThrowAwayMagazine()
    {
        magazine.transform.parent = null;
        magazine = null;
    }
    
    public void DrawLine()
    {
        LaserSite.SetPosition(0,firePosition.transform.position);
        LaserSite.SetPosition(1,DrawRay());
    }

    public virtual Vector3 DrawRay()
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
