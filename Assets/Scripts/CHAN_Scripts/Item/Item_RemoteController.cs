using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Agora_RTC_Plugin.API_Example.Examples.Advanced.ScreenShare;
public class Item_RemoteController : MonoBehaviourPun
{
    //태어났을 때 TV매니저에 접근해서 화면 움직이도록 한다.
    readonly Item_TVManager_Agora mgr = Item_TVManager_Agora.instance;
    Dropdown dropDown;
    void Start()
    {
        mgr.Move(true);
        mgr.ReadyToShare();
        dropDown = GameObject.Find("Dropdown").GetComponent<Dropdown>();
        dropDown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropDown);
        });
    }
    // Update is called once per frame
    private void OnDestroy()
    {
        mgr.EndShare();
        mgr.Move(false);
    }
    void DropdownValueChanged(Dropdown change)
    {
        if (photonView.IsMine)
        {
            mgr.SendScreen(ScreenShare.myID.ToString());
            Debug.LogWarning("uid :" + ScreenShare.myID.ToString());
            Debug.LogWarning(change.value + "화면 전환");
        }
    }

}
