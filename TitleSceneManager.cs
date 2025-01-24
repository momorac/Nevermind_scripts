using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{

    LoadingSceneController loadingSceneController;
    private bool isChanging;
    public GameObject UIbar;
    private void Awake()
    {
        loadingSceneController = GetComponent<LoadingSceneController>();
        isChanging = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UIbar.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UIbar.SetActive(true);
        }
        else if (Input.anyKeyDown && !isChanging)
        {
            loadingSceneController.ChangeScene();
            isChanging = true;
        }
    }
}
