using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanInteractablePoint : MonoBehaviour, IHandleObject
{
    private EnemyControllerCore  _controllerCore;

    public FixedJoint fixedJoint;

    public FixedJoint parentJoint;
    // public event Action OnInteractableStart;
    // public event Action OnInteractableEnd;

    public void Init(EnemyControllerCore controllerCore)
    {
        // parentJoint = GetComponentInParent<FixedJoint>();
        fixedJoint = GetComponent<FixedJoint>();
        _controllerCore = controllerCore;
    }
    public bool Grabbed { get; set; }
    public void EnterGrabbing(GameObject grabbingTransform)
    {
        if (!_controllerCore.isDie)
        {
            Grabbed = true;
            _controllerCore.OnCharacterJoint();
            fixedJoint.connectedBody = grabbingTransform.GetComponent<Rigidbody>();
            // fixedJoint.connectedBody = _controllerCore.transform.parent.GetComponent<Rigidbody>();
            fixedJoint.breakForce = Mathf.Infinity; // 원하는 힘으로 설정
            fixedJoint.breakTorque = Mathf.Infinity;

            parentJoint.connectedBody = grabbingTransform.GetComponent<Rigidbody>();
            parentJoint.breakForce = Mathf.Infinity; // 원하는 힘으로 설정
            parentJoint.breakTorque = Mathf.Infinity;
        }
    }

    public void ExitGrabbing()
    {
        Grabbed = false;
        fixedJoint.connectedBody = null;
        parentJoint.connectedBody = null;
        _controllerCore.OffCharacterJoint();
    }

    public void ItemUse()
    {
       
    }

    public void InputButtonEvent()
    {
        
    }
}
