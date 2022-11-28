using Photon.Pun;
using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

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
    public GameObject text_EnterRoom; 
    string userName;
    Vector3 dir;
    float lerpSpeed = 10;
    public bool islandSelected;
    bool mouseClicked;
    float Rotate_Pitch;
    float Rotate_Yaw;
    Vector3 receivePos;
    Quaternion receiveRot;
    string temp_userIsland_ID;
    public AudioSource audio;
    public AudioClip clip;
    public string my_Nickname;
    bool turn;
    float curTime;
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
        else
        {
            //���� ����䰡 �ִ� ��ü��� �ݶ��̴��� �Ͻ������� ����
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
            #region �÷��̾� �Է±�     
            if (!mouseClicked)
            {
                float Input_Forward = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);
                float Input_Rotate_Yaw = Input.GetAxis("Mouse X");
                float Input_Rotate_Pitch = Input.GetAxis("Mouse Y");

                Rotate_Pitch -= Input_Rotate_Pitch * rotateSpeed * Time.deltaTime;
                Rotate_Yaw += Input_Rotate_Yaw * rotateSpeed * Time.deltaTime;
                #endregion
                // �ϴ� �̵������� �յڷ� �� �� �ֵ��� �����. 
                //����ƮŰ�� ������ �� �뽬�ǵ��� �����.
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
                if (turn)
                {
                    ReturnControl();
                }
                else
                { 
                    transform.position += dir * totalSpeed * Time.deltaTime;
                }
                transform.localRotation = Quaternion.EulerAngles(Mathf.Clamp(Rotate_Pitch, -70 * Mathf.Deg2Rad, 70 * Mathf.Deg2Rad), Rotate_Yaw, 0);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                MouseVisual(!Cursor.visible);
                mouseClicked = !mouseClicked;
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
            if (photonView.IsMine)
            {
                text_EnterRoom.SetActive(true);
                userName = other.gameObject.GetComponent<Island_Profile>().user_name;
                temp_userIsland_ID = other.gameObject.GetComponent<Island_Profile>().user_IslandID;
                islandSelected = true;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("UserIsland"))
        {
            if (photonView.IsMine)
            {
                CheckDistance(other.transform);
            }
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
        //�������� ���� ���� �� ID ���� ����
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
        while (time < 3)
        {
            time += Time.deltaTime;
            yield return null;
        }
        gameObject.GetComponent<SphereCollider>().enabled = true;
        gameObject.GetComponentInChildren<AudioSource>().enabled = true;
    }
    void CheckDistance(Transform trans)
    {
        // �÷��̾�� ������ �Ÿ�
        float distance = Vector3.Distance(transform.position, trans.position);
        Vector3 dir = (transform.position - trans.position).normalized;
        //Debug.Log("�÷��̾�� �� ���� �Ÿ�: "+distance);
        if (distance <= 35)
        {
            transform.position += dir * 10 * Time.deltaTime;
            turn = true;
        }
        else
        {
            turn = false;
        }

    }
    void ReturnControl()
    {
        curTime += Time.deltaTime;
        if (curTime > 1)
        {
            turn = false;
            curTime = 0;
        }

    }
}
