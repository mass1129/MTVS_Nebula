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
    public List<GameObject> playerIcons=new List<GameObject>();
    public GameObject Icon_prefab;
    public Image IslandIcon;
    Transform player;
    void Start()
    {
        //�����Ҷ� �ϴü��� ������Ʈ, �÷��̾��� ������Ʈ�� ���´�. 
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
                // �÷��̾� �������� ȭ��ǥ ������� 
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
    // ���� �ϴü��� �����ϴ� �������� �ľ��ؾ� �Ѵ�.
    // client manager���� ������ ������ ������Ʈ ������ �����´�.
    // �÷��̾� ������ UI�� �����´�. (UI ������ ���� ����Ʈ ������ŭ ����)
    // �� ����Ŭ���� ���ڸ� ���Ͽ� ������ UI�ϳ� �߰��ϰ� ������ �����Ѵ�. 
    // ���� �ο��� ��ŭ �ݺ�
    // �� ������ ��ǥ�� �� UI�� ��ġ��Ŵ

}
