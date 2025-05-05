using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Image RightHand;
    public TextMeshProUGUI  rightHandText;
    public TextMeshProUGUI  rightHandInfoText;
    public Image LeftHand;
    public TextMeshProUGUI  leftHandText;
    public TextMeshProUGUI  leftHandInfoText;
    public GameObject playerStats;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RightHandInfoUpdate(string text,string textInfo)
    {
        rightHandText.text = text;
        rightHandInfoText.text = textInfo;
    }
    public void LeftHandInfoUpdate(string text,string textInfo)
    {
        leftHandText.text = text;
        leftHandInfoText.text = textInfo;
    }
}
