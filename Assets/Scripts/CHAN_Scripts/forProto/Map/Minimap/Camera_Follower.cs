using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미니맵 전용 카메라가 플레이어를 따라가도록 구현
/// </summary>
public class Camera_Follower : MonoBehaviour
{
    public float yPos;
    private void Update()
    {
        //플레이어가 없으면 그냥 리턴 시킨다.
        if (!CHAN_PlayerManger.LocalPlayerInstance) return;
        
            Vector3 Pos = new Vector3(CHAN_PlayerManger.LocalPlayerInstance.transform.position.x, yPos, CHAN_PlayerManger.LocalPlayerInstance.transform.position.z);
            transform.position = Pos;
        //float RotY =CHAN_PlayerManger.LocalPlayerInstance.transform.rotation.y;
        //transform.rotation =Quaternion.Euler(new Vector3(90, RotY, 0));


    }

}
