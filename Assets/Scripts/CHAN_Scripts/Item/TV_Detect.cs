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
        // TV가 플레이어를 감지하지 못했으면 그냥 넘어간다.
        if (!onDetect) return;
        // 플레이어에게 제어 권한이 있고 
        // 벽이 움직이지 않으며
        // 'F' 키를 눌렀을 때 해당 이벤트가 발생
        if (Input.GetKeyDown(KeyCode.F) && Item_TVManager_Agora.instance.hasControl == false && !Item_TVManager_Agora.instance.moving)
        {
            // 플레이어에게 리모콘을 주고
            detectPlayer.AddComponent<Item_RemoteController>();
            // 다른플레이어에게 리모콘을 못주도록 막는다.
            Item_TVManager_Agora.instance.TurnControl();
            Debug.LogWarning("리모콘 권한 부여됨");
        }
        else if (Input.GetKeyDown(KeyCode.F) && Item_TVManager_Agora.instance.hasControl == true && detectPlayer.transform.GetComponent<Item_RemoteController>() && !Item_TVManager_Agora.instance.moving)
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
            // 아직 아무도 TV제어권을 갖지 못했다면 안내메세지 송출
            if (!Item_TVManager_Agora.instance.hasControl)
            {
                message.text = "'F' 를 눌러 TV켜기";
            }
            else
            {
                message.text = "'F' 를 눌러 TV끄기";
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
