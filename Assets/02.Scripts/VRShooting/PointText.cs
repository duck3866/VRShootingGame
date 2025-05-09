using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointText : MonoBehaviour
{
    [SerializeField] private float deleteTime;
    private Animator animator;
    private void OnEnable()
    {
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(WaitAnimation());
    }

    private IEnumerator WaitAnimation()
    {
        yield return new WaitForSecondsRealtime(deleteTime);
        animator.SetTrigger("toDelete");
        yield return new WaitForSecondsRealtime(0.5f);
        Destroy(gameObject);
    }
}
