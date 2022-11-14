using Photon.Pun;
using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

/// <summary>
/// ������ �⺻ �̵� �Է±�
/// Ŀ�ǵ� �� : WASD , Shift
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
    public GameObject btn_EnterRoom; 
    string userName;
    Vector3 dir;
    float lerpSpeed = 10;
    public bool islandSelected;
    float Rotate_Pitch;
    float Rotate_Yaw;
    Vector3 receivePos;
    Quaternion receiveRot;
    bool mouseOn;
    string temp_userIsland_ID;
    public AudioSource audio;
    public AudioClip clip;
    //W: ����
    //S: ���� 
    //A: �ݽð���� ȸ�� 
    //D: �ð���� ȸ��
    //MouseUp : �� ���� ��ȯ
    //MouseDown : �� �Ʒ��� ��ȯ
    //Shift: �뽬
    //Space bar : �� �����Ҹ�

    //Ư��ȿ��
    //�����鳢�� ������ ������ ������ �̵��ӵ��� �����ϰ� ��.

    void Start()
    {
        if (!photonView.IsMine)
        {
            CVCam.SetActive(false);
        }
        Cursor.visible = false;
        OrcaObj.SetActive(true);
        btn_EnterRoom.GetComponentInChildren<Button>().onClick.AddListener(OnClickEnterBtn);
        btn_EnterRoom.SetActive(false);
        animator = transform.GetComponentInChildren<Animator>();
        
    }

    [System.Obsolete]
    void Update()
    {
        if (photonView.IsMine)
        {
            #region �÷��̾� �Է±�     
            float Input_Forward = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
            float Input_Rotate_Yaw = Input.GetAxis("Mouse X");
            float Input_Rotate_Pitch = Input.GetAxis("Mouse Y");
            #endregion
            // �ϴ� �̵������� �յڷ� �� �� �ֵ��� �����. 
                //����ƮŰ�� ������ �� �뽬�ǵ��� �����.
                float totalSpeed;
            if (!mouseOn)
            {
                dir = (transform.forward).normalized;
                Rotate_Pitch -= Input_Rotate_Pitch * rotateSpeed * Time.deltaTime;
                Rotate_Yaw += Input_Rotate_Yaw * rotateSpeed * Time.deltaTime;

                if (Input.GetKey(KeyCode.W))
                {
                    userSpeed += (Input_Forward) * accMultipiler * Time.deltaTime;
                }
                else
                {
                    userSpeed -= accMultipiler * 2 * Time.deltaTime;
                }
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
        if (Input.GetKeyDown(KeyCode.I))
        {
            MouseVisual(!Cursor.visible);
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
            btn_EnterRoom.SetActive(true);
            userName = other.gameObject.GetComponent<Island_Profile>().user_name;
            temp_userIsland_ID = other.gameObject.GetComponent<Island_Profile>().user_IslandID;
            MouseVisual(true);
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
            btn_EnterRoom.SetActive(false);
            MouseVisual(false);
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
        PlayerPrefs.SetString("User_Island_ID", temp_userIsland_ID);
        print("�� ID: "+PlayerPrefs.GetString("Island_ID"));
        CHAN_GameManager.instance.Go_User_Scene(userName);
    }
    void MouseVisual(bool b)
    {
        Cursor.visible = b;
        mouseOn = b;
    }
}
