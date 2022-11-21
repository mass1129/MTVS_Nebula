using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Agora_RTC_Plugin.API_Example.Examples.Advanced.ScreenShare;
public class Item_RemoteController : MonoBehaviourPun
{
    //�¾�� �� TV�Ŵ����� �����ؼ� ȭ�� �����̵��� �Ѵ�.
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
            Debug.LogWarning(change.value + "ȭ�� ��ȯ");
        }
    }

}
