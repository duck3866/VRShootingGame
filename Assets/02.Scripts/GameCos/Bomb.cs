using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Transform explosion;
    private ParticleSystem expEffect;
    private AudioSource expAudio;

    public float range = 5f;
    
    void Start()
    {
        explosion = GameObject.Find("Explosion").transform;
        expEffect = explosion.GetComponent<ParticleSystem>();
        expAudio = expEffect.GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        int layerMask = 1 << LayerMask.NameToLayer("Drone");
        Collider[] drones = Physics.OverlapSphere(transform.position, range, layerMask);
        foreach (Collider drone in drones)
        {
            Debug.Log("Destroy drone");
            Destroy(drone.gameObject);
        }

        explosion.position = transform.position;
        expEffect.Play();
        expAudio.Play();
        Destroy(gameObject);
    }
}
