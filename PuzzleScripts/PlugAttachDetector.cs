using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugAttachDetector : MonoBehaviour
{

    public GameObject Destination;
    public PuzzleManager_first puzzleManager_First;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(Destination))
        {
            puzzleManager_First.plugAttached = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(Destination))
        {
            puzzleManager_First.plugAttached = false;
        }
    }


}
