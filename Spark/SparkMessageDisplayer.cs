using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SparkMessageDisplayer : MonoBehaviour
{
    public String[] messages = new String[10];
    public TMP_Text text;
    void Start()
    {
        text.text = messages[Random.Range(0, messages.Length)];
    }
}

