using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour, IDamagable
{
    [SerializeField] private Collider[] colliders;

    public void TakeDamage(float damage)
    {
               
    }

    public void HitPoint(Vector3 hitPoint)
    {
        Debug.Log($"맞기는 맞았는데요 {gameObject.name}");
        foreach (var collider in colliders)
        {
            if (collider.bounds.Contains(hitPoint))
            {
                if (collider == colliders[0])
                {
                    ReStartGame();
                }
                else if (collider == colliders[1])
                {
                    EndGame();
                }
            }
        }
    }

    public void ReStartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndGame()
    {
        Debug.Log("종료");
        Application.Quit();
    }
}   
