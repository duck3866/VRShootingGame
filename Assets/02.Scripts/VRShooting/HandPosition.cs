using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPosition : MonoBehaviour
{
    [SerializeField] private bool isRightHand = true; // 오른쪽 손인지 여부
    // [SerializeField] private Vector3 offset; 
    [SerializeField] private bool grabbingObject = false; // 잡고 있는지 여부
    [SerializeField] private GameObject grabObject; // 잡은 오브젝트

    private void LateUpdate()
    {
        OnClickHand();
        if (isRightHand)
        {
            transform.position = ARAVRInput.RHandPosition;
            transform.rotation = ARAVRInput.GetRHandRotation();
        }
        else
        {
            transform.position = ARAVRInput.LHandPosition;
            transform.rotation = ARAVRInput.GetLHandRotation();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!grabbingObject)
        {
            if (other.TryGetComponent<IHandleObject>(out var hand))
            {
                if (hand.IsCanGrab())
                {
                    // grabObject = other.gameObject.transform.root.gameObject;
                    grabObject = other.gameObject;
                    // grabObject.transform.SetParent(transform);
                    grabbingObject = true;
                    hand.EnterGrabbing(gameObject);
                    if(isRightHand) UIManager.Instance.RightHandInfoUpdate(other.gameObject.name,"");
                    else UIManager.Instance.LeftHandInfoUpdate(other.gameObject.name,"");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IHandleObject>(out var hand))
        {
            if (!other.CompareTag("Enemy"))
            {
                if (grabObject == other.gameObject)
                {
                    // ThrowAwayObject(isRightHand);
                    grabbingObject = false;
                    grabObject = null;
                    if (isRightHand) UIManager.Instance.RightHandInfoUpdate("Null", "");
                    else UIManager.Instance.LeftHandInfoUpdate("Null", "");
                    hand.ExitGrabbing();
                }
            }
        }
    }

    private void OnClickHand()
    {
        // 물건 던지는 키
        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            if (grabObject != null)
            {
                ThrowAwayObject(false);
            }
        }
        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.RTouch))
        {
            if (grabObject != null)
            {
                ThrowAwayObject(true);
            }
        }
        
        if (ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.LTouch))
        {
            Debug.Log("오브젝트 InputButtonEvent 호출" + $" {gameObject.name}");
            if (grabObject != null)
            {
                if (grabObject.TryGetComponent<IHandleObject>(out var hand))
                {
                    hand.InputButtonEvent();
                }
            }
        }
        // 물건 사용함
        if (isRightHand)
        {
            if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch))
            {
                if (grabObject.TryGetComponent<IHandleObject>(out IHandleObject hand))
                {
                    hand.ItemUse();
                    ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
                }
                else
                {
                    Debug.Log("IHandleObject not found");
                }
            }
        }
        else if (!isRightHand)
        {
            if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.LTouch))
            {
                if (grabObject.TryGetComponent<IHandleObject>(out IHandleObject hand))
                {
                    hand.ItemUse();
                    ARAVRInput.PlayVibration(ARAVRInput.Controller.LTouch);
                }
                else
                {
                    Debug.Log("IHandleObject not found");
                }
            }
        }
    }

    private void ThrowAwayObject(bool isRight)
    {
        if (isRight == isRightHand)
        {
            if (grabbingObject)
            {
                // grabObject.transform.parent = null;
                grabObject.GetComponentInChildren<IHandleObject>().ExitGrabbing();
                Debug.Log($"오브젝트 IHandleObject 보유 여부 : {grabObject.GetComponentInParent<IHandleObject>()}");
                grabbingObject = false;
                grabObject = null;
                if (isRightHand) UIManager.Instance.RightHandInfoUpdate("Null", "");
                else UIManager.Instance.LeftHandInfoUpdate("Null", "");
            }
        }
    }
}