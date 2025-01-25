using UnityEngine;
using System.Collections;
#pragma warning disable CS0108 // 멤버가 상속된 멤버를 숨깁니다. new 키워드가 없습니다.

[RequireComponent(typeof(GravityBody))]
public class FirstPersonController : MonoBehaviour
{
    //public classes
    public Animator characterAnimator;

    [Space(10)]
    // public vars
    public static bool onTutorial;
    public float mouseSensitivityX = 1;
    public float mouseSensitivityY = 1;
    public float walkSpeed = 6;
    public float jumpForce = 220;
    public float jumpDelay = 0;
    public bool isSpinning = false;

    [Space(10)]
    [Header("effect & audio")]
    public GameObject particleEffect;
    public AudioSource footstepSound;
    public AudioSource sparkEndSound;
    public AudioSource spinSound;

    // System vars
    private bool isGrounded;
    private Vector3 moveAmount;
    private Vector3 smoothMoveVelocity;
    private float verticalLookRotation;
    private Rigidbody rigidbody;
    private Transform cameraTransform;


    void Start()
    {
        footstepSound.playOnAwake = true;
        footstepSound.Pause();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraTransform = Camera.main.transform;
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Calculate movement
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(inputX, 0, inputY).normalized;

        // animation control
        if (moveDir != Vector3.zero)
        {
            characterAnimator.SetBool("IsMoving", true);
            footstepSound.UnPause();
        }
        else
        {
            characterAnimator.SetBool("IsMoving", false);
            footstepSound.Pause();
        }

        // Apply movement to rigidbody
        if (!isSpinning)
        {
            moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * walkSpeed, ref smoothMoveVelocity, .15f);
            Vector3 localMove = transform.TransformDirection(moveAmount) * Time.deltaTime;
            rigidbody.MovePosition(rigidbody.position + localMove);
        }

    }


    private void Update()
    {

        //Look rotation
        float horizontalMouseInput = Input.GetAxis("Mouse X") * mouseSensitivityX;
        transform.Rotate(Vector3.up * horizontalMouseInput);
        if (horizontalMouseInput != 0)
        {
            characterAnimator.SetFloat("TurnAmount", horizontalMouseInput < 0 ? -1 * horizontalMouseInput : horizontalMouseInput);
        }

        verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
        cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                characterAnimator.SetTrigger("JumpTrigger");
                Invoke("Jump", jumpDelay);
            }
        }

        //attack - ontutorial 일때는 어택 액션 비활성화
        if (!onTutorial && Input.GetMouseButtonDown(0))
        {
            if (!characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                characterAnimator.SetTrigger("AttackTrigger");
        }

        // Spin
        if (Input.GetKeyDown(KeyCode.LeftShift) && GameManager.Instance.skillCharged >= 100)
        {
            GameManager.Instance.skillCharged = 0;
            isSpinning = true;
            spinSound.Play();
            characterAnimator.SetTrigger("SpinTrigger");

            // 스킬 이펙트 파티클 오브젝트 활성화 후 비활성화 딜레이
            particleEffect.SetActive(true);
            Invoke("DisableParticle", 3f);
        }

    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            isGrounded = false;
        }
    }


    private void Jump()
    {
        if (isGrounded == true)

        {
            rigidbody.AddForce(transform.up * jumpForce);
        }
    }

    private void DisableParticle()
    {
        isSpinning = false;
        particleEffect.SetActive(false);
    }


    private void OnTriggerStay(Collider other)
    {
        if (!onTutorial && other.gameObject.tag == "Crystal")
        {
            if (Input.GetMouseButtonDown(0))
            {
                other.gameObject.GetComponent<CrystalDestroyer>().BreakTrigger();
            }
        }
    }



}