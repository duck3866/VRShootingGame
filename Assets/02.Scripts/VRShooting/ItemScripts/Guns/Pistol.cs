using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Input;
using UnityEngine;

public class Pistol : GunItem
{
    private bool isFire = false;
    public virtual void EnterGrabbing(GameObject grabbingTransform)
    {
        Debug.Log("잡힘 초기화");
        Grabbed = true;
        isFire = false;
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
    public void FixedUpdate()
    {
        // Debug.DrawRay(firePosition.transform.position,-firePosition.transform.up * bulletDistance,Color.red);
        if (Grabbed)
        {
            DrawLine();
        }
    }
    
    public override void Fire()
    {
        // int layerMask = 1 << LayerMask.NameToLayer("Default");
        if (!isFire)
        {
            StartCoroutine(WaitFire());
            Ray ray = new Ray(firePosition.transform.position, -firePosition.transform.up);
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
    }

    private IEnumerator WaitFire()
    {
        isFire = true;
        yield return new WaitForSecondsRealtime(0.1f);
        isFire = false;
    }
    public override void InputButtonEvent()
    {
        Debug.Log("총 상태: 탄창 분리 시도");
        if (magazine != null)
        {
            Debug.Log("총 상태: 탄창 분리 성공");
            if (magazine.TryGetComponent<IHandleObject>(out IHandleObject hand))
            {
                hand.ItemUse();
            }
            ThrowAwayMagazine();
        }
        else
        {
            Debug.Log("총 상태: 탄창 분리 실패(탄창 없음)");
        }
    }

    private void OnTriggerEnter(Collider other)
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
                    magazineBullet = 30f;
                    currentBullet = maxBullet; // 최대탄창 초기화
                    GameManager.AudioManager.PlaySoundEffect(reloadSound, transform.position, reloadSoundVolume);
                    if (gameObject.transform.root.CompareTag("Player"))
                    {
                        if (parentObjectIsRight) UIManager.Instance.RightHandInfoUpdate(gameObject.name, $"BUlLET: {currentBullet}/{magazineBullet}");
                        else UIManager.Instance.LeftHandInfoUpdate(gameObject.name, $"BUlLET: {currentBullet}/{magazineBullet}");
                    }
                }
            }
        }
    }
}
