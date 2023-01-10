using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedPeopleSystem;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using PN = Photon.Pun.PhotonNetwork;

public class K_UserWorldMgr : MonoBehaviourPunCallbacks
{
    public static K_UserWorldMgr instance;
    // Start is called before the first frame update
    K_01_Character _character;
    public List<K_01_Character> playerPool;
    public GridBuildingSystem3D buildingSystem;
    public List<GameObject> buildingSystemSet;
    public List<PlacedObject> loadObjectList;
    public string playerPrefeb;



    private void Awake()
    {

            instance = this;

    }


    void Start()
    {
        var player = PhotonNetwork.Instantiate(playerPrefeb, new Vector3((int)Random.Range(40, 60), 3, (int)Random.Range(40, 60)), Quaternion.identity);
        _character = player.GetComponent<K_01_Character>(); 

        if (PhotonNetwork.CurrentRoom.PlayerCount <2)
        {
            buildingSystem.BuildingSetup();
            buildingSystem.FirstBuildingLoad().Forget();
        }
        HandleBuildingObj(false);

        _character.PlayerSetting();
    }



    public void LoadSkyScene()
    {
        //_character.GetComponent<K_PlayerItemSystem>().DestoryPlayer();
        CHAN_GameManager.instance.Go_Sky_Scene();
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
