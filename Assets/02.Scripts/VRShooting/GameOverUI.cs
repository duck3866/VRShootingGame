using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour, IDamagable
{
    [SerializeField] private Collider[] colliders = new Collider[2];
    [SerializeField] private Material[] materials =  new Material[2];

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

                ChangeColor(collider.gameObject);
            }
        }
    }

    private void ReStartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
        Application.Quit();
    }
    private IEnumerator ChangeColor(GameObject button)
    {
        Material Material = button.GetComponent<Renderer>().material;
        Material.color = materials[0].color;
        yield return new WaitForSecondsRealtime(0.5f);
        Material.color = materials[1].color;
    }
}   
