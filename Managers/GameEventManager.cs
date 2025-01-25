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
    public event Action<float> OnPlayerHPChanged;
    public event Action OnPlayerPowermode;

    public void PlayerHPChanged(float newHP) => OnPlayerHPChanged?.Invoke(newHP);
    public void PlayerPowermode() => OnPlayerPowermode?.Invoke();


    // 아이템 획득/사용 이벤트
    public event Action<string> OnItemGet;
    public event Action<string> OnItemUsed;

    public void ItemGet(string itemName) => OnItemGet?.Invoke(itemName);
    public void ItemUsed(string itemName) => OnItemUsed?.Invoke(itemName);
}

