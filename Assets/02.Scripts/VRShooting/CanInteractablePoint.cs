using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanInteractablePoint : MonoBehaviour, IHandleObject
{
   
    [Header("던져질 힘")]
    public float throwPower = 2000f;
    // public GameObject parentObject;
    [Header("위치 고정용 FixedJoint")]
    public FixedJoint fixedJoint;
    // public FixedJoint parentJoint;
    [HideInInspector] public bool parentObjectIsRight; // 부모가 어느 손인지 확인하기 위함
    private EnemyControllerCore _controllerCore; // 적의 콘트롤러 클래스
    
    private Vector3 prevPos; // 처음 잡혔을때 위치
    private Quaternion prevRot; // 처음 잡혔을때 각도
    
    public bool Grabbed { get; set; }

    // public event Action OnInteractableStart;
    // public event Action OnInteractableEnd;
    /// <summary>
    /// 초기화 함수
    /// </summary>
    /// <param name="controllerCore">할당할 콘트롤러 클래스</param>
    public void Init(EnemyControllerCore controllerCore)
    {
        // parentJoint = GetComponentInParent<FixedJoint>();
        fixedJoint = GetComponent<FixedJoint>();
        _controllerCore = controllerCore;
    }
    /// <summary>
    /// 잡혔을때 발동하는 함수
    /// </summary>
    /// <param name="grabbingTransform">잡은 손 오브젝트의 트랜스폼</param>
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
    /// <summary>
    /// 놓았을때 호출되는 함수
    /// </summary>
    public void ExitGrabbing()
    {
        if (fixedJoint != null) fixedJoint.connectedBody = null;
        // if (parentJoint != null) parentJoint.connectedBody = null;

        StartCoroutine(ExitGrabbingAction((ARAVRInput.RHandPosition - prevPos)));
    }

    public IEnumerator ExitGrabbingAction(Vector3 throwDirection)
    {
        Debug.Log("터진거 맞고 날아감");
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
            
            yield return new WaitForFixedUpdate();
            
            // Rigidbody rigidbody = _controllerCore.OnThrowObject();
            Rigidbody rigidbody = _controllerCore.hips;
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
            
            // Vector3 throwDirection = (ARAVRInput.RHandPosition - prevPos);
            Quaternion deltaRotation = ARAVRInput.RHand.rotation * Quaternion.Inverse(prevRot);

            rigidbody.AddForce(throwDirection * throwPower, ForceMode.Force);

            float angle;
            Vector3 axis;
            deltaRotation.ToAngleAxis(out angle, out axis);
            Vector3 angularVelocity = (1.0f / Time.deltaTime) * angle * axis;
            rigidbody.angularVelocity = angularVelocity;
            
            yield return new WaitForSeconds(3f); 
            
            rigidbody.velocity = Vector3.zero;
            _controllerCore.IsTharwing = false;
            _controllerCore.ChangeState(EnemyControllerCore.EnemyState.Chase);
            // parentObject.transform.position = transform.position;
            _controllerCore.OffCharacterJoint();
            rigidbody.useGravity = false;
            Grabbed = false;
        }
    }
    /// <summary>
    /// IndexTrigger가 눌렸을때 호출되는 함수
    /// </summary>
    public void ItemUse()
    {
    }
    /// <summary>
    /// Two가 눌렸을때 호출되는 함수
    /// </summary>
    public void InputButtonEvent()
    {
    }
    /// <summary>
    /// 잡을 수 있는지 여부
    /// </summary>
    /// <returns>현재 잡혀있으면 false 아니면 true</returns>
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