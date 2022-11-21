using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TV_Detect : MonoBehaviour
{
    bool onDetect;
    GameObject detectPlayer;
    //readonly Item_TVManager_Agora _mgr = Item_TVManager_Agora.instance;
    public Text message;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // TV�� �÷��̾ �������� �������� �׳� �Ѿ��.
        if (!onDetect) return;
        // �÷��̾�� ���� ������ �ְ� 
        // ���� �������� ������
        // 'F' Ű�� ������ �� �ش� �̺�Ʈ�� �߻�
        if (Input.GetKeyDown(KeyCode.F) && Item_TVManager_Agora.instance.hasControl == false && !Item_TVManager_Agora.instance.moving)
        {
            // �÷��̾�� �������� �ְ�
            detectPlayer.AddComponent<Item_RemoteController>();
            // �ٸ��÷��̾�� �������� ���ֵ��� ���´�.
            Item_TVManager_Agora.instance.TurnControl();
            Debug.LogWarning("������ ���� �ο���");
        }
        else if (Input.GetKeyDown(KeyCode.F) && Item_TVManager_Agora.instance.hasControl == true && detectPlayer.transform.GetComponent<Item_RemoteController>() && !Item_TVManager_Agora.instance.moving)
        {
            // �������� ������ �ִ� �÷��̾��� �������� �����ϰ�
            Destroy(detectPlayer.GetComponent<Item_RemoteController>());
            // ������ ������ �ٽ� �ο��Ѵ�.
            Item_TVManager_Agora.instance.TurnControl();
            Debug.LogWarning("������ ���� �ʱ�ȭ");
        }
    }
    // collider �����ϸ�, �װ��� ����̸� Text ������ �Ѵ�. 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            detectPlayer = other.gameObject;
            onDetect = true;
            // ���� �ƹ��� TV������� ���� ���ߴٸ� �ȳ��޼��� ����
            if (!Item_TVManager_Agora.instance.hasControl)
            {
                message.text = "'F' �� ���� TV�ѱ�";
            }
            else
            {
                message.text = "'F' �� ���� TV����";
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            onDetect = false;
            message.text = "TV";

        }
    }
}
