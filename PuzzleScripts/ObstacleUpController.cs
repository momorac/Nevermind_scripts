using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObstacleUpController : MonoBehaviour
{

    public GameObject character;
    public int repeatTime;
    public ClickMovement clickMovement;

    void Start()
    {
        InvokeRepeating("UpObstacle", 1, repeatTime);
    }

    void FixedUpdate()
    {
        if (clickMovement.isEnded)
        {
            CancelInvoke();
        }
    }


    void UpObstacle()
    {
        Vector3 characterPosition = character.transform.position;
        transform.position = new Vector3(characterPosition.x, -4f, characterPosition.z);

        DOVirtual.DelayedCall(0.5f, () =>
        {
            transform.DOMoveY(0.2f, 0.5f);
        });

        DOVirtual.DelayedCall(1.5f, () =>
       {
           transform.DOMoveY(-4, 1.5f);
       });
    }

}


