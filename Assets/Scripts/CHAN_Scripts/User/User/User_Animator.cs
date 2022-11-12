using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class User_Animator : MonoBehaviourPun
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        if (transform.parent.gameObject.GetComponent<PhotonView>().IsMine)
        { 
            //�÷��̾ �̵����̸� isSwiming true 
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                anim.SetBool("isSwimming", true);
            }
            else
            {
                anim.SetBool("isSwimming", false);
            }
            //�����̽� ������ ����(����� ��ȭ ����)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("isAttacking");
            }
        }
    }
}
