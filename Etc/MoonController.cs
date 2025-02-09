using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class MoonController : MonoBehaviour
{
    public Transform character;
    public GameObject characterMoonPosition;
    public Image moonBubble;
    public TMP_Text moonBubble_text;

    public GameObject targetCrystal;

    void Awake()
    {
    }

    void Start()
    {
        targetCrystal = null;
        moonBubble.color = new Color(0, 0, 0, 0);
        moonBubble_text.color = new Color(0, 0, 0, 0);
    }

    void FixedUpdate()
    {
        transform.LookAt(character);

        if (targetCrystal == null) return;


        // 타겟 크리스탈 위치로 이동
        float targetdistance = Vector3.Distance(character.position, transform.position);

        transform.position = Vector3.Lerp(transform.position, targetCrystal.transform.position, 0.5f * Time.deltaTime);
    }

    public void MoonMoveToCharacter()
    {
        transform.position = characterMoonPosition.transform.position;
        moonBubble.DOColor(Color.white, 1f);
        moonBubble_text.DOColor(Color.black, 1f);
        moonBubble_text.text = "가로등을 수리할 수 있게 되었어. 하늘을 보고 나를 따라와!";
    }

    public void UnActivateBubble()
    {
        moonBubble.color = new Color(0, 0, 0, 0);
        moonBubble_text.color = new Color(0, 0, 0, 0);
    }

    public void ActivateBubble()
    {
        moonBubble.DOColor(Color.white, 1f);
        moonBubble_text.DOColor(Color.black, 1f);
    }

}
