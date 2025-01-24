using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBlow : MonoBehaviour
{
    public GameObject pre;
    public GameObject post;
    public int delay;

    void Start()
    {
        Invoke("Blow", delay);
    }

    private void Blow()
    {
        Destroy(pre);
        post.SetActive(true);
        Invoke("DestroySelf", 2f);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
