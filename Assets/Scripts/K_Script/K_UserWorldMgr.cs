using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedPeopleSystem;
public class K_UserWorldMgr : MonoBehaviourPun
{
    // Start is called before the first frame update
    K_01_Character _character;
    public GridBuildingSystem3D buildingSystem;
    private void Awake()
    {
        
        _character = CHAN_ClientManager.instance.myCharacter;

    }
    void Start()
    {
        _character.PlayerInfoSetting();
        StartCoroutine(_character.SetActiveObj());
        
        
    }

    public void PlayerMoveCC(bool s)
    {
        _character.canMove = s;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
