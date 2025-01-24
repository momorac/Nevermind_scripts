using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class ClickMovement : MonoBehaviour
{
    public bool isEnded;
    public NavMeshAgent agent;
    Animator animator;

    public GameObject plug;
    public GameObject clickPoint;
    public AudioSource pickUpSound;
    public bool isCarryingPlug = false;    // 캐릭터가 plug를 잡고 있는지 여부
    public Transform carryPosition;         // plug가 종속될 위치 (캐릭터 앞)
    public AudioSource beepSound;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isEnded = false;
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                NavMeshHit navHit;
                NavMesh.SamplePosition(hit.point, out navHit, 100, NavMesh.AllAreas);

                // NavMeshPath로 경로를 계산하여 확인
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(navHit.position, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    // 경로가 유효하면 clickPoint 생성
                    clickPoint.SetActive(false);
                    clickPoint.transform.position = navHit.position;
                    clickPoint.SetActive(true);

                    agent.SetDestination(navHit.position);
                }
                else
                {
                    Debug.Log("Cannot reach the clicked point - obstacle in the way.");
                }
            }


            // 에이전트의 남은 거리를 체크하여 도착 여부를 확인
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                // 남은 거리가 거의 없다면 속도를 0으로 설정
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    animator.SetBool("IsMoving", false);
                    agent.isStopped = true;  // 이동 중지
                    agent.updateRotation = false; // 회전 중지
                }
            }
            else
            {
                animator.SetBool("IsMoving", true);
                agent.isStopped = false; // 이동 재개
                agent.updateRotation = true; // 회전 허용
            }


            HandleCarryingPlug();
        }
    }


    private void HandleCarryingPlug()
    {
        if (isEnded) return;  // 엔딩 상태면 플러그 관련 동작 중지

        if (isCarryingPlug)
        {
            // 캐릭터가 Plug를 운반 중일 때, Plug를 캐릭터와 함께 이동
            plug.transform.position = carryPosition.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Plug와의 충돌을 확인하고 클릭 시 Plug를 들 수 있도록 함
        if (!isEnded && other.gameObject == plug)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PickUpPlug();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            animator.SetTrigger("HitTrigger");
        }
    }

    private void PickUpPlug()
    {
        if (isEnded) return;

        if (!isCarryingPlug) pickUpSound.Play();
        isCarryingPlug = true;
        //animator.SetBool("IsGrab", true);

        plug.transform.SetParent(carryPosition);
        plug.transform.localPosition = Vector3.zero;
        plug.transform.localRotation = Quaternion.identity;
    }

}
