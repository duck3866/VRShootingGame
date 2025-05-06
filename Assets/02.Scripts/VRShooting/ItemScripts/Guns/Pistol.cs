using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Input;
using UnityEngine;

public class Pistol : GunItem
{
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
        Ray ray = new Ray(firePosition.transform.position, -firePosition.transform.up);
        if (Physics.Raycast(ray,out RaycastHit hitInfo, bulletDistance))
        {
            IDamagable damagable = hitInfo.collider.GetComponentInParent<IDamagable>();
            if (damagable != null)
            {
                Debug.Log($"플레이어 공격 적중: {hitInfo.collider.gameObject.name}");
                damagable.TakeDamage(5f);
                damagable.HitPoint(hitInfo.point);
            }
            bulletEffect.Stop();
            bulletEffect.Play();
            bulletEffectPosition.position = hitInfo.point;
            bulletEffectPosition.forward = hitInfo.normal;
            Debug.Log(hitInfo.collider.gameObject.name);
        }
        currentBullet--;
        if (parentObjectIsRight) UIManager.Instance.RightHandInfoUpdate(gameObject.name,$"BUlLET: {currentBullet}/{maxBullet}");
        else UIManager.Instance.LeftHandInfoUpdate(gameObject.name,$"BUlLET: {currentBullet}/{maxBullet}");
        Debug.Log("총 상태: 총알발사");
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
}
