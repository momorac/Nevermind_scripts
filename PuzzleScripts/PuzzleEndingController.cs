using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleEndingController : MonoBehaviour
{
    [Header("Scene Control")]
    public LoadingSceneController loadingSceneController;
    public CinemachineVirtualCamera ClearedVirtualCamera;
    public ClickMovement characterController;
    public Animator characterAnimator;
    public GameObject playerCharacter;
    public Volume volume;
    public GameObject EndUI;


    [Space(10)]
    [Header("Set Plug State")]
    public GameObject PlugObject;
    public AudioSource AttachSound;
    public Vector3 PlugAttached_position;
    public Quaternion PlugAttached_rotation;

    [Space(10)]
    [Header("Particle Effects Control")]
    public GameObject LampUpEffect;
    public GameObject SuccessParticleEffect;
    public GameObject NoiseParticleEffect;

    [Space(10)]
    [Header("AudioSources")]
    public AudioSource lightOnSound;
    public AudioSource SuccessEffectSound;


    void Start()
    {
        // 엔딩씬 버추얼 카메라 활성화
        ClearedVirtualCamera.Priority = 10;
        characterController.isEnded = true;


        // 캐릭터 컨트롤 비활성화
        Debug.Log("Set Controller Unable");
        characterController.agent.ResetPath(); // 이동 경로 초기화
        characterController.agent.isStopped = true;
        characterController.agent.enabled = false;

        characterController.enabled = false;

        // 캐릭터 기뻐하는 애니메이션
        playerCharacter.transform.rotation = Quaternion.Euler(0, 180, 0);
        characterAnimator.SetTrigger("EndTrigger");

        // 플러그 최종상태로 변경
        PlugObject.transform.SetParent(null);

        PlugObject.transform.position = PlugAttached_position;
        PlugObject.transform.rotation = PlugAttached_rotation;
        AttachSound.Play();


        StartCoroutine(SuccessSequence());
    }

    private IEnumerator SuccessSequence()
    {
        // 버추얼카메라 트랜지션을 위채 2초간 대기
        yield return new WaitForSeconds(2);

        lightOnSound.Play();
        NoiseParticleEffect.SetActive(false);
        LampUpEffect.SetActive(true);
        yield return new WaitForSeconds(0.37f);
        ChangeAmbient();
        yield return new WaitForSeconds(0.05f);
        EndUI.SetActive(true);
        SuccessParticleEffect.SetActive(true);
        SuccessEffectSound.Play();

        yield break;
    }

    private float timer = 0;
    private bool isSceneChanged = false;
    private void Update()
    {
        timer += Time.deltaTime;
        if (Input.anyKeyDown && timer > 3.5f && !isSceneChanged)
        {
            isSceneChanged = true;
            loadingSceneController.ChangeScene();
        }
    }

    private void ChangeAmbient()
    {
        VolumeProfile profile = volume.profile;

        // 채도, 밝기 조정
        if (profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            colorAdjustments.saturation.overrideState = true; // 값을 수정 가능하게 만듦
            colorAdjustments.saturation.value = 0f;

            colorAdjustments.postExposure.overrideState = true;
            colorAdjustments.postExposure.value = 0.5f;
        }

        // 노이즈 비활성화
        if (profile.TryGet(out ChromaticAberration chromatic))
        {
            chromatic.intensity.overrideState = true;
            chromatic.intensity.value = 0f;
        }

        // 비네트 비활성화
        if (profile.TryGet(out Vignette vignette))
        {
            vignette.intensity.overrideState = true;
            vignette.intensity.value = 0;
        }

    }

}
