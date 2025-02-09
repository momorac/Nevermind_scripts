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


    [SerializeField] private GameObject tutorialManager;
    [SerializeField] private GameObject gameoverManager;
    [SerializeField] private SparkeySpawner sparkeySpawner;
    [SerializeField] private LoadingSceneController loadingSceneController;
    [Space(5)]

    [Header("current status field")]
    public GameObject Player;
    [Range(0, 100)]
    public float playerHP;  //플레이어 현재 체력
    public float skillGage = 0f;
    public int crystalRemain;
    public int crystalCount = 0;
    public int SparkCount = 0;
    public int puzzleStage = 0;
    [Space(5)]
    public bool isClosedToLight = false;
    public bool isPuzzleOpened = false;
    public bool isCharacterPowerMode = false;
    public bool TutorialCompleted = false;
    [SerializeField] private bool isProtected = false;
    public int item_index = 0;

    [Space(10)]
    [Header("stat value")]
    public float skillChargeAmount;
    public float SparkAtkAmount = 0.01f;
    public float effectDuration = 5f;
    public int puzzleUnlockCount;

    [Space(5)]
    [Header("Item Count")]
    public int heal = 0;
    public int recharge = 0;
    public int bomb = 0;
    public int speedUp = 0;
    public int shield = 0;
    public int healAmount = 50;

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
    [HideInInspector] public Vector3 CharacterPosition = new Vector3(0, 4.4f, 0);

    // system reference variable
    private bool onPuzzle = false;
    private FirstPersonController playerController;

    private float walkSpeed_origin;
    private float jumpForce_origin;
    private Vector3 initScale;

    [HideInInspector] public int EffectCount = 3;

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

        // GameEventManager 이벤트 구독
        GameEventManager.Instance.OnPlayerPowerMode += OnPlayerPowerMode;
    }

    void Start()
    {
        // 캐릭터 기본 state 초기화
        walkSpeed_origin = playerController.walkSpeed;
        jumpForce_origin = playerController.jumpForce;
        initScale = Player.transform.localScale;

        skillGage = 100f;
        playerHP = 100;

        isPuzzleOpened = false;
        moonController.UnActivateBubble();


        // 메인 크리스탈 오브젝트들 linkedlist에 할당                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
        for (int i = 0; i < 7; i++)
        {
            maincrystalList.AddLast(mainCrystalObjects[i]);
        }

        // 이전 저장된 플레이어 위치 불러오기
        if (CharacterPosition != null)
            Player.transform.position = CharacterPosition;

        // 튜토리얼 완료 시 매니저 해제
        if (TutorialCompleted)
        {
            Destroy(tutorialManager);
        }

    }


    void Update()
    {
        // skil charge control
        if (skillGage < 100)
        {
            skillGage += skillChargeAmount * Time.deltaTime;
            GameEventManager.Instance.SkillGageChanged(skillGage);
        }
        else
        {
            skillGage = 100;
            GameEventManager.Instance.SkillGageChanged(skillGage);
        }


        // item usage
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameEventManager.Instance.ItemChanged(++item_index);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (item_index == 0) ItemUse("heal");
            else if (item_index == 1) ItemUse("recharge");
            else if (item_index == 2) ItemUse("bomb");
            else if (item_index == 3) ItemUse("speedUp");
            else if (item_index == 4) ItemUse("shield");
        }

        // puzzlie scene load
        if (isClosedToLight && isPuzzleOpened)
        {
            if (!onPuzzle && Input.GetKeyDown(KeyCode.Return))
            {
                LoadPuzzleScene();
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

        // 플레이어 체력 소진 시 게임오버 진입
        if (playerHP < 0.1f)
        {
            gameoverManager.SetActive(true);
        }


        Debug.Log("크리스탈카운트:" + crystalCount + " 퍼즐스테이지:" + puzzleStage);

        // 일정 개수 이상 크리스탈 파괴 시 퍼즐게임 씬 진입하는 코드
        if (!isPuzzleOpened && (crystalCount >= (puzzleStage + 1) * puzzleUnlockCount))
        {
            isPuzzleOpened = true;
            GameEventManager.Instance.PuzzleOpened();

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

        else if (isPuzzleOpened && target != null)
        {
            float distance = Vector3.Distance(Player.transform.position, target.transform.position);

            if (distance < 1.5f)
            {
                isClosedToLight = true;
                GameEventManager.Instance.PlayerClosedToLight(isClosedToLight);
                moonController.UnActivateBubble();
            }
            else
            {
                isClosedToLight = false;
                GameEventManager.Instance.PlayerClosedToLight(isClosedToLight);
                moonController.ActivateBubble();
            }
        }
    }

    private void LoadPuzzleScene()
    {
        isPuzzleOpened = false;
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

    public void Attacked()
    {
        if (isProtected) return;
        GameEventManager.Instance.PlayerHPChanged(playerHP - SparkAtkAmount);
        playerHP -= SparkAtkAmount;
    }

    public void UnAttacked()
    {
        if (onPuzzle)
            return;
        GameEventManager.Instance.PlayerHPChanged(playerHP);

    }

    //크리스탈 파괴 시 캐릭터 효과 부여
    public void CrystalBreakEffect()
    {
        Debug.Log("Crystal Destroyed!");

        int seed = Random.Range(0, 100);
        if (seed >= 60 && seed < 64)
        {
            OnItemGet("heal");
            GameEventManager.Instance.ItemGet("heal");
        }
        else if (seed >= 65 && seed < 70)
        {
            OnItemGet("recharge");
            GameEventManager.Instance.ItemGet("recharge");
        }
        else if (seed >= 70 && seed < 75)
        {
            OnItemGet("bomb");
            GameEventManager.Instance.ItemGet("bomb");
        }
        else if (seed >= 75 && seed < 80)
        {
            OnItemGet("speedUp");
            GameEventManager.Instance.ItemGet("speedUp");
        }
        else if (seed >= 80 && seed < 85)
        {
            OnItemGet("shield");
            GameEventManager.Instance.ItemGet("shield");
        }
        else if (seed >= 96 && seed < 100)
        {
            Debug.Log("Power Mode!!");
            if (!isCharacterPowerMode)
                GameEventManager.Instance.PlayerPowerMode();
        }
    }

    // 아이템 획득 메소드
    private void OnItemGet(string itemName)
    {
        switch (itemName)
        {
            case "heal":
                Debug.Log("getHeal");
                heal++;
                break;
            case "recharge":
                Debug.Log("getRechrge");
                recharge++;
                break;
            case "bomb":
                Debug.Log("getBomb");
                bomb++;
                break;
            case "speedUp":
                Debug.Log("getSpeedUp");
                speedUp++;
                break;
            case "shield":
                Debug.Log("getShield");
                shield++;
                break;
        }
    }


    private void ItemUse(string itemName)
    {
        switch (itemName)
        {
            case "heal":
                if (heal <= 0) break;

                heal--;
                healEffect.SetActive(false);
                healEffect.SetActive(true);

                playerHP += healAmount;
                if (playerHP > 100) playerHP = 100;
                GameEventManager.Instance.ItemUse("heal");
                break;

            case "recharge":
                if (recharge <= 0) return;

                recharge--;
                chargeEffect.SetActive(false);
                chargeEffect.SetActive(true);

                skillGage = 100f;
                GameEventManager.Instance.ItemUse("recharge");
                break;

            case "bomb":
                if (bomb <= 0) return;

                bomb--;
                Instantiate(bombPrefab, Player.transform.position, quaternion.identity);
                GameEventManager.Instance.ItemUse("bomb");
                break;

            case "speedUp":

                if (speedUp <= 0) return;

                speedUp--;
                speedUpEffect.SetActive(false);
                speedUpEffect.SetActive(true);

                playerController.walkSpeed = walkSpeed_origin * 2;
                GameEventManager.Instance.ItemUse("speedUp");

                Invoke("InitStat", 5f);
                break;

            case "shield":

                if (shield <= 0) break;

                shield--;
                isProtected = true;
                shieldEffect.SetActive(false);
                shieldEffect.SetActive(true);
                GameEventManager.Instance.ItemUse("speedUp");

                Invoke("InitStat", 5f);
                break;
        }
    }


    // 캐릭터 파워모드 메소드
    private void OnPlayerPowerMode()
    {
        Player.transform.localScale = initScale * 2;

        powerEffect.SetActive(false);
        powerEffect.SetActive(true);

        isCharacterPowerMode = true;
        Invoke("InitStat", 3f); // powermode duration
    }

    public void InitStat()
    {
        playerController.walkSpeed = walkSpeed_origin;
        playerController.jumpForce = jumpForce_origin;
        Player.transform.localScale = initScale;
        isProtected = false;
        isCharacterPowerMode = false;
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

}
