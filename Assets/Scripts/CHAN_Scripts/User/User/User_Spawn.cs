using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Spawn : MonoBehaviour
{
    // 현재 진입해있는 섬의 ID정보를 저장하는 변수 
    public static string curIsland_ID;
    // 플레이어가 하늘 씬으로 이동할때 불려지는 함수
    public float distance;
    public void Spawn()
    {
        // 현재 섬의 위치 좌표를 구한다.
        // 원점과 현재 위치 좌표의 방향을 구한다. 
        // 현재 섬의 위치 기준 위의 구한 방향과 특정 거리를 곱한값으로 플레이어를 스폰
        Vector3 curIslandPosition = Island_Information.instance.Island_Dic[PlayerPrefs.GetString("User_Island_ID")].island_Pos;
        Vector3 dir = (Vector3.zero-curIslandPosition).normalized;
        transform.position = curIslandPosition + (dir * distance);
    }


}
