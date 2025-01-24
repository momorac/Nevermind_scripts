using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Threading;
using TMPro;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class LoadingSceneController : MonoBehaviour
{

    public string nextScene;
    public float loadingTime;

    [Space(7)]
    public GameObject LoadingScene;
    public Image progressBar;
    public TMP_Text loadingMessage;
    private string[] messages = new string[]{
        "모든 생각이 진실일 필요는 없습니다",
        "캐릭터를 상처입혀서는 안 돼요",
        "올바른 길을 찾아주세요",
        "모든 것이 완벽할 필요는 없습니다",
        "과거보다는 현재에 집중해보세요",
        "캐릭터를 다치게 하는 것은 다 치워버리세요",
        "분명 이 행성은 더 나아질 것입니다",
        "행성의 빛을 다시 밝혀주세요",
        "누군가가 하는 말이 다 옳지는 않습니다"
    };



    public void ChangeScene()
    {
        LoadingScene.SetActive(true);
        loadingMessage.text = messages[Random.Range(0, messages.Length)];
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);
        operation.allowSceneActivation = false;
        float time = 0f;

        while (!operation.isDone)
        {
            time += Time.deltaTime;

            progressBar.fillAmount = time / loadingTime;
            if (time >= loadingTime)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;

        }
    }
}
