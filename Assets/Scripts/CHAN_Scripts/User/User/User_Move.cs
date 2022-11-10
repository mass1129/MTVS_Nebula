using Photon.Pun;
using UnityEngine;

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
    //W: ����
    //S: ���� 
    //A: �ݽð���� ȸ�� 
    //D: �ð���� ȸ��
    //MouseUp : �� ���� ��ȯ
    //MouseDown : �� �Ʒ��� ��ȯ
    //Shift: �뽬
    //Space bar : �� �����Ҹ�
    float _Input_Forward;
    float _Input_Rotate_Yaw;
    float _Input_Rotate_Pitch;

    #region �÷��̾� �Է±�
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

    //Ư��ȿ��
    //�����鳢�� ������ ������ ������ �̵��ӵ��� �����ϰ� ��.

    [System.Obsolete]
    void Update()
    {

        
        if (photonView.IsMine)
        {
            // �ϴ� �̵������� �յڷ� �� �� �ֵ��� �����. 
            dir = (Input_Forward * transform.forward).normalized;
            Rotate_Pitch -= Input_Rotate_Pitch * rotateSpeed * Time.deltaTime;
            Rotate_Yaw += Input_Rotate_Yaw * rotateSpeed * Time.deltaTime;
            //����ƮŰ�� ������ �� �뽬�ǵ��� �����.
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
