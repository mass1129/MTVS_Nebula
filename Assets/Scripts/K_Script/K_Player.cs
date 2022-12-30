using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using AdvancedPeopleSystem;
using Cysharp.Threading.Tasks;
public enum PlayerStates // Player의 기본 상태
{
    Idle,
    ThirdMove,
    FirstMove,
    BuildingMode,
    ThirdSprinting,
    Sitting,
    Jump,
    Falling,
    FreeCamMode,
    Death,
    Global
}



// 모든 Player가 사용하는 에이전트 클래스
public class K_Player : MonoBehaviourPun, IPunObservable
{
    

    public Animator anim;
    public CharacterController cc;
    public Transform cam;
    public K_CameraMgr camMgr;
    public K_PlayerItemSystem itemSystem;
    
    [System.NonSerialized]
    public string avatarName = null;
    [System.NonSerialized]
    public string ownIslandID = null;

    [HideInInspector] public Vector2 input;
    [HideInInspector] public Vector3 rootMotion;
    [HideInInspector] public Vector3 velocity;

    public float yVelocity = 0;
    public float jumpHeight;
    public float gravity = -15.0f;
    public float stepDown;
    public float airControl;
    public float jumpDamp;
    public float groundSpeed;
    public float AnimBlendSpeed = 11f;
    [HideInInspector] public float h, v;
    [HideInInspector] public Vector3 dir;

    public bool canMove = true;
    public AudioSource microphone;

    public float turnSmoothTime = 0.1f;
    public float turnSpeed = 15;
    public float SpceCamSpeed = 15;


    public bool Grounded = true;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;
    public LayerMask GroundLayers;

    public GameObject playerUi;

    public PlayerStates CurrentState { get; set; } // 현재 기본 상태


    public GameObject camPos;

    // Player가 가지고 있는 모든 상태, 상태 관리자. 기본 상태와 상체 상태 따로 관리
    public K_PlayerState<K_Player>[] states;
    public K_StateMachine<K_Player> stateMachine;
    public K_PlayerState<K_Player>[] upperBodyStates;
    public K_StateMachine<K_Player> upperBodyStateMachine;


    private void Update()
    {
        if (!photonView.IsMine)
            return;
        Updated();
        //Updated_UpperBody();
        GroundedCheck();
        
       if(Input.GetKeyDown(KeyCode.O))
        Debug.Log(CurrentState.ToString() + "," + PhotonNetwork.CurrentRoom.Name + "," + avatarName + "," + avatarName + "," + ownIslandID+ "," + PlayerPrefs.GetString("User_Island_ID"));
        //Debug.Log(cc.isGrounded);
    }

   

    public async UniTask PlayerSetting()
    {
        if (photonView.IsMine)
        {
            avatarName = CHAN_GameManager.instance.avatarName;
            ownIslandID = CHAN_GameManager.instance.ownIslandId;
            Debug.Log("AvatarName : " + avatarName + "  Island_ID : " + ownIslandID);
            canMove = false;
            camPos.SetActive(true);
            playerUi.SetActive(true);
            await itemSystem.SetItemSystem(avatarName);
            canMove = true;

        }
    }


    private void OnAnimatorMove()
    {
        if (!photonView.IsMine)
            return;
        rootMotion += anim.deltaPosition;
    }

    private void GroundedCheck()
    {
        if (!photonView.IsMine)
            return;
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);


    }
    private void OnDrawGizmosSelected()
    {
        if (!photonView.IsMine)
            return;
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);
    }


    #region StateMachine
    public void Updated()
    {
        stateMachine.Execute();
    }

    public void Updated_UpperBody()
    {
        upperBodyStateMachine.Execute();
    }

    public void ChangeState(PlayerStates newState)
    {
        if (!photonView.IsMine)
            return;
        CurrentState = newState;
        stateMachine.ChangeState(states[(int)newState]);
    }


    


    // PunRPC 함수들은 항상 오브젝트에 붙어있는 컴포넌트에서 실행되어야 한다. 따라서 virtual 클래스로 구현하여 각 직업군들의 클래스에서 override로 재정의 해줘야 한다.
    public virtual void SetTrigger(string s)
    {
        /*photonView.RPC("RpcSetTrigger", RpcTarget.All, s);*/
    }

    [PunRPC]
    public virtual void RpcSetTrigger(string s)
    {
        /*anim.SetTrigger(s);*/
    }

    public virtual void ResetTrigger(string s)
    {
        /*photonView.RPC("RpcResetTrigger", RpcTarget.All, s);*/
    }

    [PunRPC]
    public virtual void RpcResetTrigger(string s)
    {
        /*anim.ResetTrigger(s);*/
    }

    public virtual void SetFloat(string s, float f)
    {
        /*photonView.RPC("RpcSetFloat", RpcTarget.All, s, f);*/
    }

    [PunRPC]
    public virtual void RpcSetFloat(string s, float f)
    {
        /*anim.SetFloat(s, f);*/
    }

    public virtual void Play(string s, int layer, float normallizedTime)
    {
        /*photonView.RPC("RpcPlay", RpcTarget.All, s, layer, normallizedTime);*/
    }

    [PunRPC]
    public virtual void RpcPlay(string s, int layer, float normalizedTime)
    {
        /*anim.Play(s, layer, normalizedTime);*/
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
    #endregion



}
