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
    float userSpeed;
    public float accMultipiler;
    public float MaxSpeed;
    public float rotateSpeed;
    public float speedMultiplier;
    public GameObject CVCam;
    public GameObject OrcaObj;
    public ParticleSystem sornar;
    //public GameObject Audio;

    void Start()
    {
        if (!photonView.IsMine)
        {
            CVCam.SetActive(false);
            //Audio.SetActive(false);
        }
        OrcaObj.SetActive(true);
    }
    Vector3 dir;
    float lerpSpeed = 10;
    public bool islandSelected;
    float Rotate_Pitch;
    float Rotate_Yaw;
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

    //특수효과
    //유저들끼리 가까이 전근해 있을때 이동속도가 증가하게 됨.

    [System.Obsolete]
    void Update()
    {
        if (photonView.IsMine)
        {
            #region 플레이어 입력기
            float Input_Forward = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
            float Input_Rotate_Yaw = Input.GetAxis("Mouse X");
            float Input_Rotate_Pitch = Input.GetAxis("Mouse Y");
            #endregion
            // 일단 이동방향은 앞뒤로 갈 수 있도록 만든다. 
            dir = (transform.forward).normalized;
            Rotate_Pitch -= Input_Rotate_Pitch * rotateSpeed * Time.deltaTime;        
            Rotate_Yaw += Input_Rotate_Yaw * rotateSpeed * Time.deltaTime;
            //쉬프트키를 눌렀을 때 대쉬되도록 만든다.
            float totalSpeed;
            if (Input.GetKey(KeyCode.W))
            {
                userSpeed += Input_Forward *accMultipiler* Time.deltaTime;
            }
            else
            {
                userSpeed -=  accMultipiler *2* Time.deltaTime;
            }
            userSpeed = Mathf.Clamp(userSpeed, 0, MaxSpeed);
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalSpeed = userSpeed * speedMultiplier;
            }
            else
            {
                totalSpeed = userSpeed;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Do_Shout();
            }
            transform.position += dir * totalSpeed * Time.deltaTime;
            transform.localRotation = Quaternion.EulerAngles(Mathf.Clamp(Rotate_Pitch,-70*Mathf.Deg2Rad, 70 * Mathf.Deg2Rad), Rotate_Yaw, 0);


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
            stream.SendNext(transform.localRotation);
        }
        else if (stream.IsReading)
        {
            receivePos=(Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
    void Do_Shout()
    {
        photonView.RPC("RPCDo_Shout", RpcTarget.All);
    }
    [PunRPC]
    void RPCDo_Shout()
    {
        sornar.Play();
    }
}
