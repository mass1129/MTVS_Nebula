using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class K_UserWorldBuildingMGR : MonoBehaviourPun
{
    public GridBuildingSystem3D gridBuildingSystem;
    bool isDone = false;
    private void Awake()
    {
          
        
    }
    private void Start()
    {   
       
        
    }
    private void OnEnable()
    {
       
        if(!isDone)
        {
            gridBuildingSystem.TestLoad();
            isDone = true;
        }
       
        
    }
}
