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
        if (CHAN_GameManager.instance.prefab == CHAN_GameManager.instance.WhalePrepab)
        {
            if (photonView.IsMine)
            {
                CHAN_PlayerManger.LocalPlayerInstance = this.gameObject;
                transform.GetChild(1).GetComponent<IconMove>().OnPlayerIcon();
            }
            else
            {
                gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                transform.GetChild(1).GetComponent<IconMove>().OnUserIcon();
            }
        }
        
        DontDestroyOnLoad(gameObject);
        CHAN_ClientManager.instance.AddPlayer(this.photonView);
    }
    private void OnDestroy()
    {
        if (photonView.IsMine)
        {
            CHAN_ClientManager.instance.RemovePlayer(this.photonView);
        }
    }
}
