using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class for_DataTest : MonoBehaviourPun
{

    public Dropdown dropdown;
    private void Awake()
    {
        dropdown.onValueChanged.AddListener(OnValueChanged);
    }
    public void OnValueChanged(int index)
    {
        if (index == 0) SetData("test_FriendList.csv");
        if (index == 1) SetData("subset_30_v3_fin.csv");
        //if (index == 2) SetData("subset500_pc4_v2.csv");



    }
    public void SetData(string data)
    {
        photonView.RPC("RPCSetData", RpcTarget.AllBufferedViaServer, data);

    }
    [PunRPC]
    void RPCSetData(string data)
    {
        Island_Information.instance.Done = false;
        Island_Information.instance.DeleteData();
        Island_Information.instance.Load();
        print("ภüผÛตส");
    }
}
