using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserOnOff : MonoBehaviour
{

    public GameObject laser;

    void Start()
    {
        StartCoroutine(OnandOff());
    }


    private IEnumerator OnandOff()
    {
        while (true)
        {
            laser.SetActive(true);
            yield return new WaitForSeconds(3f);
            laser.SetActive(false);
            yield return new WaitForSeconds(3f);
        }
    }
}
