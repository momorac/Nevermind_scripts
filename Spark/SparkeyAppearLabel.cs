using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class SparkeyAppearLabel : MonoBehaviour
{

    TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    void Start()
    {
        text.color = new Color(0, 0, 0, 0);
    }

    public void AppearLabel()
    {
        text.DOColor(Color.white, 0.5f);
        DOVirtual.DelayedCall(1.5f, () =>
      {
          text.DOColor(new Color(0, 0, 0, 0), 1f);
      });
    }

}
