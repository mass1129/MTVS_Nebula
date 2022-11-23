using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHAN_PlayerManger : MonoBehaviourPun
{
    // ���� ������ ������Ʈ ������ LocalPlayerInstance�� �����Ѵ�. 
    public static GameObject LocalPlayerInstance;
    private void Awake()
    {
        // ĳ���Ͱ� �ڱ��ڽ��� �� ����
        if (CHAN_GameManager.instance.prefab == CHAN_GameManager.instance.WhalePrepab)
        {
            // ������̵� �����̶�� 
            if (photonView.IsMine)
            {
                //������Ʈ �ڽĿ� �ִ� �÷��̾� �������� Ȱ��ȭ�ϰ� �ٸ� ���� �������� ��Ȱ��ȭ �Ѵ�.
                CHAN_PlayerManger.LocalPlayerInstance = this.gameObject;
                transform.GetChild(1).GetComponent<User_IconMove>().OnPlayerIcon();
            }
            else
            {
                //�÷��̾� ������ ��Ȱ��ȭ�ϰ� ���� �������� Ȱ��ȭ�Ѵ�.
                gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                transform.GetChild(1).GetComponent<User_IconMove>().OnUserIcon();
            }
        }
        //JoinRoom ���� �÷��̾ �����ǹǷ� �ε��, ������Ʈ�� �������� �ȵȴ�. 
        DontDestroyOnLoad(gameObject);
    }  
}
