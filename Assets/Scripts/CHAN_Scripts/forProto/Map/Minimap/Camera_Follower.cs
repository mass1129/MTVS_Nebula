using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̴ϸ� ���� ī�޶� �÷��̾ ���󰡵��� ����
/// </summary>
public class Camera_Follower : MonoBehaviour
{
    public float yPos;
    private void Update()
    {
        //�÷��̾ ������ �׳� ���� ��Ų��.
        if (!CHAN_PlayerManger.LocalPlayerInstance) return;
        
            Vector3 Pos = new Vector3(CHAN_PlayerManger.LocalPlayerInstance.transform.position.x, yPos, CHAN_PlayerManger.LocalPlayerInstance.transform.position.z);
            transform.position = Pos;
        //float RotY =CHAN_PlayerManger.LocalPlayerInstance.transform.rotation.y;
        //transform.rotation =Quaternion.Euler(new Vector3(90, RotY, 0));


    }

}
