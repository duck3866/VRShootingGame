using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVRIK : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
        
    public Transform leftFoot;
    public Transform rightFoot;
    
    private Animator animator;
    // private int layerIndex_Weapons;
    public Transform spine; // 아바타의 상체
    // public Animator myani;
    public Transform LookTarget;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spine = animator.GetBoneTransform(HumanBodyBones.Spine); // 상체 transform 가져오기
        leftHand = GameObject.FindWithTag("Left").transform;
        rightHand = GameObject.FindWithTag("Right").transform;
    }

    public void Initialized()
    {
        var vector3 = transform.localPosition;
        vector3.y = new Vector3(0, 1, 0).y;
        transform.localPosition = vector3;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void LateUpdate()
    {
        if (animator != null && LookTarget != null)
        {
            spine.transform.LookAt(LookTarget.position);
        }
        
    }
   

    private void OnAnimatorIK(int _layerIndex)
    {
        // if (_layerIndex != layerIndex_Weapons)
        // {
        //    return;
        // }
        
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand,1f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        //------------------------------------------------------
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
        //------------------------------------------------------
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation * Quaternion.Euler(0,0,90));
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation * Quaternion.Euler(-90,90,0));
        //------------------------------------------------------
        animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFoot.position);
        animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFoot.rotation);
        animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFoot.position);
        animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFoot.rotation);
    }
}
