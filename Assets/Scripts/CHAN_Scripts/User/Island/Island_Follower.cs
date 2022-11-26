using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island_Follower : MonoBehaviour
{
    // 섬의 이미지가 유저를 따라가도록 만들것임
    Transform playerPos;
    void Start()
    {
        playerPos = CHAN_PlayerManger.LocalPlayerInstance.transform.GetChild(5).gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponentInParent<Island_Profile>().turn) return;
        ShowImage();
    }
    void ShowImage()
    {
        transform.forward = playerPos.forward;
    }
}
