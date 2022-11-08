using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    //해당 클래스는 로딩을 위한 클래스임 
    //오브젝트 형식으로 불러들이지말고 스크립트 상에서 바로 불러올 수 있도록 만들어 보자
    // 속성
    // 로딩 플레그, 이미지
    public bool isLoadingDone;
    public GameObject LoadingIcon;
    public Image LoadingImage;
    void Start()
    {
        
    }

    // Update is called once per frame

    IEnumerator ShowLoading()
    {
        //해당 코루틴이 시작하자마자 전체 스크린을 fadeIn fadeOut 한다.
        //그리고 로딩 이미지를 보여준다. 
        //로딩 조건이 끝날 때 까지 계속 반복해야 함
        while (!isLoadingDone)
        {
            //여기서는 로딩바가 돌아가는 모션을 넣자 
            LoadingIcon.SetActive(true);
            yield return null;

        }
        //로딩이 끝나면 반복을 멈춘다. 
        
    }
}
