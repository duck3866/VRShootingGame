using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPackItem : MonoBehaviour, IHandleObject
{
    [SerializeField] private AudioClip healSound;
    [SerializeField][Range(0,1)] private float healSoundVolume;
    private void Start()
    {
        Grabbed = false;
    }

    public bool Grabbed { get; set; }
    public bool parentObjectIsRight { get; set; }

    public void EnterGrabbing(GameObject grabbingTransform)
    {
        parentObjectIsRight = grabbingTransform.gameObject.CompareTag("Right");
        transform.SetParent(grabbingTransform.transform);
        Grabbed = true;
        if (parentObjectIsRight) UIManager.Instance.RightHandInfoUpdate(gameObject.name, "");
        else UIManager.Instance.LeftHandInfoUpdate(gameObject.name, "");
    }

    public void ExitGrabbing()
    {
        transform.parent = null;
        Grabbed = false;
    }

    public void ItemUse()
    {
        Debug.Log("힐");
        
        IDamagable damagable = gameObject.GetComponentInParent<IDamagable>();
        if (damagable != null)
        {
            GameManager.AudioManager.PlaySoundEffect(healSound, transform.position, healSoundVolume);
            damagable.TakeDamage(-10f);
            ExitGrabbing();
        }
       
    }

    public void InputButtonEvent()
    {
       
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
