using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum PlayerStates // Player�� �⺻ ����
{
    Idle,
    ThirdMove,
    FirstMove,
    BuildingMode,
    ThirdSprinting,
    Sitting,
    Jump,
    Falling,
    Death,
    Global
}



// ��� Player�� ����ϴ� ������Ʈ Ŭ����
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


    public float turnSmoothTime = 0.1f;
    public float turnSpeed = 15;
    public float SpceCamSpeed = 15;


    public bool Grounded = true;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;
    public LayerMask GroundLayers;


    public PlayerStates CurrentState { get; set; } // ���� �⺻ ����
 

    // Player�� ������ �ִ� ��� ����, ���� ������. �⺻ ���¿� ��ü ���� ���� ����
    public K_PlayerState<K_Player>[] states;
    public K_StateMachine<K_Player> stateMachine;
    public K_PlayerState<K_Player>[] upperBodyStates;
    public K_StateMachine<K_Player> upperBodyStateMachine;

    private void Awake()
    {
        
    }
    private void Start()
    {
        
    }
    private void OnEnable()
    {
        if (!photonView.IsMine)
            return;

        avatarName = PlayerPrefs.GetString("AvatarName");
        ownIslandID = PlayerPrefs.GetString("Island_ID");
        Debug.Log("AvatarName : " + avatarName + "  Island_ID : " + ownIslandID);
    }
    private void Update()
    {
        if (!photonView.IsMine)
            return;
        Updated();
        //Updated_UpperBody();
        GroundedCheck();
        Debug.Log(CurrentState.ToString() + "," + PhotonNetwork.CurrentRoom.Name + "," + PlayerPrefs.GetString("AvatarName") + "," + avatarName);
        //Debug.Log(cc.isGrounded);
    }

    private void OnAnimatorMove()
    {
        rootMotion += anim.deltaPosition;
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);


    }
    private void OnDrawGizmosSelected()
    {
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
        CurrentState = newState;
        stateMachine.ChangeState(states[(int)newState]);
    }





    // PunRPC �Լ����� �׻� ������Ʈ�� �پ��ִ� ������Ʈ���� ����Ǿ�� �Ѵ�. ���� virtual Ŭ������ �����Ͽ� �� ���������� Ŭ�������� override�� ������ ����� �Ѵ�.
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
