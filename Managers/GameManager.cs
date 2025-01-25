using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    #region singleton setting
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                // 씬에서 GameManager 찾음
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    // 존재하지 않으면 새로 생성
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                    DontDestroyOnLoad(obj); // 씬 전환 시에도 유지
                }
            }
            return instance;
        }

    }

    #endregion


    [SerializeField]
    [Header("current status field")]
    public GameObject Player;
    [Range(0, 100)]
    public float playerHP;  //플레이어 현재 체력
    public float skillCharged = 0f;
    public int crystalRemain;
    public int crystalCount = 0;
    public int SparkCount = 0;
    public int puzzleStage = 0;
    public bool isPuzzleOpen = false;
    public bool isCharacterPowerMode = false;
    private bool isProtected = false;
    public int item_index = 0;


    [Space(10)]
    [Header("stat value")]
    public float skillChargeAmount;
    public float SparkAtkAmount = 0.01f;
    public float effectDuration = 5f;
    public int puzzleUnlockCount;

    private int highHP = 80;
    private int midHP = 50;
    private int lowHP = 30;
    private int fewHP = 10;

    [Space(5)]
    [Header("Item Count")]
    public int heal = 0;
    public int recharge = 0;
    public int bomb = 0;
    public int speedUp = 0;
    public int shield = 0;
    public int healAmount = 50;

    [Space(5)]
    public AudioSource itemTabSound;
    public Image itemUseEffect_image;
    public TMP_Text itemUseEffect_text;
    public Animator itemUseEffectAnimator;
    public Sprite[] ItemUseImages = new Sprite[5];

    public Image itemGetEffect;
    public Animator itemGetEffectAnimator;
    public Sprite[] ItemGetImages = new Sprite[5];


    [Space(5)]
    public TMP_Text healLabel;
    public TMP_Text rechargeLabel;
    public TMP_Text bombLabel;
    public TMP_Text speedUpLabel;
    public TMP_Text shieldLabel;

    [Space(5)]
    public GameObject bombPrefab;


    [Space(10)]
    [Header("effect particles")]
    public GameObject healEffect;
    public GameObject speedUpEffect;
    public GameObject chargeEffect;
    public GameObject shieldEffect;
    public GameObject powerEffect;

    [Space(10)]
    [Header("puzzle crystal objects")]
    public GameObject[] mainCrystalObjects = new GameObject[7];
    private LinkedList<GameObject> maincrystalList = new LinkedList<GameObject>();
    public MoonController moonController;

    // static fields
    public static bool TutorialCompleted = false;
    public static Vector3 CharacterPosition = new Vector3(0, 4.4f, 0);


    // system reference variable
    private bool onPuzzle = false;

    private FirstPersonController playerController;
    private LoadingSceneController loadingSceneController;
    private ChromaticAberration chromaticAberration;
    private SparkeySpawner sparkeySpawner;

    private float walkSpeed_origin;
    private float jumpForce_origin;

    private Vector3 initScale;

    [HideInInspector]
    public int EffectCount = 3;

    private void Awake()
    {
        // 싱글톤 인스턴스 초기화
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 중복 방지
        }


        playerController = Player.GetComponent<FirstPersonController>();
        loadingSceneController = GetComponent<LoadingSceneController>();
        sparkeySpawner = GetComponent<SparkeySpawner>();
    }

    void Start()
    {
        // 캐릭터 기본 state 초기화
        walkSpeed_origin = playerController.walkSpeed;
        jumpForce_origin = playerController.jumpForce;
        initScale = Player.transform.localScale;

        skillCharged = 100f;
        playerHP = 100;

        // 메인 크리스탈 오브젝트들 linkedlist에 할당                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
        for (int i = 0; i < 7; i++)
        {
            maincrystalList.AddLast(mainCrystalObjects[i]);
        }

        // 포스트프로세싱 접근
        if (volume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
        {
            // 크로매틱 강도를 초기화합니다.
            chromaticAberration.intensity.value = 0.0f;
        }

        // 이전 저장된 씬 값 불러오기
        if (CharacterPosition != null)
            Player.transform.position = CharacterPosition;

        healLabel.text = heal + "";
        speedUpLabel.text = speedUp + "";
        bombLabel.text = bomb + "";
        rechargeLabel.text = recharge + "";
        shieldLabel.text = shield + "";

        if (TutorialCompleted)
        {
            Destroy(tutorialManager);
        }

        isPuzzleOpen = false;
        puzzleEnterLabel.SetActive(false);
        moonController.UnActivateBubble();

        PuzzleLabel.text = puzzleStage + "/4";


    }


    void Update()
    {
        // skil charge control
        skilChargeImage.fillAmount = skillCharged / 100;

        if (skillCharged < 100)
            skillCharged += skillChargeAmount * Time.deltaTime;


        // item usage
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            itemTabSound.Play();
            item_index++;

            if (item_index > 4)
            {
                item_index = 0;
                swapItemPosition(ItemList[4], ItemList[0]);
            }
            else if (item_index <= 4)
                swapItemPosition(ItemList[item_index], ItemList[item_index - 1]);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (item_index == 0) ItemUse_heal();
            else if (item_index == 1) ItemUse_recharge();
            else if (item_index == 2) ItemUse_bomb();
            else if (item_index == 3) ItemUse_speed();
            else if (item_index == 4) StartCoroutine(ItemUse_shield());
        }


        // puzzlie scene load
        if (puzzleEnterLabel.activeSelf && isPuzzleOpen)
        {
            if (!onPuzzle && Input.GetKeyDown(KeyCode.Return))
            {
                PuzzleSceneLoad();
            }
        }



        // -----test code-----
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log(SearchProximateCrystal().gameObject.name);
        }

    }

    private GameObject target = null;
    private void FixedUpdate()
    {
        UnAttacked();
        // hp ui animation setting
        if (playerHP < 0.1f)
        {
            gameoverManager.SetActive(true);
        }
        else if (playerHP >= highHP)
        {
            chromaticAberration.intensity.value = 0.0f;
            UI_HPanime.SetBool("isLowHp", false);
        }
        else if (playerHP >= midHP)
        {
            chromaticAberration.intensity.value = 0.1f;
            UI_HPanime.SetBool("isLowHp", false);
        }
        else if (playerHP >= lowHP)
        {
            chromaticAberration.intensity.value = 0.5f;
            UI_HPanime.SetBool("isLowHp", false);
        }
        else if (playerHP >= fewHP)
        {
            chromaticAberration.intensity.value = 1f;
            UI_HPanime.SetBool("isLowHp", true);
        }


        Debug.Log("크리스탈카운트:" + crystalCount + " 퍼즐스테이지:" + puzzleStage);

        // 일정 개수 이상 크리스탈 파괴 시 퍼즐게임 씬 진입하는 코드
        if (!isPuzzleOpen && (crystalCount >= (puzzleStage + 1) * puzzleUnlockCount))
        {
            isPuzzleOpen = true;
            ///////puzzleOpenSound.Play();

            target = SearchProximateCrystal();

            if (target != null)
            {
                Debug.Log("Puzzle Activated! Lighted up at " + target.name);

                LightObjectAllocator lightObjectAllocator = target.GetComponent<LightObjectAllocator>();
                lightObjectAllocator.lightObject.SetActive(true);

                moonController.MoonMoveToCharacter();
                moonController.targetCrystal = target;

            }
        }

        else if (isPuzzleOpen && target != null)
        {
            float distance = Vector3.Distance(Player.transform.position, target.transform.position);

            if (distance < 1.5f && !puzzleEnterLabel.activeSelf)
            {
                moonController.UnActivateBubble();
                puzzleEnterLabel.SetActive(true);
            }
            else if (distance >= 1.5f && puzzleEnterLabel.activeSelf)
            {
                puzzleEnterLabel.SetActive(false);
            }
        }


    }

    private void PuzzleSceneLoad()
    {
        isPuzzleOpen = false;
        onPuzzle = true;
        CharacterPosition = Player.transform.position;

        switch (puzzleStage)
        {
            case 0:
                loadingSceneController.nextScene = "PuzzleScene_1";
                break;
            case 1:
                loadingSceneController.nextScene = "PuzzleScene_2";
                break;
            case 2:
                loadingSceneController.nextScene = "PuzzleScene_3";
                break;
            case 3:
                loadingSceneController.nextScene = "PuzzleScene_4";
                break;
        }

        puzzleStage++;
        loadingSceneController.ChangeScene();
    }

    private void swapItemPosition(RectTransform a, RectTransform b)
    {
        Vector2 tmp = a.position;
        a.position = b.position;
        b.position = tmp;
    }

    public void Attacked()
    {
        if (isProtected) return;

        playerHP -= SparkAtkAmount;
        hpBarImage.fillAmount = playerHP / 100;
        UI_HPanime.SetBool("isAttacked", true);


    }

    public void UnAttacked()
    {
        if (onPuzzle)
            return;

        UI_HPanime.SetBool("isAttacked", false);
    }

    //크리스탈 파괴 시 캐릭터 효과 부여
    public void CrystalBreakEffect()
    {
        int seed = Random.Range(0, 100);
        Debug.Log("Crystal Destroyed!");
        if (seed >= 60 && seed < 64)
        {
            Effect_getHeal();
        }
        else if (seed >= 65 && seed < 70)
        {
            Effect_getRecharge();
        }
        else if (seed >= 70 && seed < 75)
        {
            Effect_getSpeedUp();
        }
        else if (seed >= 75 && seed < 80)
        {
            Effect_getShield();
        }
        else if (seed >= 80 && seed < 85)
        {
            Effect_getBomb();
        }
        else if (seed >= 96 && seed < 100)
        {
            if (!isCharacterPowerMode)
                Effect_powerMode();
            Debug.Log("Power Mode!!");
        }
    }

    private void Effect_powerMode()
    {
        Player.transform.localScale = initScale * 2;

        powerEffect.SetActive(false);
        powerEffect.SetActive(true);

        //////////powerModeSound.Play();
        isCharacterPowerMode = true;
        Invoke("InitStat", 3f);
    }

    private void Effect_getHeal()
    {
        heal++;
        healLabel.text = heal + "";
        Debug.Log("getHeal");

        itemGetEffect.sprite = ItemGetImages[0];
        itemGetEffectAnimator.SetTrigger("itemGetTrigger");
    }

    private void Effect_getRecharge()
    {
        recharge++;
        rechargeLabel.text = recharge + "";
        Debug.Log("getRechrge");

        itemGetEffect.sprite = ItemGetImages[1];
        itemGetEffectAnimator.SetTrigger("itemGetTrigger");

    }
    private void Effect_getBomb()
    {
        bomb++;
        bombLabel.text = bomb + "";
        Debug.Log("getBomb");

        itemGetEffect.sprite = ItemGetImages[2];
        itemGetEffectAnimator.SetTrigger("itemGetTrigger");

    }
    private void Effect_getShield()
    {
        shield++;
        shieldLabel.text = shield + "";
        Debug.Log("getShield");

        itemGetEffect.sprite = ItemGetImages[4];
        itemGetEffectAnimator.SetTrigger("itemGetTrigger");

    }
    private void Effect_getSpeedUp()
    {
        speedUp++;
        speedUpLabel.text = speedUp + "";

        Debug.Log("getSpeedUp");

        itemGetEffect.sprite = ItemGetImages[3];
        itemGetEffectAnimator.SetTrigger("itemGetTrigger");
    }

    private void ItemUse_heal()
    {
        if (heal <= 0)
            return;

        Debug.Log("Use Heal Item");

        heal--;
        healLabel.text = heal + "";

        itemUseEffect_image.sprite = ItemUseImages[0];
        itemUseEffect_text.text = "체력 회복!";
        itemUseEffectAnimator.SetTrigger("itemUseTrigger");

        healEffect.SetActive(false);
        healEffect.SetActive(true);

        playerHP += healAmount;
        if (playerHP > 100) playerHP = 100;
        hpBarImage.fillAmount = playerHP / 100;

    }

    private void ItemUse_recharge()
    {
        if (recharge <= 0)
            return;

        recharge--;
        rechargeLabel.text = recharge + "";

        itemUseEffect_image.sprite = ItemUseImages[1];
        itemUseEffect_text.text = "스킬 쿨타임 회복!";
        itemUseEffectAnimator.SetTrigger("itemUseTrigger");

        chargeEffect.SetActive(false);
        chargeEffect.SetActive(true);

        skillCharged = 100f;

    }

    private void ItemUse_bomb()
    {
        if (bomb <= 0)
            return;

        bomb--;
        Instantiate(bombPrefab, Player.transform.position, quaternion.identity);

        itemUseEffect_image.sprite = ItemUseImages[2];
        itemUseEffect_text.text = "  ";
        itemUseEffectAnimator.SetTrigger("itemUseTrigger");

        bombLabel.text = bomb + "";

    }
    private void ItemUse_speed()
    {
        if (speedUp <= 0) return;

        speedUp--;
        speedUpLabel.text = speedUp + "";

        itemUseEffect_image.sprite = ItemUseImages[3];
        itemUseEffect_text.text = "이동속도 증가!";
        itemUseEffectAnimator.SetTrigger("itemUseTrigger");

        speedUpEffect.SetActive(false);
        speedUpEffect.SetActive(true);

        playerController.walkSpeed = walkSpeed_origin * 2;
        Invoke("InitStat", 5f);

    }

    private IEnumerator ItemUse_shield()
    {
        if (shield <= 0)
            yield break;

        shield--;
        shieldLabel.text = shield + "";

        itemUseEffect_image.sprite = ItemUseImages[4];
        itemUseEffect_text.text = "보호막 생성!";
        itemUseEffectAnimator.SetTrigger("itemUseTrigger");

        isProtected = true;
        shieldEffect.SetActive(false);
        shieldEffect.SetActive(true);

        yield return new WaitForSeconds(5);
        isProtected = false;
    }



    private GameObject SearchProximateCrystal()
    {
        GameObject proximate = null;
        float min = float.MaxValue;

        foreach (GameObject crystal in maincrystalList)
        {
            float dist = Vector3.Distance(Player.transform.position, crystal.transform.position);
            if (dist < min)
            {
                proximate = crystal;
                min = dist;
            }
        }

        return proximate;
    }


    public void InitStat()
    {
        playerController.walkSpeed = walkSpeed_origin;
        playerController.jumpForce = jumpForce_origin;
        Player.transform.localScale = initScale;
        isCharacterPowerMode = false;
    }

    public void setGetBomb()
    {
        Effect_getBomb();
    }


}
