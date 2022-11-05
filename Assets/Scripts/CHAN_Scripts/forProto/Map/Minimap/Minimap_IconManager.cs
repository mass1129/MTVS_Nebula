using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �̴ϸʻ� �÷��̾�, �ϴü��������� �̴ϸʻ����� ǥ�õǵ��� �����Ѵ�.
/// </summary>
public class Minimap_IconManager : MonoBehaviour
{
    public static Minimap_IconManager instance;
    private void Awake()
    {
        instance = this;
    }
    public List<GameObject> playerIcons=new List<GameObject>();
    public GameObject Icon_prefab;
    public Transform UserIcons;
    Transform player;
    void Start()
    {
        //�����Ҷ� �ϴü��� ������Ʈ, �÷��̾��� ������Ʈ�� ���´�. 
        player = CHAN_PlayerManger.LocalPlayerInstance.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (playerIcons.Count != CHAN_ClientManager.players.Count)
        {
            Create_User_Icon(); 
        }
        else
        {
            Update_User_Icon();
           
        }
    }

    void Create_User_Icon()
    {
        while(playerIcons.Count < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            GameObject obj = Instantiate(Icon_prefab,UserIcons);
            playerIcons.Add(obj);
        }
    }
    public void Remove_User_Icon()
    {
        foreach (GameObject obj in playerIcons)
        {
            Destroy(obj);
        }
        playerIcons.Clear();
    }
    void Update_User_Icon()
    {
        if (playerIcons.Count == 0)
            return;
        List<PhotonView> p = CHAN_ClientManager.players;
        for (int i = 0; i < CHAN_ClientManager.players.Count; i++)
        { 
            if (playerIcons.Count != CHAN_ClientManager.players.Count) 
                return;
            if (CHAN_ClientManager.players[i].IsMine)
            {
                playerIcons[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                playerIcons[i].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                playerIcons[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                playerIcons[i].transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            }
            Vector3 pos = p[i].gameObject.transform.position;
            playerIcons[i].transform.position = new Vector3(pos.x, 450, pos.z);
            float rotY = Mathf.Deg2Rad *p[i].gameObject.transform.rotation.eulerAngles.y;
            playerIcons[i].transform.localRotation = Quaternion.EulerAngles(0, rotY, 0);
        }
    }
    //void Update_Icon_Position()
    //{
    //    if (playerIcons.Count != CHAN_ClientManager.players.Count || playerIcons.Count==0)
    //        return;
    //    for (int i = 0; i < playerIcons.Count; i++)
    //    {
            
           
    //    }
    //}
    // ���� �ϴü��� �����ϴ� �������� �ľ��ؾ� �Ѵ�.
    // client manager���� ������ ������ ������Ʈ ������ �����´�.
    // �÷��̾� ������ UI�� �����´�. (UI ������ ���� ����Ʈ ������ŭ ����)
    // �� ����Ŭ���� ���ڸ� ���Ͽ� ������ UI�ϳ� �߰��ϰ� ������ �����Ѵ�. 
    // ���� �ο��� ��ŭ �ݺ�
    // �� ������ ��ǥ�� �� UI�� ��ġ��Ŵ

}
