using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 미니맵상 플레이어, 하늘섬아이콘을 미니맵상으로 표시되도록 구현한다.
/// </summary>
public class Minimap_IconManager : MonoBehaviour
{
    public GameObject playerIcon;
    public Image IslandIcon;

    Transform player;
    void Start()
    {
        //시작할때 하늘섬의 오브젝트, 플레이어의 오브젝트를 들고온다. 
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    //[System.Obsolete]
    //void Update()
    //{
    //    playerIcon.transform.position = player.position;
    //    playerIcon.transform.localRotation = Quaternion.EulerAngles(0,User_Rotate.instance.ry,0);
    //}
}
