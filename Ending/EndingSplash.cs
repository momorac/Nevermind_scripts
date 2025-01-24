using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EndingSplash : MonoBehaviour
{
    public Image blackOverlay;
    public TMP_Text splashText;
    public GameObject endingManager;

    public GameObject secondCamera;

    private void Start()
    {
        StartCoroutine(SplashSequence());
    }



    private IEnumerator SplashSequence()
    {
        yield return new WaitForSeconds(3f);
        blackOverlay.DOColor(Color.black, 0.5f);
        splashText.DOColor(new Color(0, 0, 0, 0), 0.5f);
        yield return new WaitForSeconds(0.5f);
        splashText.text = "행성은 마침내 평화와 온기를 되찾았습니다.";
        secondCamera.SetActive(true);
        blackOverlay.DOColor(new Color(0, 0, 0, 0.2f), 0.5f);
        splashText.DOColor(Color.white, 0.5f);
        yield return new WaitForSeconds(3f);
        blackOverlay.DOColor(Color.black, 1f);
        endingManager.SetActive(true);
        Destroy(gameObject);
        yield break;
    }
}
