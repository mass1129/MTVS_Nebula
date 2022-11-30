using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cysharp.Threading.Tasks;

public class K_01_Character : K_Player
{
    public GameObject camPos;
    public List<GameObject> playerUI;
    public List<GameObject> inActiveObj;
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

    public async UniTaskVoid SetActiveObj()
    {
        if (photonView.IsMine)
        {
            canMove = false;
            camPos.SetActive(true);
            charCustom.LoadCharacterFromFile(PlayerPrefs.GetString("AvatarName"));
            await UniTask.DelayFrame(50);
            if (PhotonNetwork.IsMasterClient&&PhotonNetwork.CurrentRoom.PlayerCount<2)
            {
                gridBuildingSystem.gameObject.SetActive(true);
                gridBuildingSystem.TestLoad(PlayerPrefs.GetString("User_Island_ID"));
                
            }
            await UniTask.DelayFrame(50);
            itemSystem.UpdateItemSystem();
            await UniTask.DelayFrame(50);
            for (int i = 0; i < playerUI.Count; i++)
            {
                int temp = i;
                playerUI[temp].SetActive(true);
                
            }
            await UniTask.DelayFrame(50);
            itemSystem.ItemLoad();
            await UniTask.DelayFrame(50);
            InActiveObj();
            await UniTask.DelayFrame(50);
            canMove = true;

        }
    }
    
    public void PlayerInfoSetting()
    {
        if (photonView.IsMine)
        {
            avatarName = PlayerPrefs.GetString("AvatarName");
            ownIslandID = PlayerPrefs.GetString("Island_ID");
            Debug.Log("AvatarName : " + avatarName + "  Island_ID : " + ownIslandID);
        }
    }
    public void InActiveObj()
    {
        photonView.RPC("RpcInActiveTrigger", RpcTarget.AllBuffered);
        
    }
    [PunRPC]
    public void RpcInActiveTrigger()
    {
        for (int i = 0; i < inActiveObj.Count; i++)
        {
            inActiveObj[i].SetActive(false);
        }
       
    }


    private void OnEnable()
    {
        
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
