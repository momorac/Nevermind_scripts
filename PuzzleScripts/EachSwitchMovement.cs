using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EachSwitchMovement : MonoBehaviour
{
    public bool isActivated;

    public GameObject button;
    private Transform btn_position;
    private Renderer btn_render;
    public Color targetEmissionColor = Color.green; // 목표 Emission Color
    public AudioSource clickSound;

    void Awake()
    {
        btn_position = button.GetComponent<Transform>();
        btn_render = button.GetComponent<Renderer>();
    }

    void Start()
    {
        isActivated = false;
        btn_render.material.EnableKeyword("_EMISSION");
    }


    public void ActivateSwitch()
    {
        PlaySound();
        btn_position.transform.DOLocalMoveY(0f, 0.5f);
        btn_render.material.DOColor(targetEmissionColor, "_EmissionColor", 1f);
        isActivated = true;
    }

    public void UnActivateSwitch()
    {
        PlaySound();
        btn_position.transform.DOLocalMoveY(0.16f, 0.5f);
        btn_render.material.DOColor(Color.black, "_EmissionColor", 1f);
        isActivated = false;
    }

    private void PlaySound()
    {
        clickSound.Play();
    }


    
}
