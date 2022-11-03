using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 상태 관리에 대한 클래스
public class K_StateMachine<T> where T : class
{
    private T ownerEntity; // StateMachine의 소유주 (에이전트 클래스)
    private K_PlayerState<T> currentState; // 현재 기본 상태
    private K_PlayerState<T> previousState; // 이전 상태
    public K_PlayerState<T> globalState; // 전역 상태

    // StateMachine을 참조 클래스로 사용하는 에이전트에서 호출. 에이전트의 상태 초기화.
    public void SetUp(T owner, K_PlayerState<T> entryState)
    {
        ownerEntity = owner;
        currentState = null;
        previousState = null;
        //globalState = null;

        ChangeState(entryState);
    }

    public void Execute()
    {
        if (currentState != null)
        {
            currentState.Execute(ownerEntity);
        }

        //if (globalState != null)
        //{
        //    globalState.Execute(ownerEntity);
        //}
    }

    public void ChangeState(K_PlayerState<T> newState)
    {
        // 새로 바꾸려는 상태가 비어있으면 상태 변경 X
        if (newState == null) return;

        if (currentState != null)
        {
            // 상태 변경되면 현재 상태가 이전 상태가 됨.
            previousState = currentState;
            currentState.Exit(ownerEntity);
        }

        // 새로운 상태로 변경하고, 새로 바뀐 상태의 Enter() 메소드 호출
        currentState = newState;
        currentState.Enter(ownerEntity);
    }
    public void RevertToPreviousState()
    {
        ChangeState(previousState);
    }

    //public void SetGlobalState(K_PlayerState<T> newState)
    //{
    //    globalState = newState;
    //}
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

}
