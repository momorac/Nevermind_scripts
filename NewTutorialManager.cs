/*
0: 안녕? 나는 달님이야.
1: 이 행성은 지금 위기에 처했어.
2: 원래 평화롭던 행성인데, 어느 날 스파크들이 찾아와 괴롭히기 시작했어.
3: 나를 따라서 이 행성을 구해줘!
4: 마우스를 움직여서 시점을 회전할 수 있어.    ->  mouse ui 보여주기
5: WASD 키를 눌러서 이동할 수 있어.           -> wasd ui 보여주기
6: 곳곳에 자라난 얼음 수정들이 보이지?
7: 가까이 다가가서 마우스 왼쪽 버튼을 클릭해봐!  -> 마우스 좌클릭 ui 보여주기 
                                                 어택 활성화 - controller ontutorial<-false  / count 증가하면 넘어가기
8: 그렇게 수많은 얼음 수정들을 모두 부숴주면 돼.
9: 곳곳에서 스파크가 나타나서 수리공을 공격할거야.
10: 수리공을 상처입게 해서는 안돼!   ->  spark 하나 생성
11: Shift 키를 눌러볼래?     ->  shift ui 보여주기  /  shift키를 누르면 넘어감
12: Shift 키를 누르면 스파크를 무찌르는 스킬을 사용할 수 있어.    ->   spark가 destroy되지 않으면 destroy하기
13: 스킬을 사용하면 오른쪽 위의 게이지가 소모돼.
14: 스킬 게이지는 자동으로 천천히 충전되지만, 수정을 부숴서 더 빨리 충전할 수 있어.
15: 그리고 얼음 수정을 없애다 보면 아이템을 획득할 수 있어.
16: 내가 폭탄 아이템을 하나 줄게.     ->   getbomb() 호출
17: Tab 키를 눌러서 폭탄 아이템을 선택해봐.   -> tab키 눌러서 폭탄 선택되면 넘어감
18: 아이템을 선택한 상태로 마우스 오른쪽 버튼을 클릭해봐!    ->  마우스 우클릭 ui 보여주기  / 마우스 오른쪽 버튼 누르면 넘어감
19: 봤지? 폭탄 아이템은 주변 일정 범위 내의 얼음 수정들을 없애줘.
20: 이 외에도 회복, 스피드업, 보호 아이템이 있어!
21: 얼음을 부시다 보면 아이템 외에도 여러 효과들이 있을거야.
22: 최대한 많이 얼음 수정을 부시고, 스파크를 처치해서 이 행성을 구해줘!
23: 또 필요한 일이 있으면 찾아올게!
*/

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Image = UnityEngine.UI.Image;
using Unity.VisualScripting;

public class NewTutorialManager : MonoBehaviour
{

    public int idx = -2;
    [TextArea]
    public string[] texts = new string[25];
    public TMP_Text mainText;
    public GameObject clickToNextText;
    public Image blackOverlay;
    public TMP_Text startText;

    [Space(10)]
    public GameManager mainGameManager;
    public FirstPersonController characterController;
    public SparkeySpawner sparkeySpawner;
    public Transform characterPosition;

    [Space(10)]
    public int spriteIndex = 0;
    public Image uiImage;
    public List<Sprite> uiSprites = new List<Sprite>(5);

    [Space(5)]
    public GameObject SparkeyObject;
    private GameObject newSpark;

    private void Awake()
    {

    }

    private void Start()
    {
        // 메인 스파키 스폰 비활성화
        sparkeySpawner.enabled = false;
        FirstPersonController.onTutorial = true;

        idx = -2;
        mainText.text = texts[0];

        newSpark = SparkeyObject;
    }

    private bool isUiActivated = false;
    private bool isSparkSpawned = false;
    private bool isGetBomb = false;

    private void Update()
    {
        // esc 누르면 튜토리얼 종료
        if (Input.GetKey(KeyCode.Escape))
        {
            FirstPersonController.onTutorial = false;
            sparkeySpawner.enabled = true;

            Destroy(gameObject);
        }

        if (idx == -2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startText.text = "이 행성은 지금, 당신의 도움이 필요합니다.";
                idx++;
            }
        }
        else if (idx == -1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                blackOverlay.DOColor(new Color(0, 0, 0, 0), 1f);
                startText.DOColor(new Color(1, 1, 1, 0), 1f);
                idx++;
            }
        }


        else if (idx == 4 || idx == 5)
        {
            if (!isUiActivated)
            {
                ActivateUIimage();
                isUiActivated = true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                UnActivateUIimage();
                isUiActivated = false;
                ProgressNext();
            }
        }
        else if (idx == 7)
        {
            if (!isUiActivated)
            {
                ActivateUIimage();
                isUiActivated = true;
            }
            FirstPersonController.onTutorial = false;
            clickToNextText.SetActive(false);

            // 수정 부시면 넘어가기
            if (GameManager.crystalCount > 0)
            {
                UnActivateUIimage();
                isUiActivated = false;
                clickToNextText.SetActive(true);
                ProgressNext();
            }
        }
        else if (idx == 10)
        {
            // 스파크 하나 소환하기
            if (!isSparkSpawned)
            {
                isSparkSpawned = true;
                Instantiate(newSpark, SparkeyObject.transform.position, Quaternion.identity);
                newSpark.SetActive(true);
            }

            if (Input.GetMouseButtonDown(0))
                ProgressNext();
        }
        else if (idx == 11)
        {
            if (!isUiActivated)
            {
                ActivateUIimage();
                isUiActivated = true;
            }
            clickToNextText.SetActive(false);

            // 쉬프트 눌러서 스킬 사용하면 넘어가기
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                UnActivateUIimage();
                isUiActivated = false;
                clickToNextText.SetActive(true);

                // 스파크 안죽었으면 죽여버리기
                if (newSpark != null && !newSpark.IsDestroyed())
                    Destroy(newSpark);

                ProgressNext();
            }
        }
        else if (idx == 16)
        {
            // 폭탄 하나 주기
            if (!isGetBomb)
            {
                isGetBomb = true;
                mainGameManager.setGetBomb();
            }

            if (Input.GetMouseButtonDown(0))
                ProgressNext();
        }
        else if (idx == 17)
        {
            if (!isUiActivated)
            {
                ActivateUIimage();
                isUiActivated = true;
            }
            clickToNextText.SetActive(false);

            // 폭탄 선택되면 넘어가기
            if (mainGameManager.item_index == 2)
            {
                isUiActivated = false;
                ProgressNext();
            }
        }
        else if (idx == 18)
        {
            if (!isUiActivated)
            {
                ActivateUIimage();
                isUiActivated = true;
            }

            if (Input.GetMouseButtonDown(1))
            {
                UnActivateUIimage();
                isUiActivated = false;
                clickToNextText.SetActive(true);
                ProgressNext();
            }
        }
        else if (idx > 23)
        {
            FirstPersonController.onTutorial = false;
            sparkeySpawner.enabled = true;

            Destroy(gameObject);

        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("progress");
                ProgressNext();
            }
        }

    }

    private void OnDestroy()
    {
        GameManager.TutorialCompleted = true;
        FirstPersonController.onTutorial = false;
    }

    void ProgressNext()
    {
        idx++;
        mainText.text = texts[idx];
    }

    void ActivateUIimage()
    {
        uiImage.sprite = uiSprites[spriteIndex];
        spriteIndex++;
        uiImage.DOColor(new Color(1, 1, 1, 0.6f), 0.6f);
    }

    void UnActivateUIimage()
    {
        uiImage.color = new Color(1, 1, 1, 0);  // 이미지 다시 안보이게 투명
    }

}
