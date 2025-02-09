using System;
using System.Collections;
using System.Collections.Generic;
using Obi;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("character audio source")]
    [SerializeField] private AudioSource spinSound;
    [SerializeField] private AudioSource sparkDieSound;

    [Space(10)]
    [Header("effect audio source")]
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource powerModeSound;
    [SerializeField] private AudioSource itemGetSound;
    [SerializeField] private AudioSource healSound;
    [SerializeField] private AudioSource speedUpSound;
    [SerializeField] private AudioSource chargeSound;

    [Space(10)]
    [Header("alert audio source")]
    [SerializeField] private AudioSource puzzleOpenSound;
    [SerializeField] private AudioSource itemTabSound;



    #region player control
 
    // 플레이어 데미지 입었을 시 사운드 출력
    private void OnPlayerAttacked(float newHP)
    {
        if (GameManager.Instance.playerHP > newHP && !hitSound.isPlaying)
        {
            hitSound.Play();
        }
    }

    private void OnPlayerSpin()
    {
        spinSound.Play();
    }

    #endregion




    #region item&effect
    // 아이템 획득시 효과음 출력
    private void OnItemGet(string itemName)
    {
        itemGetSound.Play();
    }

    // 아이템 사용 시 효과음 출력
    private void OnItemUse(string itemName)
    {
        switch (itemName)
        {
            case "heal":
                healSound.Play();
                break;
            case "speedUp":
                speedUpSound.Play();
                break;
            case "charge":
            case "shield":
                chargeSound.Play();
                break;
            default:
                break;
        }
    }

    // 선택된 아이템 변경 시 효과음 출력
    private void OnItemChanged(int newIndex)
    {
        itemTabSound.Play();
    }

    // 플레이어 파워모드 시 효과음 출력
    private void OnPlayerPowerMode()
    {
        powerModeSound.Play();
    }
    #endregion


    private void OnPuzzleOpened()
    {
        puzzleOpenSound.Play();
    }

    private void OnSparkeyDie()
    {
        sparkDieSound.Play();
    }


    private void Awake()
    {
        // GameEventManager 이벤트 구독
        GameEventManager.Instance.OnPlayerHPChanged += OnPlayerAttacked;
        GameEventManager.Instance.OnPlayerSpin += OnPlayerSpin;
        GameEventManager.Instance.OnPlayerPowerMode += OnPlayerPowerMode;
        GameEventManager.Instance.OnItemGet += OnItemGet;
        GameEventManager.Instance.OnItemUse += OnItemUse;
        GameEventManager.Instance.OnItemChanged += OnItemChanged;
        GameEventManager.Instance.OnPuzzleOpened += OnPuzzleOpened;
        GameEventManager.Instance.OnSparkeyDie += OnSparkeyDie;
    }

    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        // GameEventManager 이벤트 구독해제
        GameEventManager.Instance.OnPlayerHPChanged -= OnPlayerAttacked;
        GameEventManager.Instance.OnPlayerSpin -= OnPlayerSpin;
        GameEventManager.Instance.OnPlayerPowerMode -= OnPlayerPowerMode;
        GameEventManager.Instance.OnItemGet -= OnItemGet;
        GameEventManager.Instance.OnItemUse -= OnItemUse;
        GameEventManager.Instance.OnItemChanged -= OnItemChanged;
        GameEventManager.Instance.OnPuzzleOpened -= OnPuzzleOpened;
        GameEventManager.Instance.OnSparkeyDie -= OnSparkeyDie;


    }
}
