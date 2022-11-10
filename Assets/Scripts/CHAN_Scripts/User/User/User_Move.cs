using Photon.Pun;
using UnityEngine;

/// <summary>
/// 유저의 기본 이동 입력기
/// 커맨드 값 : WASD , Shift
/// </summary>

public class User_Move : MonoBehaviourPun,IPunObservable
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public float userSpeed;
    public float speedMultiplier;


    void Start()
    {
        
    }

    
    Vector3 dir;
    float lerpSpeed = 10;
    public bool islandSelected;

    Vector3 receivePos;

    void Update()
    {

        if (photonView.IsMine)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");


            dir = (/*(x* transform.right) +*/ (y * transform.forward)).normalized;
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
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
        }
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else if (stream.IsReading)
        {
            receivePos=(Vector3)stream.ReceiveNext();
        }
    }
}
