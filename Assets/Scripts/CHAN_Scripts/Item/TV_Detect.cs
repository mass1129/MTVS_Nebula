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
            // 플레이어에게 리모콘을 주고
            detectPlayer.AddComponent<Item_RemoteController>();
            // 다른플레이어에게 리모콘을 못주도록 막는다.
            Item_TVManager_Agora.instance.TurnControl();
            Debug.LogWarning("리모콘 권한 부여됨");
        }
        else if (Input.GetKeyDown(KeyCode.F) && Item_TVManager_Agora.instance.hasControl == true&& detectPlayer.transform.GetComponent<Item_RemoteController>())
        {
            // 리모콘을 가지고 있던 플레이어의 리모콘을 삭제하고
            Destroy(detectPlayer.GetComponent<Item_RemoteController>());
            // 리모콘 권한을 다시 부여한다.
            Item_TVManager_Agora.instance.TurnControl();
            Debug.LogWarning("리모콘 권한 초기화");
        }
    }
    // collider 감지하면, 그것이 사람이면 Text 켜지게 한다. 
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
