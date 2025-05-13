using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneUI : MonoBehaviour, IDamagable
{
    [SerializeField] private Material [] materials = new Material[2];
    [SerializeField] private Collider[] colliders;
    [SerializeField] private GameObject imageOne;
    [SerializeField] private GameObject imageTwo;
    private float _imageIndex = 0;

    private void Start()
    {
        _imageIndex = 0;
        imageTwo.SetActive(false);
    }

    private void GameStart()
    {
        SceneManager.LoadScene(1);
    }
    private void ImageLoad(float value)
    {
        if (_imageIndex != 0 && value > 0)
        {
            _imageIndex--;
            imageOne.SetActive(true);
            imageTwo.SetActive(false);
        }
        else if (_imageIndex == 0 && value < 0)
        {
            _imageIndex++;
            imageOne.SetActive(false);
            imageTwo.SetActive(true);
        }
    }

    public void TakeDamage(float damage)
    {
        
    }

    private IEnumerator ChangeColor(GameObject button)
    {
        Material Material = button.GetComponent<Renderer>().material;
        Material.color = materials[0].color;
        yield return new WaitForSecondsRealtime(0.5f);
        Material.color = materials[1].color;
    }
    public void HitPoint(Vector3 hitPoint)
    {
        foreach (var collider in colliders)
        {
            if (collider.bounds.Contains(hitPoint))
            {
                if (collider == colliders[0])
                {
                    ImageLoad(1);
                }
                else if (collider == colliders[1])
                {
                    ImageLoad(-1);
                }
                else if (collider == colliders[2])
                {
                   GameStart();
                }
                StartCoroutine(ChangeColor(collider.gameObject));
            }
        }
    }
}
