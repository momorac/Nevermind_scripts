using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EachSpikeMovement : MonoBehaviour
{
    public bool isOpen;

    private void Start()
    {
        isOpen = false;
    }

    public void OpenSpike()
    {
        isOpen = true;
        transform.DOLocalMoveY(-3, 1.5f);
    }
    public void CloseSpike()
    {
        isOpen = false;
        transform.DOLocalMoveY(0, 1.5f);
    }


}
