using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable CS0414

public class CameraTracker : MonoBehaviour
{
    public GameObject Player;
    public GameObject Planet;


    void LateUpdate()
    {
        // SMOOTH POSITION
        transform.position = Vector3.Lerp(transform.position, Player.transform.position, 50 * Time.deltaTime);

        // GRAVITY DIRECTION
        Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

        // SMOOTH ROTATION
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 100 * Time.deltaTime);
    }
}