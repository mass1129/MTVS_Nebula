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
            //플레이어가 이동중이면 isSwiming true 
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                anim.SetBool("isSwimming", true);
            }
            else
            {
                anim.SetBool("isSwimming", false);
            }
            //스페이스 누르면 공격(사실은 대화 수단)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("isAttacking");
            }
        }
    }
}
