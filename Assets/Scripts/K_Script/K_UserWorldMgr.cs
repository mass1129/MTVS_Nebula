using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_UserWorldMgr : MonoBehaviourPun
{
    // Start is called before the first frame update
    K_01_Character _character;
    private void Awake()
    {
        CHAN_GameManager.instance.SetPlayer(CHAN_GameManager.instance.prefab);
        _character = CHAN_ClientManager.instance.myCharacter;
    }
    void Start()
    {
       

        _character.PlayerInfoSetting();
        _character.SetActiveObj();
        _character.gridBuildingSystem.FirstLoadBuilding();
        

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
