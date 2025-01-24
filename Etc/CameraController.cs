using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 2.0f;         // 카메라 이동 속도
    public float lookSpeed = 2.0f;          // 마우스를 통한 회전 속도
    public float verticalMoveSpeed = 2.0f;  // Y축 이동 속도
    public float shiftRotationSpeed = 100.0f; // Shift 키를 통한 Z축 회전 속도

    private float yaw = 0.0f;   // 카메라의 수평 회전 값 (Y축 기준)
    private float pitch = 0.0f; // 카메라의 수직 회전 값 (X축 기준)
    private float roll = 0.0f;  // 카메라의 Z축 회전 값

    void Start()
    {
        // 초기 회전 값을 현재 카메라의 회전 값으로 설정
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void Update()
    {
        // 마우스 움직임에 따라 카메라 회전 (Z축 회전을 제외하고)
        yaw += lookSpeed * Input.GetAxis("Mouse X");
        pitch -= lookSpeed * Input.GetAxis("Mouse Y");

        // pitch 값 제한 (카메라가 수직으로 뒤집히지 않도록 제한)
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        // Tab 키로 상승
        if (Input.GetKey(KeyCode.Tab))
        {
            transform.position += Vector3.up * verticalMoveSpeed * Time.deltaTime;
        }

        // Caps Lock 키로 하강
        if (Input.GetKey(KeyCode.CapsLock))
        {
            transform.position += Vector3.down * verticalMoveSpeed * Time.deltaTime;
        }

        // 좌우 Shift 키로 Z축 회전
        if (Input.GetKey(KeyCode.LeftShift))
        {
            roll -= shiftRotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightShift))
        {
            roll += shiftRotationSpeed * Time.deltaTime;
        }

        // 변경된 회전 값을 적용 (Z축 회전 포함)
        transform.eulerAngles = new Vector3(pitch, yaw, roll);

        // 키보드 입력에 따라 카메라 이동
        float horizontal = Input.GetAxis("Horizontal"); // A, D 키
        float vertical = Input.GetAxis("Vertical");     // W, S 키

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);
        moveDirection = transform.TransformDirection(moveDirection); // 로컬 좌표계를 기준으로 방향 변환
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
