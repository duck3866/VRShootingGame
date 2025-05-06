using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject rightHand;
    public TextMeshProUGUI  rightHandText;
    public TextMeshProUGUI  rightHandInfoText;
    public GameObject leftHand;
    public TextMeshProUGUI  leftHandText;
    public TextMeshProUGUI  leftHandInfoText;
    public GameObject playerStats;
    public Image PlayerHpImage;
    public TextMeshProUGUI  playerHpText;
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
        transform.localPosition = Vector3.zero;
        Init();
    }

    public void Init()
    {
        rightHand.transform.localPosition = new Vector3(0.23f, 0, 0.5f);
        rightHandInfoText.transform.localPosition = new Vector3(0, -0.3f, 0);
        leftHand.transform.localPosition = new Vector3(-0.23f, 0, 0.5f);
        leftHandInfoText.transform.localPosition = new Vector3(0, -0.3f, 0);
        playerStats.transform.localPosition = new Vector3(0, 0, 0.6f);
    }

    public void PlayerHPUpdate(float hp, float maxHp)
    {
        PlayerHpImage.fillAmount = hp/maxHp;
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
