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
    float Rotate_Pitch;
    float Rotate_Yaw;
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

    //Ư��ȿ��
    //�����鳢�� ������ ������ ������ �̵��ӵ��� �����ϰ� ��.

    [System.Obsolete]
    void Update()
    {
        #region �÷��̾� �Է±�
        float Input_Forward = Mathf.Clamp(Input.GetAxis("Vertical"),0,1);
        float Input_Rotate_Yaw = Input.GetAxis("Mouse X");
        float Input_Rotate_Pitch = Input.GetAxis("Mouse Y");
        #endregion

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
}
