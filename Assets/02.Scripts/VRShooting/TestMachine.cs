using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMachine : MonoBehaviour
{
    [SerializeField] private GameObject firePosition;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float createTime;
    [SerializeField] private float currentTime; 
    private void Start()
    {
        currentTime = 0;
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        if (createTime < currentTime)
        {
            GameObject obj = Instantiate(bullet);
            obj.transform.position = firePosition.transform.position;
            obj.transform.forward = firePosition.transform.forward;
            currentTime = 0;
        }
    }
}
