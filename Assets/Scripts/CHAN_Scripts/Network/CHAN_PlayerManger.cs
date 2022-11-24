using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHAN_PlayerManger : MonoBehaviourPun
{
    // 유저 고유의 오브젝트 정보를 LocalPlayerInstance에 저장한다. 
    public static GameObject LocalPlayerInstance;
    private void Awake()
    {
        // 캐릭터가 자기자신일 때 저장
        if (CHAN_GameManager.instance.prefab == CHAN_GameManager.instance.WhalePrepab)
        {
            // 포톤아이디가 내것이라면 
            if (photonView.IsMine)
            {
                //오브젝트 자식에 있는 플레이어 아이콘을 활성화하고 다른 유저 아이콘은 비활성화 한다.
                CHAN_PlayerManger.LocalPlayerInstance = this.gameObject;
                transform.GetChild(1).GetComponent<User_IconMove>().OnPlayerIcon();
            }
            else
            {
                //플레이어 아이콘 비활성화하고 유저 아이콘을 활성화한다.
                gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                transform.GetChild(1).GetComponent<User_IconMove>().OnUserIcon();
            }
        }
        //JoinRoom 전에 플레이어가 생성되므로 로드시, 오브젝트가 없어지면 안된다. 
        DontDestroyOnLoad(gameObject);
    }  
}
