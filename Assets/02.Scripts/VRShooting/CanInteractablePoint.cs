using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanInteractablePoint : MonoBehaviour, IHandleObject
{
    private EnemyControllerCore _controllerCore;
    public GameObject parentObject;
    public FixedJoint fixedJoint;

    public FixedJoint parentJoint;

    private Vector3 prevPos;
    private Quaternion prevRot;
    
    [HideInInspector] public bool parentObjectIsRight;

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
            if (grabbingTransform.gameObject.CompareTag("Right")) parentObjectIsRight = true;
            else parentObjectIsRight = false;
            _controllerCore.ChangeState(EnemyControllerCore.EnemyState.Grabbing);
            // if (!parentObject.TryGetComponent(out FixedJoint pJoint))
            // {
            //     parentObject.AddComponent<FixedJoint>();
            //     parentJoint = pJoint;
            //     parentJoint.connectedBody = transform.GetComponent<Rigidbody>();
            //     fixedJoint.breakForce = Mathf.Infinity; // 원하는 힘으로 설정
            //     fixedJoint.breakTorque = Mathf.Infinity;
            // }
            //
            if (!transform.TryGetComponent(out FixedJoint fJoint))
            {
                fixedJoint = transform.AddComponent<FixedJoint>();
            }

            Grabbed = true;
            _controllerCore.OnCharacterJoint();
            fixedJoint.connectedBody = grabbingTransform.GetComponent<Rigidbody>();
            // fixedJoint.connectedBody = _controllerCore.transform.parent.GetComponent<Rigidbody>();
            fixedJoint.breakForce = Mathf.Infinity; // 원하는 힘으로 설정
            fixedJoint.breakTorque = Mathf.Infinity;

            // parentJoint.connectedBody = grabbingTransform.GetComponent<Rigidbody>();
            // // parentJoint.connectedBody = transform.GetComponent<Rigidbody>();
            // parentJoint.breakForce = Mathf.Infinity; // 원하는 힘으로 설정
            // parentJoint.breakTorque = Mathf.Infinity;
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
    }

    public void ExitGrabbing()
    {
        if (fixedJoint != null) fixedJoint.connectedBody = null;
        // if (parentJoint != null) parentJoint.connectedBody = null;

        StartCoroutine(ExitGrabbingAction());
    }

    private IEnumerator ExitGrabbingAction()
    {
        // 1. Joint 해제
        if (!_controllerCore.isDie)
        {
            _controllerCore.IsTharwing = true;
            if (fixedJoint != null) fixedJoint.connectedBody = null;
            // if (parentJoint != null) parentJoint.connectedBody = null;

            _controllerCore.OnCharacterJoint();
            FixedJoint[] fixedJoints = GetComponentsInChildren<FixedJoint>();
            foreach (var fixedJoint in fixedJoints)
            {
                Destroy(fixedJoint);
            }

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

            rigidbody.AddForce(throwDirection * throwPower, ForceMode.Force);

            float angle;
            Vector3 axis;
            deltaRotation.ToAngleAxis(out angle, out axis);
            Vector3 angularVelocity = (1.0f / Time.deltaTime) * angle * axis;
            rigidbody.angularVelocity = angularVelocity;

            // 5. 후처리
            yield return new WaitForSeconds(3f); // 필요시 약간의 시간 대기
            
            rigidbody.velocity = Vector3.zero;
            _controllerCore.IsTharwing = false;
            _controllerCore.ChangeState(EnemyControllerCore.EnemyState.Chase);
            // parentObject.transform.position = transform.position;
            _controllerCore.OffCharacterJoint();
            rigidbody.useGravity = false;
            Grabbed = false;
        }
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