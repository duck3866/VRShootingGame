using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinGun : GunItem
{
    protected override void Start()
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
        currentBullet = magazineBullet;
    }
    public override void EnterGrabbing(GameObject grabbingTransform)
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
        transform.localPosition = new Vector3(0,-1,3);
        transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
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
    public void FixedUpdate()
    {
        if (Grabbed)
        {
            DrawLine();
        }
    }
    public override void ItemUse()
    {
        Animator.SetTrigger("isFire");
        if (currentBullet > 0)
        {
            Fire();
        }
    }
    public override void Fire()
    {
        // int layerMask = 1 << LayerMask.NameToLayer("Default");
        Ray ray = new Ray(firePosition.transform.position, -firePosition.transform.right);
        if (Physics.Raycast(ray,out RaycastHit hitInfo, bulletDistance,layerMask,QueryTriggerInteraction.Ignore))
        {
            IDamagable damagable = hitInfo.collider.GetComponentInParent<IDamagable>();
            if (damagable != null)
            {
                Debug.Log($"플레이어 공격 적중: {hitInfo.collider.gameObject.name}");
                damagable.HitPoint(hitInfo.point);
                damagable.TakeDamage(attackPower);
            }
            bulletEffect.Stop();
            bulletEffect.Play();
            bulletEffectPosition.position = hitInfo.point;
            bulletEffectPosition.forward = hitInfo.normal;
            // Debug.Log($"wow {hitInfo.collider.gameObject.name}");
        }
        GameManager.AudioManager.PlaySoundEffect(fireSound, firePosition.transform.position, fireSoundVolume);
        currentBullet--;
        if (parentObjectIsRight) UIManager.Instance.RightHandInfoUpdate(gameObject.name,$"BUlLET: {currentBullet}/{magazineBullet}");
        else UIManager.Instance.LeftHandInfoUpdate(gameObject.name,$"BUlLET: {currentBullet}/{magazineBullet}");
        Debug.Log("총 상태: 총알발사");
    }
    public override Vector3 DrawRay()
    {
        Ray ray = new Ray(firePosition.transform.position, -firePosition.transform.right);
        if (Physics.Raycast(ray,out RaycastHit hitInfo, bulletDistance))
        { 
            return hitInfo.point;
        }
        else
        {
            return firePosition.transform.position + (-firePosition.transform.right * bulletDistance);
        }
    }
}
