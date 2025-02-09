using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class EndingSceneScript : MonoBehaviour
{
    public float colorChangeDuration = 2.0f;
    public float ambientChangeDuration = 3.0f;
    public AudioSource music;

    [Header("Virtual Cameras")]
    public GameObject VCamera_first;
    public GameObject VCamera_second;
    public GameObject VCamera_third;
    public GameObject VCamera_final;
    public int cameraCount = 0;


    [Space(10)]
    [Header("Final Credit")]
    public string[] credit_texts;
    public GameObject creditText;
    [SerializeField]
    private int currentCreditCount;


    [Space(10)]
    [Header("Sky&Lights")]
    public Material skyBox_Material;
    private float skybox_InitValue = 1.3f;
    public float skybox_TargetValue;

    [Space(10)]
    public Material skyHalo_Material;
    public Color skyHalo_InitialColor;
    public Color skyHalo_TargetColor;
    public Renderer SkyHalo_Overlay;

    [Space(10)]
    public GameObject light_first;
    public Color firstLight_InitialColor;
    public Color firstLight_TargetColor;

    [Space(10)]
    public GameObject lensFlare;
    public Vector3 lensFlare_InitPosition;
    public Vector3 lensFlare_TargetPosition;


    [Space(10)]
    public GameObject light_second;
    public GameObject Auroras;
    public Color secondLight_InitialColor;
    public Color secondLight_TargetColor;

    [Space(10)]
    public GameObject light_third_point;
    public GameObject light_third_directional;

    [Space(10)]
    public GameObject lights_final;
    public GameObject Stars;
    public GameObject OvalLights;

    [Space(10)]
    [Header("Animators")]
    public GameObject animatorParent_first;
    public Animator lilyAnimation;
    public Animator transitionAnimator;


    [Space(10)]
    [Header("Color Changing Materials")]
    [Space(10)]
    public Material firstFloor_Material;
    public Color firstFloor_InitialColor;
    public Color firstFloor_TargetColor;
    [Space(10)]
    public Material terrain_Material;
    public Color terrain_InitialColor;
    public Color terrain_TargetColor;

    [Space(10)]
    public Material firstTree_Material;
    public Color firstTree_InitialColor;
    public Color firstTree_TargetColor;
    [Space(10)]
    public Material secondTree_Material;
    public Color secondTree_InitialColor;
    public Color secondTree_TargetColor;
    [Space(10)]
    public Material thirdTree_Material;
    public Color thirdTree_InitialColor;
    public Color thirdTree_TargetColor;
    [Space(10)]
    public Material grass_Material;
    public Color grass_InitialColor;
    public Color grass_TargetColor;


    void Start()
    {

        skyBox_Material.SetFloat("_D2I", 1.3f);
        skyHalo_Material.color = new Color(0, 0, 0, 0);

        light_first.GetComponent<Light>().color = firstLight_InitialColor;
        lensFlare.transform.position = lensFlare_InitPosition;

        currentCreditCount = 0;

    }

    private void OnApplicationQuit()
    {
        firstFloor_Material.color = firstFloor_InitialColor;
        terrain_Material.color = terrain_InitialColor;
        firstTree_Material.color = firstTree_InitialColor;
        secondTree_Material.color = secondTree_InitialColor;
        thirdTree_Material.color = thirdTree_InitialColor;
        grass_Material.color = grass_InitialColor;

        skyBox_Material.SetFloat("_D2I", skybox_InitValue);
    }
    private void OnDestroy()
    {
         firstFloor_Material.color = firstFloor_InitialColor;
        terrain_Material.color = terrain_InitialColor;
        firstTree_Material.color = firstTree_InitialColor;
        secondTree_Material.color = secondTree_InitialColor;
        thirdTree_Material.color = thirdTree_InitialColor;
        grass_Material.color = grass_InitialColor;

        skyBox_Material.SetFloat("_D2I", skybox_InitValue);
    }

    private void Update()
    {

        // if (Input.GetMouseButtonDown(0))
        // {
        //     StartCoroutine("SkyAmbientChange");
        //     StartCoroutine("LightingChange");
        // }

    }

    // Change skybox Material Intensity & halo effect Intensity 
    private IEnumerator SkyAmbientChange(int count)
    {
        float elapsedTime = 0f;

        // 1: 첫번째 씬에서 하늘 밝아지는 연출
        if (count == 1)
        {
            skyBox_Material.SetFloat("_D2I", skybox_InitValue);
            skyHalo_Material.color = new Color(0, 0, 0, 0);

            while (elapsedTime < ambientChangeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / ambientChangeDuration;

                float newSkyBox = Mathf.Lerp(skybox_InitValue, skybox_TargetValue, t);
                skyBox_Material.SetFloat("_D2I", newSkyBox);

                skyHalo_Material.color = Color.Lerp(skyHalo_InitialColor, skyHalo_TargetColor, t);

                yield return null;
            }
        }

        else if (count == 2)
        {
            SkyHalo_Overlay.material.color = Color.white;
            skyBox_Material.SetFloat("_D2I", 0.99f);
            skyHalo_Material.color = skyHalo_TargetColor;

            while (elapsedTime < 8)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / 8;

                float newSkyBox = Mathf.Lerp(0.99f, 1.3f, t);
                skyBox_Material.SetFloat("_D2I", newSkyBox);

                skyHalo_Material.color = Color.Lerp(skyHalo_TargetColor, new Color(0, 0, 0, 0), t);

                SkyHalo_Overlay.material.color = Color.Lerp(new Color(0.3f, 0.6f, 1f), new Color(0, 0, 0, 0), t);
                yield return null;
            }
        }

    }

    // Light Ambient & Lens Flare Effect Change
    private IEnumerator LightingChange()
    {
        float elapsedTime = 0f;

        // 첫번째 씬 라이팅 세팅
        if (cameraCount == 0)
        {
            // 첫번째씬 light 오브젝트 활성화
            light_first.SetActive(true);
            Light firstLight = light_first.GetComponent<Light>();
            firstLight.color = firstLight_InitialColor;

            // 첫번째씬 lensFlare 오브젝트 활성화
            lensFlare.SetActive(true);
            lensFlare.transform.position = lensFlare_InitPosition;
            LensFlareComponentSRP lf = lensFlare.GetComponent<LensFlareComponentSRP>();

            // 코루틴으로 라이트 컬러 변경 & 렌즈플레어 위치/강도 변경
            while (elapsedTime < ambientChangeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / ambientChangeDuration;

                firstLight.color = Color.Lerp(firstLight_InitialColor, firstLight_TargetColor, t);
                lensFlare.transform.position = Vector3.Lerp(lensFlare_InitPosition, lensFlare_TargetPosition, t);
                lf.intensity = Mathf.Lerp(0, 1, t);

                yield return null;
            }
        }


        // 두번째 씬 라이팅 세팅
        else if (cameraCount == 1)
        {
            light_first.SetActive(false);
            light_second.SetActive(true);

            skyBox_Material.SetFloat("_D2I", 0.98f);
            Auroras.SetActive(true);

            Light secondLight = light_second.GetComponent<Light>();

            while (elapsedTime < ambientChangeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / ambientChangeDuration;

                secondLight.color = Color.Lerp(secondLight_InitialColor, secondLight_TargetColor, t);

                yield return null;
            }
        }

        // 세번째 씬 라이팅 세팅
        else if (cameraCount == 2)
        {
            light_second.SetActive(false);
            lensFlare.SetActive(false);
            Auroras.SetActive(false);

            light_third_point.SetActive(true);
            light_third_directional.SetActive(true);
        }


        // 파이널씬 라이팅 세팅
        else if (cameraCount == 3)
        {
            light_third_point.SetActive(false);
            Light thirdSceneLight = light_third_directional.GetComponent<Light>();

            StartCoroutine(FinalScene_LightFade());

            while (thirdSceneLight.intensity > 0.15f)
            {
                thirdSceneLight.intensity -= 0.02f;

                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private IEnumerator FinalScene_LightFade()
    {

        // 부모 오브젝트의 모든 자식 오브젝트 탐색
        List<Light> lights = new List<Light>();

        foreach (Transform child in lights_final.transform)
        {
            Light childLight = child.GetComponent<Light>();
            if (childLight != null && childLight.type == LightType.Point)
            {
                childLight.intensity = 0;
                lights.Add(childLight);
            }
        }

        lights_final.SetActive(true);
        float elapsedTime = 0;

        while (elapsedTime < 2)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / 2;

            foreach (Light light in lights)
            {
                light.intensity = Mathf.Lerp(0, 1, t);
            }
            yield return null;
        }
    }



    private IEnumerator GroundColorChange()
    {
        Debug.Log("Color Change Coroutine Start");
        float elapsedTime = 0f;

        while (elapsedTime < colorChangeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / colorChangeDuration;

            // Lerp를 통해 색상을 서서히 변경
            firstFloor_Material.color = Color.Lerp(firstFloor_InitialColor, firstFloor_TargetColor, t);
            terrain_Material.color = Color.Lerp(terrain_InitialColor, terrain_TargetColor, t);
            firstTree_Material.color = Color.Lerp(firstTree_InitialColor, firstTree_TargetColor, t);
            secondTree_Material.color = Color.Lerp(secondTree_InitialColor, secondTree_TargetColor, t);
            thirdTree_Material.color = Color.Lerp(thirdTree_InitialColor, thirdTree_TargetColor, t);
            grass_Material.color = Color.Lerp(grass_InitialColor, grass_TargetColor, t);



            yield return null;
        }

        firstFloor_Material.color = firstFloor_TargetColor;
        terrain_Material.color = terrain_TargetColor;
        firstTree_Material.color = firstTree_TargetColor;
        secondTree_Material.color = secondTree_TargetColor;
        thirdTree_Material.color = thirdTree_TargetColor;
        grass_Material.color = grass_TargetColor;
    }

    private void SetAnimation()
    {
        for (int i = 0; i < animatorParent_first.transform.childCount; i++)
        {
            // 현재 자식 오브젝트 가져오기
            Transform child = animatorParent_first.transform.GetChild(i);

            Transform firstChild = child.GetChild(0);

            // 첫 번째 자식 오브젝트의 Animator 컴포넌트 가져오기
            Animator animator = firstChild.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("RestartTrigger");
            }
        }

        if (cameraCount == 2)
        {
            lilyAnimation.SetTrigger("RestartTrigger");
        }
    }


    private void ChangeCameraScene()
    {
        switch (cameraCount)
        {
            case 0:
                SetAnimation();
                StartCoroutine(GroundColorChange());
                StartCoroutine(SkyAmbientChange(1));
                StartCoroutine(LightingChange());
                VCamera_first.SetActive(true);
                cameraCount++;
                break;
            case 1:
                VCamera_first.SetActive(false);
                SetAnimation();
                StartCoroutine(LightingChange());
                SkyHalo_Overlay.material.DOColor(Color.blue, 4f);
                StartCoroutine(GroundColorChange());
                VCamera_second.SetActive(true);
                cameraCount++;
                break;
            case 2:
                VCamera_second.SetActive(false);

                SetAnimation();
                StartCoroutine(GroundColorChange());
                StartCoroutine(SkyAmbientChange(2));
                StartCoroutine(LightingChange());

                VCamera_third.SetActive(true);
                cameraCount++;
                break;
            case 3:
                VCamera_final.SetActive(true);

                StartCoroutine(LightingChange());
                Stars.GetComponent<Animator>().enabled = true;
                OvalLights.SetActive(true);

                break;
        }
    }

    IEnumerator EndingCreditRoll()
    {
        if (currentCreditCount == credit_texts.Length)
        {
            music.DOFade(0, 3f);
            DOVirtual.DelayedCall(4f, () =>
                {
                    GameManager.Instance.TutorialCompleted = false;
                    GameManager.Instance.playerHP = 100;
                    GameManager.Instance.crystalCount = 0;
                    GameManager.Instance.puzzleStage = 0;

                    GameManager.Instance.heal = 0;
                    GameManager.Instance.recharge = 0;
                    GameManager.Instance.bomb = 0;
                    GameManager.Instance.speedUp = 0;
                    GameManager.Instance.shield = 0;

                    GameManager.Instance.CharacterPosition = new Vector3(0, 4.4f, 0);

                    SceneManager.LoadScene("TitleScene");
                });
        }

        float elapsedTime = 0;

        TMP_Text txt = creditText.GetComponent<TMP_Text>();
        txt.text = credit_texts[currentCreditCount];
        currentCreditCount++;

        while (elapsedTime < 0.7f)
        {
            elapsedTime += Time.deltaTime;

            txt.color = Color.Lerp(new Color(0, 0, 0, 0), Color.white, elapsedTime / 0.7f);
            yield return null;
        }
        while (elapsedTime < 1.8f)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        while (elapsedTime < 2.5f)
        {
            elapsedTime += Time.deltaTime;

            txt.color = Color.Lerp(Color.white, new Color(0, 0, 0, 0), elapsedTime - 1.8f / 0.7f);

            yield return null;
        }

        StartCoroutine(EndingCreditRoll());
    }

}
