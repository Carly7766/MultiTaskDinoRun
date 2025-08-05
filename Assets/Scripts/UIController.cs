using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject TitleText;
    [SerializeField] private GameObject RetryGuideText;

    private void Awake()
    {
        TitleText.SetActive(true);
        RetryGuideText.SetActive(false);
    }

    private void Update()
    {
        if (scoreText.isActiveAndEnabled)
        {
            scoreText.text = $"HI {MainSceneDataStore.Instance.highScore:00000} {MainSceneDataStore.Instance.score:00000}";
        }
    }

    public void TitleToIngame()
    {
        TitleText.SetActive(false);
    }

    public void IngameToGameOver()
    {
        RetryGuideText.SetActive(true);
    }
}