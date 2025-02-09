using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using TMPro;

using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;
using System;

public class UIManager : MonoBehaviour
{
    [Header("UI Component")]
    public Volume volume;
    public Image hpBarImage;
    public Animator hpAnimator;
    public Image skilGageImage;
    [Space(5)]
    public GameObject puzzleEnterLabel;
    public TMP_Text PuzzleLabel;

    [Space(10)]
    [Header("Items Component")]
    public RectTransform[] itemsList = new RectTransform[5];
    [Space(5)]
    public TMP_Text healLabel;
    public TMP_Text rechargeLabel;
    public TMP_Text bombLabel;
    public TMP_Text speedUpLabel;
    public TMP_Text shieldLabel;

    [Header("Item Usage")]
    [Space(5)]
    public Image itemUseEffect_image;
    public TMP_Text itemUseEffect_text;
    public Animator itemUseEffectAnimator;
    public Sprite[] ItemUseImages = new Sprite[5];

    public Image itemGetEffect;
    public Animator itemGetEffectAnimator;
    public Sprite[] ItemGetImages = new Sprite[5];


    [Space(10)]
    [Header("Player HP range")]
    private int highHP = 80;
    private int midHP = 50;
    private int lowHP = 30;
    private int fewHP = 10;

    // system vars
    private ChromaticAberration chromaticAberration;



    // 체력 관련 UI 변경 메소드 - hp에 따라 크로매틱 강도 조절
    private void OnPlayerHPChanged(float newHP)
    {
        if (newHP >= highHP)
        {
            chromaticAberration.intensity.value = 0.0f;
            hpAnimator.SetBool("isLowHp", false);
        }
        else if (newHP >= midHP)
        {
            chromaticAberration.intensity.value = 0.1f;
            hpAnimator.SetBool("isLowHp", false);
        }
        else if (newHP >= lowHP)
        {
            chromaticAberration.intensity.value = 0.5f;
            hpAnimator.SetBool("isLowHp", false);
        }
        else if (newHP >= fewHP)
        {
            chromaticAberration.intensity.value = 1f;
            hpAnimator.SetBool("isLowHp", true);
        }
    }

    // 플레이어 대미지 입었을 시 hpui 애니메이터 설정
    private void OnPlayerAttacked(float newHP)
    {
        hpBarImage.fillAmount = newHP / 100;
        if (newHP < GameManager.Instance.playerHP)
        {
            hpAnimator.SetBool("isAttacked", true);
        }
        else
        {
            hpAnimator.SetBool("isAttacked", false);
        }
    }

    private void OnSkillGageChanged(float newGage)
    {
        skilGageImage.fillAmount = newGage / 100;
    }


    // 아이템 관련 메소드

    private void OnItemGet(string itemName)
    {
        switch (itemName)
        {
            case "heal":
                healLabel.text = GameManager.Instance.heal + "";
                itemGetEffect.sprite = ItemGetImages[0];
                itemGetEffectAnimator.SetTrigger("itemGetTrigger");
                break;
            case "recharge":
                rechargeLabel.text = GameManager.Instance.recharge + "";
                itemGetEffect.sprite = ItemGetImages[1];
                itemGetEffectAnimator.SetTrigger("itemGetTrigger");
                break;
            case "bomb":
                bombLabel.text = GameManager.Instance.bomb + "";
                itemGetEffect.sprite = ItemGetImages[2];
                itemGetEffectAnimator.SetTrigger("itemGetTrigger");
                break;
            case "speedUp":
                speedUpLabel.text = GameManager.Instance.speedUp + "";
                itemGetEffect.sprite = ItemGetImages[3];
                itemGetEffectAnimator.SetTrigger("itemGetTrigger");
                break;
            case "shield":
                shieldLabel.text = GameManager.Instance.shield + "";
                itemGetEffect.sprite = ItemGetImages[4];
                itemGetEffectAnimator.SetTrigger("itemGetTrigger");
                break;

        }
    }

    private void OnItemUse(string itemName)
    {
        switch (itemName)
        {
            case "heal":
                healLabel.text = GameManager.Instance.heal + "";
                itemUseEffect_image.sprite = ItemUseImages[0];
                itemUseEffect_text.text = "체력 회복!";
                itemUseEffectAnimator.SetTrigger("itemUseTrigger");
                hpBarImage.fillAmount = GameManager.Instance.playerHP / 100;
                break;
            case "recharge":
                rechargeLabel.text = GameManager.Instance.recharge + "";
                itemUseEffect_image.sprite = ItemUseImages[1];
                itemUseEffect_text.text = "스킬 쿨타임 회복!";
                itemUseEffectAnimator.SetTrigger("itemUseTrigger");
                break;
            case "bomb":
                bombLabel.text = GameManager.Instance.bomb + "";
                itemUseEffect_image.sprite = ItemUseImages[2];
                itemUseEffect_text.text = " ";
                itemUseEffectAnimator.SetTrigger("itemUseTrigger");
                break;
            case "speedUp":
                speedUpLabel.text = GameManager.Instance.speedUp + "";
                itemUseEffect_image.sprite = ItemUseImages[3];
                itemUseEffect_text.text = "이동속도 증가!";
                itemUseEffectAnimator.SetTrigger("itemUseTrigger");
                break;

            case "shield":
                shieldLabel.text = GameManager.Instance.shield + "";
                itemUseEffect_image.sprite = ItemUseImages[4];
                itemUseEffect_text.text = "보호막 생성!";
                itemUseEffectAnimator.SetTrigger("itemUseTrigger");
                break;
        }
    }

    private void OnItemChanged(int newIndex)
    {
        if (newIndex > 4)
        {
            GameManager.Instance.item_index = 0;
            swapItemPosition(itemsList[4], itemsList[0]);
        }
        else if (newIndex <= 4)
            swapItemPosition(itemsList[newIndex], itemsList[newIndex - 1]);
    }

    private void swapItemPosition(RectTransform a, RectTransform b)
    {
        Vector2 tmp = a.position;
        a.position = b.position;
        b.position = tmp;
    }


    // 퍼즐 씬 관련 이벤트
    private void OnPlayerClosedToLight(bool isClosedToLight)
    {
        if (isClosedToLight)
        {
            puzzleEnterLabel.SetActive(true);
        }
        else
        {
            puzzleEnterLabel.SetActive(false);
        }
    }


    private void Start()
    {
        // 포스트프로세싱 접근
        if (volume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
        {
            OnPlayerHPChanged(GameManager.Instance.playerHP);
        }

        skilGageImage.fillAmount = 1;


        healLabel.text = GameManager.Instance.heal + "";
        speedUpLabel.text = GameManager.Instance.speedUp + "";
        bombLabel.text = GameManager.Instance.bomb + "";
        rechargeLabel.text = GameManager.Instance.recharge + "";
        shieldLabel.text = GameManager.Instance.shield + "";

        puzzleEnterLabel.SetActive(false);
        PuzzleLabel.text = GameManager.Instance.puzzleStage + "/4";
    }

    private void Awake()
    {
        // GameEventManager 이벤트 구독
        GameEventManager.Instance.OnPlayerHPChanged += OnPlayerHPChanged;
        GameEventManager.Instance.OnPlayerHPChanged += OnPlayerAttacked;
        GameEventManager.Instance.OnSkillGageChanged += OnSkillGageChanged;
        GameEventManager.Instance.OnItemGet += OnItemGet;
        GameEventManager.Instance.OnItemUse += OnItemUse;
        GameEventManager.Instance.OnItemChanged += OnItemChanged;
        GameEventManager.Instance.OnPlayerClosedToLight += OnPlayerClosedToLight;

    }

    private void OnDestroy()
    {
        // GameEventManager 이벤트 구독 해제

        GameEventManager.Instance.OnPlayerHPChanged -= OnPlayerHPChanged;
        GameEventManager.Instance.OnPlayerHPChanged -= OnPlayerAttacked;
        GameEventManager.Instance.OnSkillGageChanged -= OnSkillGageChanged;
        GameEventManager.Instance.OnItemGet -= OnItemGet;
        GameEventManager.Instance.OnItemUse -= OnItemUse;
        GameEventManager.Instance.OnItemChanged -= OnItemChanged;
        GameEventManager.Instance.OnPlayerClosedToLight -= OnPlayerClosedToLight;
    }

}
