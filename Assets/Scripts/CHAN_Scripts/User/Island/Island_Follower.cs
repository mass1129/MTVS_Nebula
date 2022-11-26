using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island_Follower : MonoBehaviour
{
    // ���� �̹����� ������ ���󰡵��� �������
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
