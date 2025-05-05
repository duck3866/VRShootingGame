using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public Transform damageUI;
    public Image damageImage;
    
    public int initalHP = 10;
    private int _hp = 0;
    
    public static Tower Instance;

    public float damageTime = 0.1f;

    public int HP
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            StopAllCoroutines();
            StartCoroutine(DamageEvent());
            if (_hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _hp = initalHP;
        float z = Camera.main.nearClipPlane + 0.01f;
        damageUI.parent = Camera.main.transform;
        damageUI.localPosition = new Vector3(0, 0, z);
        damageImage.enabled = false;
    }

    private void Update()
    {
        
    }

    IEnumerator DamageEvent()
    {
        damageImage.enabled = true;
        yield return new WaitForSeconds(damageTime);
        damageImage.enabled = false;
    }
}
