using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SparkeySpawner : MonoBehaviour
{
    public float spawnDelay_min = 5f;
    public float spawnDelay_max = 20f;
    [SerializeField]
    private Vector3[] randomSpawnLocations = new Vector3[7];
    public GameObject Spark;
    public SparkeyAppearLabel uiLabel;

    private GameManager gameManager;
    private bool isSpawnStopped = false;

    void Awake()
    {
        gameManager = gameObject.GetComponent<GameManager>();
    }

    void Start()
    {
        StartCoroutine(SpawnSpark());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (isSpawnStopped)
                isSpawnStopped = false;
            else
                isSpawnStopped = true;
        }
    }

    private IEnumerator SpawnSpark()
    {
        while (true)
        {
            float waitTime = Random.Range(spawnDelay_min, spawnDelay_max);
            yield return new WaitForSeconds(waitTime);

            if (!isSpawnStopped)
                Spawn();
        }
    }
    void Spawn()
    {
        GameObject newSpark = Instantiate(Spark, randomSpawnLocations[Random.Range(0, randomSpawnLocations.Length)], Quaternion.identity);
        newSpark.SetActive(true);
        uiLabel.AppearLabel();
        GameManager.Instance.SparkCount++;
        //Debug.Log("Spark Spawned!!");
    }

}
