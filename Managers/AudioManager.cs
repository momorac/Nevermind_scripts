using System;
using System.Collections;
using System.Collections.Generic;
using Obi;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("effect audio source")]
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource powerModeSound;
    [SerializeField] private AudioSource itemGetSound;
    [SerializeField] private AudioSource healSound;
    [SerializeField] private AudioSource speedUpSound;
    [SerializeField] private AudioSource chargeSound;
    [SerializeField] private AudioSource puzzleOpenSound;

    // 플레이어 데미지 입었을 시 사운드 출력
    private void OnAttacked(float newHP)
    {
        if (GameManager.Instance.playerHP > newHP && !hitSound.isPlaying)
        {
            hitSound.Play();
        }
    }

    // 아이템 획득시 효과음 출력
    private void OnItemGet(string itemName)
    {
        itemGetSound.Play();
    }

    // 아이템 사용 시 효과음 출력
    private void OnItemUsed(string itemName)
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

    // 플레이어 파워모드 시 효과음 출력
    private void OnPlayerPowermode()
    {
        powerModeSound.Play();
    }

    private void Awake()
    {
        GameEventManager.Instance.OnPlayerHPChanged += OnAttacked;
        GameEventManager.Instance.OnItemGet += OnItemGet;
        GameEventManager.Instance.OnItemUsed += OnItemUsed;
        GameEventManager.Instance.OnPlayerPowermode += OnPlayerPowermode;
    }

    private void Start()
    {
    }
}
