using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̴ϸ� ���� ī�޶� �÷��̾ ���󰡵��� ����
/// </summary>
public class Camera_Follower : MonoBehaviour
{
    Transform playerPos;

    private void Start()
    {
        playerPos = GameObject.Find("Player").transform;

    }
    private void Update()
    {
        Vector3 Pos = new Vector3(playerPos.position.x, playerPos.position.y+10, playerPos.position.z);
        transform.position = Pos;
    }

}
