using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class K_01_Character : K_Player
{
    public GameObject camPos;
    public List<GameObject> playerUI;
    public List<GameObject> inActiveObj;
    private void Awake()
    {
        if (!photonView.IsMine)
            this.enabled = false;
        
        // Assult�� ���� �� �ִ� ���� ������ŭ �޸� �Ҵ�, �� ���¿� Ŭ���� �޸� �Ҵ�. states[(int)PlayerStates.Idle].Execute()�� ���� ������� ���.
        states = new K_PlayerState<K_Player>[8];
        states[(int)PlayerStates.Idle] = new K_01_OwnedStates.Idle();
        states[(int)PlayerStates.ThirdMove] = new K_01_OwnedStates.ThirdMove();
        states[(int)PlayerStates.FirstMove] = new K_01_OwnedStates.FirstMove();
        states[(int)PlayerStates.BuildingMode] = new K_01_OwnedStates.BuildingMode();
        states[(int)PlayerStates.ThirdSprinting] = new K_01_OwnedStates.ThirdSprinting();
        states[(int)PlayerStates.Sitting] = new K_01_OwnedStates.Sitting();
        states[(int)PlayerStates.Jump] = new K_01_OwnedStates.Jump();
        states[(int)PlayerStates.Falling] = new K_01_OwnedStates.Falling();
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
        
    }

    public IEnumerator SetActiveObj()
    {
        if (photonView.IsMine)
        {
            canMove = false;
            camPos.SetActive(true);
            charCustom.LoadCharacterFromFile(PlayerPrefs.GetString("AvatarName"));
            if (PhotonNetwork.IsMasterClient)
            {
                gridBuildingSystem.gameObject.SetActive(true);
                gridBuildingSystem.TestLoad(PlayerPrefs.GetString("User_Island_ID"));
                
            }
            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < playerUI.Count; i++)
            {
                playerUI[i].SetActive(true);
            }

            yield return new WaitForSeconds(0.1f);
            InActiveObj();
            yield return new WaitForSeconds(1f);
            canMove = true;
            yield break;

        }
        yield break;
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
       if(PhotonNetwork.CurrentRoom.Name == avatarName)
       {
            if (CurrentState == PlayerStates.Idle)
                ChangeState(PlayerStates.BuildingMode);
            else if (CurrentState == PlayerStates.BuildingMode)
                ChangeState(PlayerStates.Idle);
       }
       
    }


}
