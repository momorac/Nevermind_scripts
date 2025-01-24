using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class SplashStarter : MonoBehaviour
{
    private Image blackImage;
    public TMP_Text text;

    public void Awake()
    {
        blackImage = GetComponent<Image>();
    }

    public void Start()
    {
        blackImage.color = Color.black;
        text.color = Color.white;
        StartCoroutine(Splash());
    }

    private IEnumerator Splash()
    {
        yield return new WaitForSeconds(1);

        blackImage.DOColor(new Color(0, 0, 0, 0), 1.5f);
        text.DOColor(new Color(1, 1, 1, 0), 1.5f);

        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

}
