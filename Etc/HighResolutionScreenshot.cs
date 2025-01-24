using UnityEngine;
using System.IO;

public class HighResolutionScreenshot : MonoBehaviour
{
    public Camera cameraToCapture; // 캡처할 카메라
    public int resolutionMultiplier = 1; // 해상도 배율 (기본 해상도의 몇 배로 캡처할지)
    int count = 0;

    void Start()
    {
        // 캡처할 카메라가 지정되지 않았다면, 현재 스크립트가 붙어 있는 오브젝트의 카메라 사용
        if (cameraToCapture == null)
        {
            cameraToCapture = GetComponent<Camera>();
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            CaptureScreenshot();
        }
    }

    public void CaptureScreenshot()
    {
        // 현재 카메라의 해상도를 가져오기
        int width = cameraToCapture.pixelWidth * resolutionMultiplier;
        int height = cameraToCapture.pixelHeight * resolutionMultiplier;

        // RenderTexture 생성
        RenderTexture rt = new RenderTexture(width, height, 24);
        cameraToCapture.targetTexture = rt;

        // 카메라 렌더링
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
        cameraToCapture.Render();

        // RenderTexture에서 이미지 데이터 읽어오기
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();

        // RenderTexture와 카메라 초기화
        cameraToCapture.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // PNG 파일로 저장
        byte[] bytes = screenShot.EncodeToPNG();
        string filePath = Path.Combine(Application.dataPath, count + ".png");
        File.WriteAllBytes(filePath, bytes);

        Debug.Log($"Screenshot saved to {filePath}");
        count++;
    }
}
