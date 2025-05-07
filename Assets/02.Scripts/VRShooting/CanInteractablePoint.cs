using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanInteractablePoint : MonoBehaviour, IHandleObject
{
    private EnemyControllerCore  _controllerCore;

    public FixedJoint fixedJoint;

    public FixedJoint parentJoint;
    
    private Vector3 prevPos;
    private Quaternion prevRot;
    
    public float throwPower = 2000f;
   
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
            
            prevPos = ARAVRInput.RHandPosition;
            prevRot = ARAVRInput.RHand.rotation;
        }
    }

    public void ExitGrabbing()
    {
        if (fixedJoint != null) fixedJoint.connectedBody = null;
        if (parentJoint != null) parentJoint.connectedBody = null;

        StartCoroutine(ExitGrabbingAction());
    }
    
    private IEnumerator ExitGrabbingAction()
    {
        // 1. Joint 해제
        if (fixedJoint != null) fixedJoint.connectedBody = null;
        if (parentJoint != null) parentJoint.connectedBody = null;

        // 2. 물리 프레임까지 기다림
        yield return new WaitForFixedUpdate();

        // 3. Rigidbody 설정
        // Rigidbody rigidbody = _controllerCore.OnThrowObject();
        Rigidbody rigidbody = _controllerCore.hips;
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;

        // 4. 던지기 Force 적용
        Vector3 throwDirection = (ARAVRInput.RHandPosition - prevPos);
        Quaternion deltaRotation = ARAVRInput.RHand.rotation * Quaternion.Inverse(prevRot);

        rigidbody.AddForce(throwDirection * throwPower, ForceMode.Impulse);

        float angle;
        Vector3 axis;
        deltaRotation.ToAngleAxis(out angle, out axis);
        Vector3 angularVelocity = (1.0f / Time.deltaTime) * angle * axis;
        rigidbody.angularVelocity = angularVelocity;

        // 5. 후처리
        yield return new WaitForSeconds(3f); // 필요시 약간의 시간 대기
        _controllerCore.OffCharacterJoint();
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
        if (_controllerCore.isDie || Grabbed)
        {
            return false;
        }
        else
        {
            return true;
        }
       
    }
}
