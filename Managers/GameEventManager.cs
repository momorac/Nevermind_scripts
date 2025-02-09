using System;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    #region singleton setting
    private static GameEventManager instance;
    public static GameEventManager Instance
    {
        get
        {
            if (instance == null)
            {
                // 씬에서 GameEventManager를 찾음
                instance = FindObjectOfType<GameEventManager>();
                if (instance == null)
                {
                    // 존재하지 않으면 새로 생성
                    GameObject obj = new GameObject("GameEventManager");
                    instance = obj.AddComponent<GameEventManager>();
                    DontDestroyOnLoad(obj); // 씬 전환 시에도 유지
                }
            }
            return instance;
        }
    }

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
    }
    #endregion

    // 플레이어 상태 이벤트
    public event Action<bool> OnPlayerMove;
    public event Action<float> OnPlayerHPChanged;
    public event Action OnPlayerSpin;
    public event Action OnPlayerPowerMode;

    public void PlayerMove(bool isMoving) => OnPlayerMove(isMoving);
    public void PlayerHPChanged(float newHP) => OnPlayerHPChanged?.Invoke(newHP);
    public void PlayerSpin() => OnPlayerSpin?.Invoke();
    public void PlayerPowerMode() => OnPlayerPowerMode?.Invoke();

    // 스킬 사용 관련 이벤트
    public event Action<float> OnSkillGageChanged;
    public event Action OnSparkeyDie;

    public void SkillGageChanged(float newGage) => OnSkillGageChanged?.Invoke(newGage);
    public void SparkeyDie() => OnSparkeyDie?.Invoke();

    // 퍼즐 씬 전환 관련 이벤트
    public event Action OnPuzzleOpened;
    public event Action<bool> OnPlayerClosedToLight;

    public void PuzzleOpened() => OnPuzzleOpened?.Invoke();
    public void PlayerClosedToLight(bool isClosed) => OnPlayerClosedToLight?.Invoke(isClosed);


    // 아이템 획득/사용 이벤트
    public event Action<string> OnItemGet;
    public event Action<string> OnItemUse;
    public event Action<int> OnItemChanged;

    public void ItemGet(string itemName) => OnItemGet?.Invoke(itemName);
    public void ItemUse(string itemName) => OnItemUse?.Invoke(itemName);
    public void ItemChanged(int newIndex) => OnItemChanged?.Invoke(newIndex);

}

