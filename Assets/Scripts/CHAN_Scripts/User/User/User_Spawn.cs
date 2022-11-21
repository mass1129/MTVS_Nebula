using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Spawn : MonoBehaviour
{
    // 현재 진입해있는 섬의 ID정보를 저장하는 변수 
    public static string curIsland_ID;
    // 플레이어가 하늘 씬으로 이동할때 불려지는 함수
    public void Spawn()
    {
        transform.position = Island_Information.instance.Island_Dic[PlayerPrefs.GetString("User_Island_ID")].island_Pos;
    }


}
