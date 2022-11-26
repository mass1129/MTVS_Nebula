using Photon.Pun;
using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

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
    Animator animator;
    public GameObject text_EnterRoom; 
    string userName;
    Vector3 dir;
    float lerpSpeed = 10;
    public bool islandSelected;
    float Rotate_Pitch;
    float Rotate_Yaw;
    Vector3 receivePos;
    Quaternion receiveRot;
    string temp_userIsland_ID;
    public AudioSource audio;
    public AudioClip clip;
    public string my_Nickname;
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

    void Start()
    {
        if (!photonView.IsMine)
        {
            CVCam.SetActive(false);
        }
        else
        {
            //만약 포톤뷰가 있는 개체라면 콜라이더를 일시적으로 끄자
            StartCoroutine(TurnCollider());
        }
        Cursor.visible = false;
        OrcaObj.SetActive(true);
        text_EnterRoom.GetComponentInChildren<TMP_Text>();
        text_EnterRoom.SetActive(false);
        animator = transform.GetComponentInChildren<Animator>();
        my_Nickname = photonView.Owner.NickName;
        
    }

    [System.Obsolete]
    void Update()
    {

        if (photonView.IsMine)
        {
            #region 플레이어 입력기     
            float Input_Forward = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
            float Input_Rotate_Yaw = Input.GetAxis("Mouse X");
            float Input_Rotate_Pitch = Input.GetAxis("Mouse Y");
            Rotate_Pitch -= Input_Rotate_Pitch * rotateSpeed * Time.deltaTime;
            Rotate_Yaw += Input_Rotate_Yaw * rotateSpeed * Time.deltaTime;
            #endregion
            // 일단 이동방향은 앞뒤로 갈 수 있도록 만든다. 
            //쉬프트키를 눌렀을 때 대쉬되도록 만든다.
            float totalSpeed;


                dir = (transform.forward).normalized;


                if (Input.GetKey(KeyCode.W))
                {
                    userSpeed += (Input_Forward) * accMultipiler * Time.deltaTime;
                }
                else
                {
                    userSpeed -= accMultipiler * 2 * Time.deltaTime;
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

            if (Input.GetKeyDown(KeyCode.I))
            {
                MouseVisual(!Cursor.visible);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (islandSelected)
                {
                    OnClickEnterBtn();
                }
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (photonView.IsMine)
            {
                DoHappy();
                Do_Shout();
                StopAllCoroutines();
                StartCoroutine(ChangeFOV(80));
            }
        }
        if (other.gameObject.CompareTag("UserIsland"))
        {
            text_EnterRoom.SetActive(true);
            userName = other.gameObject.GetComponent<Island_Profile>().user_name;
            temp_userIsland_ID = other.gameObject.GetComponent<Island_Profile>().user_IslandID;
            islandSelected = true;
            //MouseVisual(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (photonView.IsMine)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeFOV(60));
        }
        if (photonView.IsMine && other.gameObject.CompareTag("UserIsland"))
        {
            text_EnterRoom.SetActive(false);
            islandSelected = false;
            //MouseVisual(false);
        }

    }
    void DoHappy()
    {
            photonView.RPC("RPCDoHappy", RpcTarget.All);  
    }
    [PunRPC]
    void RPCDoHappy()
    {
        animator.SetTrigger("isHappy");
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
        audio.PlayOneShot(clip);
    }
    IEnumerator ChangeFOV(float setFOV)
    {
        float fov = CVCam.transform.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView;
        while (true)
        {
            fov = Mathf.Lerp(fov, setFOV, 2 * Time.deltaTime);
            CVCam.transform.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = fov;
            if (fov.AlmostEquals(setFOV,0.1f))
            {
                break;
            }
            yield return null;
        }
    }
    public void OnClickEnterBtn()
    {
        //유저월드 진입 전에 섬 ID 정보 저장
        PlayerPrefs.SetString("User_Island_ID", temp_userIsland_ID);
        CHAN_GameManager.instance.Go_User_Scene(userName);
    }
    void MouseVisual(bool b)
    {
        Cursor.visible = b;
    }
    IEnumerator TurnCollider()
    {
        gameObject.GetComponent<SphereCollider>().enabled = false;
        gameObject.GetComponentInChildren<AudioSource>().enabled = false;
        float time=0;
        while (time < 1)
        {
            time += Time.deltaTime;
            yield return null;
        }
        gameObject.GetComponent<SphereCollider>().enabled = true;
        gameObject.GetComponentInChildren<AudioSource>().enabled = true;
    }
}
