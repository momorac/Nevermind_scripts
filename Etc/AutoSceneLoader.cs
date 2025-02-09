using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class AutoSceneLoader : MonoBehaviour
{
    public float idleTime = 10f;  // 입력이 없는 상태에서 대기할 시간 (초 단위)
    public Image blackOverlay;

    private float timer = 0f;  // 타이머 변수
    private bool hasChanged = false;

    void Start()
    {
        blackOverlay.color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        // 입력이 있으면 타이머를 0으로 초기화
        if (Input.anyKeyDown || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            timer = 0f;  // 타이머 초기화
        }
        else
        {
            // 입력이 없으면 타이머 증가
            timer += Time.deltaTime;
        }

        if (!hasChanged && timer >= idleTime)
        {
            hasChanged = true;
            blackOverlay.DOColor(Color.black, 2f);
            DOVirtual.DelayedCall(2.2f, () =>
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
    }
}
