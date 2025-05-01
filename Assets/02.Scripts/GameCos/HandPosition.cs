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
                if (!hand.Grabbed)
                {
                    grabObject = other.gameObject.transform.root.gameObject;
                    // grabObject = other.gameObject;
                    // grabObject.transform.SetParent(transform);
                    grabbingObject = true;
                    hand.EnterGrabbing(gameObject);
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
                    ThrowAwayObject();
                    hand.ExitGrabbing();
                } 
            }
        }
    }

    private void OnClickHand()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            if (grabObject != null)
            {
                ThrowAwayObject();
            }
        }

        if (ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.LTouch))
        {
            Debug.Log("tlqkf!" + $" {gameObject.name}");
            if (grabObject != null)
            {
                if (grabObject.TryGetComponent<IHandleObject>(out var hand))
                {
                    hand.InputButtonEvent();
                }
            }
        }

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

    private void ThrowAwayObject()
    {
        if (grabbingObject)
        {
            // grabObject.transform.parent = null;
            grabObject.GetComponentInChildren<IHandleObject>().ExitGrabbing();
            Debug.Log($"{grabObject.GetComponentInParent<IHandleObject>()}뭐야 이건");
            grabbingObject = false;
            grabObject = null;
        }
    }
}