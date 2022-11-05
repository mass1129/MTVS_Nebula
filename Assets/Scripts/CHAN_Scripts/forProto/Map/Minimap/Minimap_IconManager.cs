using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 미니맵상 플레이어, 하늘섬아이콘을 미니맵상으로 표시되도록 구현한다.
/// </summary>
public class Minimap_IconManager : MonoBehaviour
{
    public List<GameObject> playerIcons=new List<GameObject>();
    public GameObject Icon_prefab;
    public Image IslandIcon;
    Transform player;
    void Start()
    {
        //시작할때 하늘섬의 오브젝트, 플레이어의 오브젝트를 들고온다. 
        player = CHAN_PlayerManger.LocalPlayerInstance.transform;
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if (playerIcons.Count != CHAN_ClientManager.players.Count)
        {
            Create_User_Icon();
            Update_User_Icon();
        }
        Update_Icon_Position();
        
    }
    void Create_User_Icon()
    {
        if (playerIcons.Count < CHAN_ClientManager.players.Count)
        {
            GameObject obj = Instantiate(Icon_prefab);
            playerIcons.Add(obj);
        }
        else if (playerIcons.Count > CHAN_ClientManager.players.Count)
        {
            playerIcons.Remove(playerIcons[0]);
            Destroy(playerIcons[0]);
        }
    }
    void Update_User_Icon()
    {
        
        foreach (PhotonView pv in CHAN_ClientManager.players)
        {
            if (pv.IsMine)
            { 
                // 플레이어 아이콘을 화살표 모양으로 
            }
        }
    }
    void Update_Icon_Position()
    {
        
        for (int i = 0; i < CHAN_ClientManager.players.Count; i++)
        {
            Vector3 pos = CHAN_ClientManager.players[i].gameObject.transform.position;
            playerIcons[i].transform.position = new Vector3(pos.x, 150, pos.z);
        }
    }
    // 현재 하늘섬에 존재하는 유저들을 파악해야 한다.
    // client manager에서 저장한 유저의 오브젝트 정보를 가져온다.
    // 플레이어 아이콘 UI를 가져온다. (UI 갯수는 유저 리스트 갯수만큼 존재)
    // 매 사이클마다 숫자를 비교하여 적으면 UI하나 추가하고 적으면 삭제한다. 
    // 유저 인원수 만큼 반복
    // 각 유저의 좌표에 그 UI를 위치시킴

}
