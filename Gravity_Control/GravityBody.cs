using UnityEngine;
using System.Collections;
#pragma warning disable CS0108 // 멤버가 상속된 멤버를 숨깁니다. new 키워드가 없습니다.

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{

    public GravityAttractor planet;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Disable rigidbody gravity and rotation as this is simulated in GravityAttractor script
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        // Allow this body to be influenced by planet's gravity
        planet.Attract(rb);
    }
}