using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class K_UserWorldBuildingMGR : MonoBehaviourPun
{
    public GridBuildingSystem3D gridBuildingSystem;
    
    private void Awake()
    {
          
        
    }
    private void Start()
    {   
       
        
    }
    private void OnEnable()
    {
        string s = PlayerPrefs.GetString("User_Island_ID");
        gridBuildingSystem.FirstBuildingLoad(s);
        PhotonNetwork.Destroy(gameObject);
    }
}
