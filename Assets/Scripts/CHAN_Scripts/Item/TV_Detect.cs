using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV_Detect : MonoBehaviour
{
    bool onDetect;
    GameObject detectPlayer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!onDetect) return;
        if (Input.GetKeyDown(KeyCode.F) && Item_TVManager_Agora.instance.hasControl == false)
        {
            // �÷��̾�� �������� �ְ�
            detectPlayer.AddComponent<Item_RemoteController>();
            // �ٸ��÷��̾�� �������� ���ֵ��� ���´�.
            Item_TVManager_Agora.instance.TurnControl();
            Debug.LogWarning("������ ���� �ο���");
        }
        else if (Input.GetKeyDown(KeyCode.F) && Item_TVManager_Agora.instance.hasControl == true&& detectPlayer.transform.GetComponent<Item_RemoteController>())
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
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            onDetect = false;
        }
    }
}
