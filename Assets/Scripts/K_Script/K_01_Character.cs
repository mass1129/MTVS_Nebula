using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class K_01_Character : K_Player
{
    public GameObject camPos;
    public GameObject playerUI;

    private void Awake()
    {
        if (!photonView.IsMine)
            return;

        // Assult�� ���� �� �ִ� ���� ������ŭ �޸� �Ҵ�, �� ���¿� Ŭ���� �޸� �Ҵ�. states[(int)PlayerStates.Idle].Execute()�� ���� ������� ���.
        states = new K_PlayerState<K_Player>[4];
        states[(int)PlayerStates.Idle] = new K_01_OwnedStates.Idle();
        states[(int)PlayerStates.ThirdMove] = new K_01_OwnedStates.ThirdMove();
        states[(int)PlayerStates.FirstMove] = new K_01_OwnedStates.FirstMove();
        states[(int)PlayerStates.BuildingMode] = new K_01_OwnedStates.BuildingMode();
        //states[(int)PlayerStates.Falling] = new K_01_OwnedStates.Falling();
        //states[(int)PlayerStates.Death] = new K_01_OwnedStates.Death();
        //states[(int)PlayerStates.Global] = new K_01_OwnedStates.Global();

        // upperBodyStates�� ���� states�� ���� ����.
        //upperBodyStates = new K_PlayerState<K_Player>[7];
        //upperBodyStates[(int)PlayerUpperBodyStates.None] = new K_01_OwnedStates.None();
        //upperBodyStates[(int)PlayerUpperBodyStates.Move_Upper] = new K_01_OwnedStates.Move_Upper();
        //upperBodyStates[(int)PlayerUpperBodyStates.Global] = new K_01_OwnedStates.Global_Upper();

        // ���¸� �����ϴ� StateMachine�� �޸� �Ҵ� �� ù ���� ����
        stateMachine = new K_StateMachine<K_Player>();
        stateMachine.SetUp(this, states[(int)PlayerStates.Idle]);
        //stateMachine.SetGlobalState(states[(int)PlayerStates.Global]);

        //upperBodyStateMachine = new K_StateMachine<K_Player>();
        //upperBodyStateMachine.SetUp(this, upperBodyStates[(int)PlayerUpperBodyStates.None]);
        //upperBodyStateMachine.SetGlobalState(upperBodyStates[(int)PlayerUpperBodyStates.Global]);


    }
    private void Start()
    {
        if (photonView.IsMine)
        {
            camPos.SetActive(true);
            playerUI.SetActive(true);
        }
    }

    public override void SetTrigger(string s)
    {
        photonView.RPC("RpcSetTrigger", RpcTarget.AllBuffered, s);
    }

    [PunRPC]
    public override void RpcSetTrigger(string s)
    {
        anim.SetTrigger(s);
    }

    public override void ResetTrigger(string s)
    {
        photonView.RPC("RpcResetTrigger", RpcTarget.AllBuffered, s);
    }

    [PunRPC]
    public override void RpcResetTrigger(string s)
    {
        anim.ResetTrigger(s);
    }

    public override void SetFloat(string s, float f)
    {
        photonView.RPC("RpcSetFloat", RpcTarget.AllBuffered, s, f);
    }

    [PunRPC]
    public override void RpcSetFloat(string s, float f)
    {
        anim.SetFloat(s, f);
    }

    public override void Play(string s, int layer, float normallizedTime)
    {
        photonView.RPC("RpcPlay", RpcTarget.AllBuffered, s, layer, normallizedTime);
    }

    [PunRPC]
    public override void RpcPlay(string s, int layer, float normalizedTime)
    {
        anim.Play(s, layer, normalizedTime);
    }
    bool isBuildingMode = false;

    //public void ChangeToBuildingState()
    //{

    //    isBuildingMode = !isBuildingMode;
    //    camMgr.buildingSystem.SetActive(isBuildingMode);
    //    camMgr.subBuildCamera.gameObject.SetActive(isBuildingMode);
    //}
    public void ChangeToBuildingState()
    {
        if (CurrentState == PlayerStates.Idle)
            ChangeState(PlayerStates.BuildingMode);
        else if (CurrentState == PlayerStates.BuildingMode)
            ChangeState(PlayerStates.Idle);
    }


}
