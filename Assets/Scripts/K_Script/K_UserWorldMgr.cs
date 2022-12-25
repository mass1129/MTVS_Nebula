using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedPeopleSystem;
using Cysharp.Threading.Tasks;
public class K_UserWorldMgr : MonoBehaviourPunCallbacks
{
    public static K_UserWorldMgr instance;
    // Start is called before the first frame update
    K_01_Character _character;
    public List<K_01_Character> playerPool;
    public GridBuildingSystem3D buildingSystem;
    public List<GameObject> buildingSystemSet;
    public List<PlacedObject> loadObjectList;
    private void Awake()
    {
        instance = this;
        _character = CHAN_ClientManager.instance.myCharacter;
        _character.transform.position += Vector3.up * 5;

    }
    void Start()
    {
        _character.transform.position += Vector3.up * 5;

        if (PhotonNetwork.CurrentRoom.PlayerCount <2)
        {
            buildingSystem.BuildingSetup();
            buildingSystem.FirstBuildingLoad().Forget();
        }
        HandleBuildingObj(false);

        _character.PlayerInfoSetting();
        _character.SetActiveObj().Forget();


    }
    public void HandleBuildingObj(bool s)
    {
        for (int i = 0; i < buildingSystemSet.Count; i++)
        {   
            if(buildingSystemSet[i].activeSelf !=s)
            buildingSystemSet[i].SetActive(s);
        }
            
    }
    public void DestoryAllBuilding(Player other)
    {
        int count = loadObjectList.Count;
        for (int i = 0; i < count; i++)
        {
            int temp = i;
            loadObjectList[temp].DestroySelf();
        }
        loadObjectList.Clear();
        PhotonNetwork.SetMasterClient(other);
    }
    public override void OnPlayerEnteredRoom(Player other)
    {

        if (PhotonNetwork.CurrentRoom.Name == other.NickName && PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            DestoryAllBuilding(other);
            
            
        }


       



    }
    public override void OnPlayerLeftRoom(Player other)
    {
       
        

    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log(newMasterClient.NickName + "," + PhotonNetwork.CurrentRoom.Name);
        if (  newMasterClient.NickName == PhotonNetwork.CurrentRoom.Name)
        {
            

            HandleBuildingObj(true);
            buildingSystem.BuildingSetup();
            buildingSystem.FirstBuildingLoad().Forget();
            HandleBuildingObj(false);
        }
        

    }
    }
