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
    //섬 아이콘 색정보를 받아온다. 
    //섬이 생성될 때 1차 카테고리를 비교한다.
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 여기서는 아이콘이 바라보는 방향을 업데이트 해주는 함수가 들어간다. 
        //이이콘이 바라보는 방향은 항상 
    }
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
}
