using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_UIManager : MonoBehaviour
{
    public static Map_UIManager instance;
    private void Awake()
    {
        instance = this;
    }
    #region 버튼 정의 영역
    Button btn_Toggle_CloseTab;
    Button btn_ShowSkyMap;
    #endregion
    //스카이맵 UI 오브젝트
    public GameObject UI_Skymap;
    //미니맵 UI 오브젝트
    public GameObject UI_Miinimap;
    // 하늘뷰 캠
    public GameObject skyCam;
    //미니맵뷰 캠
    public GameObject minimapCam;
    //여기서 개별 버튼을 누르면 기능이 발동되도록 만들 것임 
    public string state_View= "minimap";
    
    void Start()
    {
        #region 각 버튼의 위치를 찾는다. 
        btn_Toggle_CloseTab = GameObject.Find("Btn_CloseTab").GetComponent<Button>();
        btn_ShowSkyMap = GameObject.Find("Btn_ShowSkyMap").GetComponent<Button>();
        #endregion
        #region 각 버튼에 기능을 삽입한다.
         btn_Toggle_CloseTab.onClick.AddListener(OnMinimapView);
        btn_ShowSkyMap.onClick.AddListener(OnSkyView);
        #endregion
        // 초기에는 미니맵 뷰로 바로 전환한다.
        OnMinimapView();
    }
    /// <summary>
    /// 스카이맵 전환 버튼
    /// 여기서 state_View 상태를 'skymap'로 바꾼다.
    /// </summary>
    public void OnSkyView()
    {
        UI_Skymap.SetActive(true);
        UI_Miinimap.SetActive(false);
        state_View = "skymap";
        skyCam.SetActive(true);
        minimapCam.SetActive(false);
        Debug.Log("스카이 뷰로 전환");
    }
    /// <summary>
    /// 미니맵 전환 버튼
    /// 여기서 state_View 상태를 'minimap'로 바꾼다.
    /// </summary>
    public void OnMinimapView()
    {
        UI_Skymap.SetActive(false);
        UI_Miinimap.SetActive(true);
        state_View = "minimap";
        skyCam.SetActive(false);
        minimapCam.SetActive(true);
        Debug.Log("미니맵 뷰로 전환");
    }



}
