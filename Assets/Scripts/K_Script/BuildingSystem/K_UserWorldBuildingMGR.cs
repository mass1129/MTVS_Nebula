using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class K_UserWorldBuildingMGR : MonoBehaviourPun
{
    public GridBuildingSystem3D gridBuildingSystem;
    public string islandId="11";
    private void Awake()
    {
            //PlayerPrefs.GetString("CurrentIslandId");
        
    }
    private void Start()
    {   
       
        //PhotonNetwork.Destroy(gameObject);
    }
}
