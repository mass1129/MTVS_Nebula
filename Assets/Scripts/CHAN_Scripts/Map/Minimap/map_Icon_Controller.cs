using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class map_Icon_Controller : MonoBehaviour
{
    [SerializeField]
    List<Color> Icon_Colors;
    [SerializeField]
    List<string> categories;
    public GameObject IslandText;
    public GameObject Image_Online;
    public GameObject Image_Offine;
    //섬 아이콘 색정보를 받아온다. 
    //섬이 생성될 때 1차 카테고리를 비교한다.

    // 아이콘이 바라볼 카메라 위치
    void Start()
    {
        IslandText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 여기서는 아이콘이 바라보는 방향을 업데이트 해주는 함수가 들어간다. 
        // 미니맵 상태일 때 아이콘을 위로 바라보도록
        if (Map_UIManager.instance.state_View == "minimap")
        {
            transform.forward = Vector3.down;
            
        }
        // 하늘맵 상태일 때 아이콘을 sky카메라로 바라보도록
        else if (Map_UIManager.instance.state_View == "skymap")
        {
            transform.LookAt(Map_UIManager.instance.skyCam.transform);
        }
    }

    /// <summary>
    /// 유저 섬의 색 식별을 위해서 지정된 색정보를 할당시켜주는 함수
    /// </summary>
    /// <param name="category">서버에서 가져온 1차 카테고리 내용이다. 수정은 인스펙터에서 수정 가능</param>
    public void SetIconColor(string category)
    {
        //profileIsland에서 1차카테고리 정보를 가져오면 1차카테고리내용과 지정된 카테고리 리스트와 비교한다.
        for (int i = 0; i < categories.Count; i++)
        {
            if (category == categories[i])
            {
                gameObject.GetComponentInChildren<Image>().color = Icon_Colors[i];
            }
        }

    }

    /// <summary>
    /// 마우스를 해당 아이콘에 갖다댔을 때, 유저의 이름이 나옴.
    /// </summary>
    public void VisualText(bool b)
    {
        IslandText.SetActive(b);
        IslandText.GetComponent<Text>().text = GetComponentInParent<Island_Profile>().user_name + " 의 섬";
    }

    public void SetOnlineIcon(bool b)
    {
        Image_Online.SetActive(b);
        Image_Offine.SetActive(!b);
    }

}
