using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ���� ������ ���� Ŭ����
public class K_StateMachine<T> where T : class
{
    private T ownerEntity; // StateMachine�� ������ (������Ʈ Ŭ����)
    private K_PlayerState<T> currentState; // ���� �⺻ ����
    private K_PlayerState<T> previousState; // ���� ����
    public K_PlayerState<T> globalState; // ���� ����

    // StateMachine�� ���� Ŭ������ ����ϴ� ������Ʈ���� ȣ��. ������Ʈ�� ���� �ʱ�ȭ.
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
        // ���� �ٲٷ��� ���°� ��������� ���� ���� X
        if (newState == null) return;

        if (currentState != null)
        {
            // ���� ����Ǹ� ���� ���°� ���� ���°� ��.
            previousState = currentState;
            currentState.Exit(ownerEntity);
        }

        // ���ο� ���·� �����ϰ�, ���� �ٲ� ������ Enter() �޼ҵ� ȣ��
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
