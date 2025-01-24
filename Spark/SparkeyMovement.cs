using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SparkeyMovement : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject player;

    public Camera mainCamera;
    public Renderer sparkRenderer;

    private FirstPersonController firstPersonController;

    public Transform playerPosition;
    public AudioSource effectSound;

    public float moveSpeed = 1f;
    public float maxSoundDistance = 3f;  // 소리가 최대 크기로 들리는 거리

    [Space(10)]
    private bool messageShown = false;
    public GameObject messageUIPrefab;
    private GameObject messageUIInstance;



    void Start()
    {
        firstPersonController = player.GetComponent<FirstPersonController>();
    }


    void FixedUpdate()
    {

        transform.position = Vector3.MoveTowards(transform.position, playerPosition.position, moveSpeed*Time.deltaTime);
        // 플레이어와 특정 오브젝트 간의 거리 계산
        float distance = Vector3.Distance(transform.position, playerPosition.position);

        // 거리에 따라 음량 조절
        effectSound.volume = Mathf.Clamp01(0.5f - (distance / maxSoundDistance));



        // 카메라 위치 계산
        if (sparkRenderer.isVisible)
        {
            // 오브젝트의 월드 좌표를 가져옵니다.
            Vector3 objectWorldPosition = transform.position;
            // 월드 좌표를 화면 좌표로 변환합니다.
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(objectWorldPosition);
            // 화면 좌표를 2D 벡터로 변환합니다.
            Vector2 screenPosition2D = new Vector2(screenPosition.x, screenPosition.y + 40);

            if (!messageShown && distance < 1)
            {
                messageShown = true;
                messageUIInstance = Instantiate(messageUIPrefab, transform.position, Quaternion.identity);
                messageUIInstance.transform.SetParent(GameObject.Find("Canvas").transform, false); // Canvas 안에 생성합니다.
            }

            // messageUIInstance가 존재하면 위치를 업데이트합니다.
            if (messageUIInstance != null)
            {
                messageUIInstance.transform.position = screenPosition2D;
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //캐릭터가 파워모드이면 
            if (gameManager.isCharacterPowerMode)
            {
                if (messageUIInstance != null)
                    Destroy(messageUIInstance);
                Destroy(gameObject);
                gameManager.UnAttacked();
                firstPersonController.sparkEndSound.Play();
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameManager.Attacked();

            if (firstPersonController.isSpinning)
            {
                if (messageUIInstance != null)
                    Destroy(messageUIInstance);
                Destroy(gameObject);
                gameManager.UnAttacked();
                firstPersonController.sparkEndSound.Play();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameManager.UnAttacked();
        }
    }
}
