using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class K_01_Character : K_Player
{
    

    private void Awake()
    {
       

        // Assult가 가질 수 있는 상태 개수만큼 메모리 할당, 각 상태에 클래스 메모리 할당. states[(int)PlayerStates.Idle].Execute()와 같은 방식으로 사용.
        states = new K_PlayerState<K_Player>[4];
        states[(int)PlayerStates.Idle] = new K_01_OwnedStates.Idle();
        states[(int)PlayerStates.ThirdMove] = new K_01_OwnedStates.ThirdMove();
        states[(int)PlayerStates.FirstMove] = new K_01_OwnedStates.FirstMove();
        states[(int)PlayerStates.SpaceCamMode] = new K_01_OwnedStates.SpaceCamMode();
        //states[(int)PlayerStates.Falling] = new K_01_OwnedStates.Falling();
        //states[(int)PlayerStates.Death] = new K_01_OwnedStates.Death();
        //states[(int)PlayerStates.Global] = new K_01_OwnedStates.Global();

        // upperBodyStates는 기존 states와 따로 관리.
        //upperBodyStates = new K_PlayerState<K_Player>[7];
        //upperBodyStates[(int)PlayerUpperBodyStates.None] = new K_01_OwnedStates.None();
        //upperBodyStates[(int)PlayerUpperBodyStates.Move_Upper] = new K_01_OwnedStates.Move_Upper();
        //upperBodyStates[(int)PlayerUpperBodyStates.Global] = new K_01_OwnedStates.Global_Upper();

        // 상태를 관리하는 StateMachine에 메모리 할당 및 첫 상태 결정
        stateMachine = new K_StateMachine<K_Player>();
        stateMachine.SetUp(this, states[(int)PlayerStates.Idle]);
        //stateMachine.SetGlobalState(states[(int)PlayerStates.Global]);

        //upperBodyStateMachine = new K_StateMachine<K_Player>();
        //upperBodyStateMachine.SetUp(this, upperBodyStates[(int)PlayerUpperBodyStates.None]);
        //upperBodyStateMachine.SetGlobalState(upperBodyStates[(int)PlayerUpperBodyStates.Global]);


    }
    private void Start()
    {
       
    }

    public override void SetTrigger(string s)
    {
        anim.SetTrigger(s);
    }


    public override void ResetTrigger(string s)
    {
        anim.ResetTrigger(s);
    }


    public override void SetFloat(string s, float f)
    {
        anim.SetFloat(s, f);
    }



    public override void Play(string s, int layer, float normallizedTime)
    {
        anim.Play(s, layer, normallizedTime);
    }

}
