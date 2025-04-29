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
            if (hitInfo.collider.gameObject.TryGetComponent<IDamagable>(out IDamagable enemy))
            {
                Debug.Log("엥?");
                enemy.TakeDamage(5f);
            }
            bulletEffect.Stop();
            bulletEffect.Play();
            bulletEffectPosition.position = hitInfo.point;
            bulletEffectPosition.forward = hitInfo.normal;
            Debug.Log(hitInfo.collider.gameObject.name);
        }
        currentBullet--;
        Debug.Log("탕!");
    }
    public override void InputButtonEvent()
    {
        Debug.Log("따흐흑 따흐흑");
        if (magazine != null)
        {
            Debug.Log("그아앙하하ㅏ하");
            if (magazine.TryGetComponent<IHandleObject>(out IHandleObject hand))
            {
                hand.InputButtonEvent();
            }
            ThrowAwayMagazine();
        }
    }
}
