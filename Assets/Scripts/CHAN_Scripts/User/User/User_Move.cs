using Photon.Pun;
using UnityEngine;

/// <summary>
/// 유저의 기본 이동 입력기
/// 커맨드 값 : WASD , Shift
/// </summary>

public class User_Move : MonoBehaviourPun, IPunObservable
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public float userSpeed;
    public float rotateSpeed;
    public float speedMultiplier;


    void Start()
    {

    }
    Vector3 dir;
    float lerpSpeed = 10;
    public bool islandSelected;

    Vector3 receivePos;
    Quaternion receiveRot;
    //W: 전진
    //S: 후진 
    //A: 반시계방향 회전 
    //D: 시계방향 회전
    //MouseUp : 고래 위로 전환
    //MouseDown : 고래 아래로 전환
    //Shift: 대쉬
    //Space bar : 고래 울음소리
    float _Input_Forward;
    float _Input_Rotate_Yaw;
    float _Input_Rotate_Pitch;

    #region 플레이어 입력기
    float Input_Forward
    {
        get { return _Input_Forward; }
        set { _Input_Forward = Input.GetAxis("Vertical"); }
    }

    float Input_Rotate_Yaw
    {
        get { return _Input_Rotate_Yaw; }
        set
        { _Input_Rotate_Yaw = Input.GetAxis("Horizontal"); }
    }
    float Input_Rotate_Pitch
    { get { return _Input_Rotate_Pitch; }
        set { _Input_Rotate_Pitch = Input.GetAxis("Mouse Y"); }
    }
        #endregion
    
    float Rotate_Pitch;
    float Rotate_Yaw;

    //특수효과
    //유저들끼리 가까이 전근해 있을때 이동속도가 증가하게 됨.

    [System.Obsolete]
    void Update()
    {

        
        if (photonView.IsMine)
        {
            // 일단 이동방향은 앞뒤로 갈 수 있도록 만든다. 
            dir = (Input_Forward * transform.forward).normalized;
            Rotate_Pitch -= Input_Rotate_Pitch * rotateSpeed * Time.deltaTime;
            Rotate_Yaw += Input_Rotate_Yaw * rotateSpeed * Time.deltaTime;
            //쉬프트키를 눌렀을 때 대쉬되도록 만든다.
            float totalSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalSpeed = userSpeed * speedMultiplier;
            }
            else
            {
                totalSpeed = userSpeed;
            }
            transform.position += dir * totalSpeed * Time.deltaTime;
            transform.localRotation = Quaternion.EulerAngles(Rotate_Pitch, Rotate_Yaw, 0);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        }
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else if (stream.IsReading)
        {
            receivePos=(Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
