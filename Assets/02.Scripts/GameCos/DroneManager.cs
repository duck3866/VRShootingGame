using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    [SerializeField] private float minTime = 1;
    [SerializeField] private float maxTime = 5;
    private float _createTime;
    private float _currentTime;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject droneFactory;
    
    // Start is called before the first frame update
    void Start()
    {
        _createTime = Random.Range(minTime, maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > _createTime)
        {
            GameObject drone = Instantiate(droneFactory);
            int index = Random.Range(0, spawnPoints.Length);
            drone.transform.position = spawnPoints[index].position;
            _currentTime = 0;
            _createTime = Random.Range(minTime, maxTime);
        }
    }
}
