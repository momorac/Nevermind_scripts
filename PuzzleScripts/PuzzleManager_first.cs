using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PuzzleManager_first : MonoBehaviour
{
    [Space(20)]
    public bool plugAttached = false;
    private bool pre_plugState = false;

    public bool lineAttached = false;
    private bool pre_lineState = false;

    [Space(20)]
    public bool endTrigger = false;

    [Space(20)]
    public GameObject fuzeLight_1;
    public GameObject fuzeLight_2;
    public Material fuzeOn_Material;
    private Material fuzeOff_Material;
    public AudioSource fuzeOn_Sound;
    public AudioSource fuzeOff_Sound;

    public GameObject fuzeNoise;
    private GameObject clearObject;


    void Awake()
    {
        clearObject = transform.GetChild(0).gameObject;
    }

    void Start()
    {
        fuzeOff_Material = fuzeLight_1.GetComponent<MeshRenderer>().material;
    }


    void Update()
    {

        // 클리어 조건 검사
        if (plugAttached && lineAttached)
        {
            // 엔딩 컨트롤러 컴포넌트 활성화
            clearObject.SetActive(true);
        }

        // lineAttached 상태 변화 확인 및 처리
        if (lineAttached != pre_lineState) // 상태가 변할 때
        {
            if (lineAttached) // 연결되었을 때
            {
                fuzeOn_Sound.Play();
                fuzeLight_1.GetComponent<MeshRenderer>().material = fuzeOn_Material;
                fuzeLight_2.GetComponent<MeshRenderer>().material = fuzeOn_Material;
                fuzeNoise.SetActive(false);
            }
            else // 연결이 해제되었을 때
            {
                fuzeOff_Sound.Play();
                fuzeLight_1.GetComponent<MeshRenderer>().material = fuzeOff_Material;
                fuzeLight_2.GetComponent<MeshRenderer>().material = fuzeOff_Material;
                fuzeNoise.SetActive(true);
            }
        }
    }

    void LateUpdate()
    {
        pre_lineState = lineAttached;
        pre_plugState = plugAttached;
    }

}
