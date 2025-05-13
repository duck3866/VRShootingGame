using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class Bullet : MonoBehaviour, IHandleObject
{
    public float moveSpeed = 5f;
    public float aliveTime;
    private float _currentTime;
    
    private Vector3 prevPos; // 처음 잡혔을때 위치
    private Quaternion prevRot; // 처음 잡혔을때 각도

    private bool _parentObjectIsRight;
    public bool Grabbed { get; set; }
    private void OnEnable()
    {
        _currentTime = 0;
        Grabbed = false;
    }

    private void Update()
    {
        ForwardingBullet();
    }

    private void ForwardingBullet()
    {
        if (!Grabbed)
        {
            if (_currentTime >= aliveTime)
            {
                Destroy(gameObject);
            }
            transform.position += transform.forward * (moveSpeed * Time.deltaTime);
            _currentTime += Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!Grabbed)
        {
            if (other.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(5f);
            }  
        }
        
    }
    
    public void EnterGrabbing(GameObject grabbingTransform)
    {
        transform.SetParent(grabbingTransform.transform);
        _parentObjectIsRight = grabbingTransform.CompareTag("Right");
        Grabbed = true;
        if (_parentObjectIsRight)
        {
            prevPos = ARAVRInput.RHandPosition;
            prevRot = ARAVRInput.RHand.rotation;
        }
        else
        {
            prevPos = ARAVRInput.LHandPosition;
            prevRot = ARAVRInput.LHand.rotation;
        }
    }

    public void ExitGrabbing()
    {
        transform.parent = null;
        StartCoroutine(WaitForExit());
        // Grabbed = false;
    }

    private IEnumerator WaitForExit()
    {
        Vector3 throwDirection = new Vector3();
        if (_parentObjectIsRight)
        {
            throwDirection = ARAVRInput.RHandPosition - prevPos;
        }
        else
        {
            throwDirection = ARAVRInput.LHandPosition - prevPos;
        }
        transform.forward = throwDirection;
        // Quaternion deltaRotation = new Quaternion();
        // if (_parentObjectIsRight)
        // {
        //     deltaRotation = ARAVRInput.RHand.rotation * Quaternion.Inverse(prevRot);
        // }
        // else
        // {
        //     deltaRotation = ARAVRInput.LHand.rotation * Quaternion.Inverse(prevRot);
        // }
        //
        yield return new WaitForSecondsRealtime(0.1f);
        Grabbed = false;
    }
    public void ItemUse()
    {
       
    }

    public void InputButtonEvent()
    {
       
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
