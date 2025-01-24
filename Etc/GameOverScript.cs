using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public Image blackOverlay;
    public TMP_Text text1;
    public TMP_Text text2;
    public TMP_Text text3;

    private float timer;
    private bool hasChanged;


    void Start()
    {
        text1.color = new Color(0, 0, 0, 0);
        text2.color = new Color(0, 0, 0, 0);
        text3.color = new Color(0, 0, 0, 0);
        blackOverlay.color = new Color(0, 0, 0, 0);

        text1.DOColor(Color.white, 1f);
        text2.DOColor(Color.white, 1f);
        text3.DOColor(Color.white, 1f);
        blackOverlay.DOColor(Color.black, 1f);

        timer = 6f;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        text3.text = timer.ToString("F0") + "초 후에 메인화면으로 이동합니다";

        if (timer <= 0.1f && !hasChanged)
        {
            hasChanged = true;
            SceneManager.LoadScene("TitleScene");
        }

    }
}
