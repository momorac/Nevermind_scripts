using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class CrystalDestroyer : MonoBehaviour
{
    public float destroyTime = 3f;
    public int sparkBlowPercent;
    public GameManager gameManager;
    public GameObject Spark;


    public GameObject main;
    public GameObject fracture;
    private bool isBlowed = false;


    private void Start()
    {
        gameManager.crystalRemain++;
    }

    public void BreakTrigger()
    {
        Invoke("BreakSelf", 0.2f);
    }

    private void BreakSelf()
    {
        if (isBlowed) return;

        isBlowed = true;

        main.SetActive(false);
        fracture.SetActive(true);

        int s = Random.Range(0, 100);
        if (s < sparkBlowPercent)
        {
            SpawnRandomSparks(Random.Range(1, 5));
        }
        else
        {
            gameManager.CrystalBreakEffect();
        }
        gameManager.skillCharged += 10;

        Invoke("TimerDestroy", destroyTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Bomb") && !isBlowed)
        {
            isBlowed = true;
            main.SetActive(false);
            fracture.SetActive(true);

            gameManager.skillCharged += 10;
            Invoke("TimerDestroy", destroyTime);
        }
    }

    private void TimerDestroy()
    {
        GameManager.crystalCount++;
        Destroy(gameObject);
    }

    private void SpawnRandomSparks(int count)
    {

        for (int i = 0; i < count; i++)
        {
            Instantiate(Spark, gameObject.transform.position, Quaternion.identity).SetActive(true);
        }
    }


}
