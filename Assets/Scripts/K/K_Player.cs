using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates // Player�� �⺻ ����
{
    Idle,
    ThirdMove,
    FirstMove,
    SpaceCamMode,
    Jump,
    Falling,
    Death,
    Global
}



// ��� Player�� ����ϴ� ������Ʈ Ŭ����
public class K_Player : MonoBehaviour
{
  
    public Animator anim;
    public CharacterController cc;
    public Transform cam;
    public K_CameraMgr camMgr;




    [HideInInspector] public Vector2 input;
    [HideInInspector] public Vector3 rootMotion;
    [HideInInspector] public Vector3 velocity;
    

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


    private void Start()
    {

    }
    private void Update()
    {
        
        Updated();
        //Updated_UpperBody();
        GroundedCheck();
        Debug.Log(CurrentState.ToString());
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

    
    public virtual void ResetTrigger(string s)
    {
        /*photonView.RPC("RpcResetTrigger", RpcTarget.All, s);*/
    }

  

    public virtual void SetFloat(string s, float f)
    {
        /*photonView.RPC("RpcSetFloat", RpcTarget.All, s, f);*/
    }

    

    public virtual void Play(string s, int layer, float normallizedTime)
    {
        /*photonView.RPC("RpcPlay", RpcTarget.All, s, layer, normallizedTime);*/
    }
    #endregion





}
