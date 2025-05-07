using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour, IHandleObject
{
    private Vector3 prevPos;
    private Quaternion prevRot;
    public float throwPower = 1000f;
    [HideInInspector] public bool parentObjectIsRight;
    private Rigidbody _rigidbody;
    [SerializeField] private Material bombMaterial;
    [SerializeField] private float detonationTime;
    private bool _isBomb;
    private Material _material;
    public bool Grabbed { get; set; }

    private void Start()
    {
        _isBomb = false;
        _material = GetComponent<Renderer>().material;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void EnterGrabbing(GameObject grabbingTransform)
    {
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
        
        if (grabbingTransform.gameObject.CompareTag("Right")) parentObjectIsRight = true;
        else parentObjectIsRight = false;
        transform.SetParent(grabbingTransform.transform);
        
        Grabbed = true;
        if (parentObjectIsRight)
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
        if (!_isBomb)
        {
            StartCoroutine(ExitGrabbingAction());
        }
    }

    private IEnumerator ExitGrabbingAction()
    {
        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;
        
        Vector3 throwDirection = (ARAVRInput.RHandPosition - prevPos);
        Quaternion deltaRotation = ARAVRInput.RHand.rotation * Quaternion.Inverse(prevRot);
        
        _rigidbody.AddForce(throwDirection * throwPower, ForceMode.Force);

        float angle;
        Vector3 axis;
        deltaRotation.ToAngleAxis(out angle, out axis);
        Vector3 angularVelocity = (1.0f / Time.deltaTime) * angle * axis;
        _rigidbody.angularVelocity = angularVelocity;
        yield return new WaitForSeconds(3f);
        _rigidbody.velocity = Vector3.zero;
        Grabbed = false;
    }

    public void ItemUse()
    {
        StartCoroutine(Detonation());
    }

    public void InputButtonEvent()
    {
        
    }

    private IEnumerator Detonation()
    {
        yield return new WaitForSeconds(detonationTime);
        _isBomb = true;
        ExitGrabbing();
        Debug.Log("펑! 대소고 터지는 소리");
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
