using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cysharp.Threading.Tasks;

public class K_01_Character : K_Player
{
    
    private void Awake()
    {
        if (!photonView.IsMine)
            return;
        
        // Assult가 가질 수 있는 상태 개수만큼 메모리 할당, 각 상태에 클래스 메모리 할당. states[(int)PlayerStates.Idle].Execute()와 같은 방식으로 사용.
        states = new K_PlayerState<K_Player>[9];
        states[(int)PlayerStates.Idle] = new K_01_OwnedStates.Idle();
        states[(int)PlayerStates.ThirdMove] = new K_01_OwnedStates.ThirdMove();
        states[(int)PlayerStates.FirstMove] = new K_01_OwnedStates.FirstMove();
        states[(int)PlayerStates.BuildingMode] = new K_01_OwnedStates.BuildingMode();
        states[(int)PlayerStates.ThirdSprinting] = new K_01_OwnedStates.ThirdSprinting();
        states[(int)PlayerStates.Sitting] = new K_01_OwnedStates.Sitting();
        states[(int)PlayerStates.Jump] = new K_01_OwnedStates.Jump();
        states[(int)PlayerStates.Falling] = new K_01_OwnedStates.Falling();
        states[(int)PlayerStates.FreeCamMode] = new K_01_OwnedStates.FreeCamMode();

        // 상태를 관리하는 StateMachine에 메모리 할당 및 첫 상태 결정
        stateMachine = new K_StateMachine<K_Player>();
        stateMachine.SetUp(this, states[(int)PlayerStates.Idle]);



    }

    public override void SetTrigger(string s)
    {
        photonView.RPC("RpcSetTrigger", RpcTarget.All, s);
    }

    [PunRPC]
    public override void RpcSetTrigger(string s)
    {
        anim.SetTrigger(s);
    }

    public override void ResetTrigger(string s)
    {
        photonView.RPC("RpcResetTrigger", RpcTarget.All, s);
    }

    [PunRPC]
    public override void RpcResetTrigger(string s)
    {
        anim.ResetTrigger(s);
    }

    public override void SetFloat(string s, float f)
    {
        photonView.RPC("RpcSetFloat", RpcTarget.All, s, f);
    }

    [PunRPC]
    public override void RpcSetFloat(string s, float f)
    {
        anim.SetFloat(s, f);
    }

    public override void Play(string s, int layer, float normallizedTime)
    {
        photonView.RPC("RpcPlay", RpcTarget.All, s, layer, normallizedTime);
    }

    [PunRPC]
    public override void RpcPlay(string s, int layer, float normalizedTime)
    {
        anim.Play(s, layer, normalizedTime);
    }



    public void ChangeToBuildingState()
    {
        if (!photonView.IsMine)
            return;
        if (PhotonNetwork.CurrentRoom.Name == avatarName)
       {
            if (CurrentState == PlayerStates.Idle)
                ChangeState(PlayerStates.BuildingMode);
            else if (CurrentState == PlayerStates.BuildingMode)
                ChangeState(PlayerStates.Idle);
       }
       
    }


}
