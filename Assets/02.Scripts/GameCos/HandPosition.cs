using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPosition : MonoBehaviour
{
    [SerializeField] private bool isRightHand = true;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool grabbingObject = false;
    [SerializeField] private GameObject grabObject;
    
    private void Update()
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
                if (!hand.Grabbed)
                {
                    grabObject = other.gameObject;
                    grabObject.transform.SetParent(transform);
                    grabbingObject = true;
                    hand.EnterGrabbing();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IHandleObject>(out var hand))
        {
            if (grabObject == other.gameObject)
            {
                ThrowAwayObject();
                hand.ExitGrabbing(); 
            }
        }
    }

    private void OnClickHand()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            ThrowAwayObject();
        }

        if (isRightHand)
        {
            if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch))
            {
                if (grabObject.TryGetComponent<IHandleObject>(out IHandleObject hand))
                {
                    hand.ItemUse();
                }
                else
                {
                    Debug.Log("IHandleObject not found");
                }
            }
        }
    }

    private void ThrowAwayObject()
    {
        if (grabbingObject)
        {
            grabObject.transform.parent = null;
            grabObject.GetComponent<IHandleObject>().ExitGrabbing();
            grabbingObject = false;
            grabObject = null;
        }
    }
}
