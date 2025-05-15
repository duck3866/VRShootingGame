using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour, IHandleObject
{
   
    public float throwPower = 1000f;
    public bool Grabbed { get; set; }
    public bool parentObjectIsRight { get; set; }

    [SerializeField] private Material bombMaterial;
    [SerializeField] private float detonationTime;
    [SerializeField] private float detonationRadius;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private AudioClip bombCountSound;
    [SerializeField][Range(0,1)] private float bombCountSoundVolume;
    [SerializeField] private AudioClip bombEffectSound;
    [SerializeField][Range(0,1)] private float bombEffectSoundSoundVolume;
    [SerializeField] private GameObject bombEffect;
    private bool _isBomb;
    private bool isDetonated;
    private Material _material;
    
    private Rigidbody _rigidbody;
    private Vector3 prevPos;
    private Quaternion prevRot;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _isBomb = false;
        isDetonated = false;
        _material = GetComponent<Renderer>().material;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void EnterGrabbing(GameObject grabbingTransform)
    {
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;

        if (grabbingTransform.gameObject.CompareTag("Right"))
        {
            parentObjectIsRight = true;
        }
        else
        {
            parentObjectIsRight = false;
        }
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
        if (parentObjectIsRight) UIManager.Instance.RightHandInfoUpdate(gameObject.name, "");
        else UIManager.Instance.LeftHandInfoUpdate(gameObject.name, "");
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
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;

        Vector3 throwDirection = new Vector3();
        if (parentObjectIsRight)
        {
            throwDirection = ARAVRInput.RHandPosition - prevPos;
        }
        else
        {
            throwDirection = ARAVRInput.LHandPosition - prevPos;
        }

        Quaternion deltaRotation = new Quaternion();
        if (parentObjectIsRight)
        {
            deltaRotation = ARAVRInput.RHand.rotation * Quaternion.Inverse(prevRot);
        }
        else
        {
            deltaRotation = ARAVRInput.LHand.rotation * Quaternion.Inverse(prevRot);
        }
        _rigidbody.AddForce(throwDirection * throwPower, ForceMode.Force);
  
        float angle;
        Vector3 axis;
        deltaRotation.ToAngleAxis(out angle, out axis);
        Vector3 angularVelocity = (1.0f / Time.deltaTime) * angle * axis;
        _rigidbody.angularVelocity = angularVelocity;
        yield return new WaitForSeconds(3f);
        if (!isDetonated)
        {
            StartCoroutine(Detonation());
        }
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
        _animator.SetTrigger("isBomb");
        GameManager.AudioManager.PlaySoundEffect(bombCountSound,transform.position, bombCountSoundVolume);
        isDetonated = true;
        yield return new WaitForSeconds(detonationTime);
        isDetonated = false;
        GameManager.AudioManager.PlaySoundEffect(bombEffectSound,transform.position,bombEffectSoundSoundVolume);
        Collider[] colliders = Physics.OverlapSphere(transform.position, detonationRadius, layerMask);
        foreach (Collider collider in colliders)
        {
            Debug.Log($"터진거 맞은 오브젝트{collider.gameObject.name}");
            IDamagable damagable = collider.GetComponentInParent<IDamagable>();
            if (damagable != null)
            {
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    EnemyControllerCore enemyControllerCore = collider.GetComponentInParent<EnemyControllerCore>();
                    enemyControllerCore.AddForceMethod(Vector3.up);
                    // CanInteractablePoint canInteractablePoint = enemyControllerCore.ReturnCanInteract();
                    // canInteractablePoint.StartCoroutine(canInteractablePoint.ExitGrabbingAction(Vector3.up));
                }
                damagable.TakeDamage(1f);
            }
        }
        _isBomb = true;
        GameObject effect = Instantiate(bombEffect);
        effect.transform.position = transform.position;
        ExitGrabbing();
        Debug.Log("펑! 대소고 터지는 소리");
        StartCoroutine(DestroyObject());
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSecondsRealtime(2f);
        Destroy(gameObject);
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detonationRadius);
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
