using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PN = Photon.Pun.PhotonNetwork;
using TMPro;
public class WhaleManager : MonoBehaviourPun
{
    // 여기는 고래 생성을 위한 매니저 
    // 플레이어가 지정된 범위 내에서 울면 카운트가 증가하고 일정 횟수 달성하면 고래를 발생시킨다. 
    float count;
    bool IsCreated;
    GameObject Whale;
    public float amptitude;
    public float frequency;
    float y;
    float dTime;
    public TMP_Text text_Narration;
    public ParticleSystem bubble;
    public ParticleSystem Light;
    [Header("여기는 내레이션 대사 모음")]
    public string enterAera;
    public string almostDone;
    public string createdWhale;
    bool turn;
    // Update is called once per frame
    private void Start()
    {
        text_Narration.enabled = false;
        bubble.Pause();
        Light.Pause();
    }
    void Update()
    {
        JustMoving();
    }

    private void OnTriggerStay(Collider other)
    {
        // 만약 들어온자가 플레이어이면
        if (other.gameObject.CompareTag("Player"))
        {
            //여기서 플레이어가 스페이스바를 누른다면 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 누른사람이 플레이어 자신이라면 
                if (photonView.IsMine && !IsCreated)
                {
                    count++;
                    Debug.Log("카운트: " + count);
                    // 여기서 카운트가 조건에 도달한 경우 고래 생성 시작
                    if (count == 3)
                        StartCoroutine(Narrate(almostDone));
                    if (count == 5 && photonView.IsMine)
                        CreateWhale();
                }
                else if (photonView.IsMine && IsCreated)
                {
                    Debug.Log("고래 이미 생성됨");
                }
            }
        }
    }
    void CreateWhale()
    {
        Debug.Log("고래 생성");
        AnnounceOthers(true);
        StartCoroutine(Narrate(createdWhale));
        Doeffect();
        if (turn)
        { 
            StartCoroutine(create());
        }

    }
    void Doeffect()
    { photonView.RPC("RPCDoeffect", RpcTarget.All); }
    [PunRPC]
    void RPCDoeffect()
    {
        StartCoroutine(DoEffect());
    }
    IEnumerator DoEffect()
    {
        bubble.Play();
        yield return new WaitForSeconds(5);
        bubble.Stop();
        Light.Play();
        yield return new WaitForSeconds(3);
        Light.Stop();
        if (photonView.IsMine)
        {
            Whale = PN.Instantiate("Player_Whale2", Vector3.up * 60, Quaternion.identity);
            StartCoroutine(Timer());
        }
        
    }
    IEnumerator create()
    {
        yield return null;

    }
    IEnumerator Timer()
    {
        Debug.Log("타이머 작동");
        // 시간 셋팅
        float SetTime =100;
        // 시간이 다 지날때 까지 반복
        while (SetTime > 0)
        {
            SetTime -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("타이머 종료");
        //타이머가 다되면 고래를 파괴한다. 
        AnnounceOthers(false);
        Debug.Log("고래파괴");
        DestroyWhale();
        turn = false;
    }
    IEnumerator Narrate(string ment)
    {
        float SetTime = 3;
        float curTime = 0;
        //3초동안 자막 킨다.
        while (curTime < SetTime)
        {
            text_Narration.enabled = true;
            text_Narration.text = ment;
            curTime += Time.deltaTime;
            yield return null;
        }
        //끝나면 자막 종료
        text_Narration.enabled = false;

    }

    public void DestroyWhale()
    {
        photonView.RPC("RPCDestroyWhale", RpcTarget.All);
    }
    [PunRPC]
    void RPCDestroyWhale()
    {
        //사라지는 애니메이션 넣자 
        Destroy(Whale);
        // 모든 인원에게 카운트를 초기화 시키자
        count = 0;
    }

    void AnnounceOthers(bool b)
    {
        photonView.RPC("RPCAnnounceOthers", RpcTarget.All,b);
        Debug.Log("고래생성 상태 : " + b);
    }
    [PunRPC]
    void RPCAnnounceOthers(bool b)
    {
        IsCreated = b;
        StartCoroutine(Narrate(createdWhale));
    }
    void JustMoving()
    {
        //그냥 위아래로 움직인다. 
        dTime += Time.deltaTime * frequency;
        y = amptitude * Mathf.Sin(dTime);
        transform.position = new Vector3(0, y, 0);
    }
}
