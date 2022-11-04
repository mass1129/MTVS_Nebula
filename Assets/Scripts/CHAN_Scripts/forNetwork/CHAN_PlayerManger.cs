using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHAN_PlayerManger : MonoBehaviourPun
{
    public static GameObject LocalPlayerInstance;
    private void Awake()
    {
        // 캐릭터가 자기자신일 때 저장
        if (photonView.IsMine)
        { 
            CHAN_PlayerManger.LocalPlayerInstance = this.gameObject;
        }
        else
        {
            gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        }
        DontDestroyOnLoad(gameObject);
        CHAN_ClientManager.instance.AddPlayer(photonView);
    }
    private void OnDestroy()
    {
        CHAN_ClientManager.instance.ExitPlayer(photonView);
    }
}
