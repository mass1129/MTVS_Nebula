using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PN = Photon.Pun.PhotonNetwork;
using TMPro;
public class WhaleManager : MonoBehaviourPun
{
    // ����� �� ������ ���� �Ŵ��� 
    // �÷��̾ ������ ���� ������ ��� ī��Ʈ�� �����ϰ� ���� Ƚ�� �޼��ϸ� ���� �߻���Ų��. 
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
    [Header("����� �����̼� ��� ����")]
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
        // ���� �����ڰ� �÷��̾��̸�
        if (other.gameObject.CompareTag("Player"))
        {
            //���⼭ �÷��̾ �����̽��ٸ� �����ٸ� 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // ��������� �÷��̾� �ڽ��̶�� 
                if (photonView.IsMine && !IsCreated)
                {
                    count++;
                    Debug.Log("ī��Ʈ: " + count);
                    // ���⼭ ī��Ʈ�� ���ǿ� ������ ��� �� ���� ����
                    if (count == 3)
                        StartCoroutine(Narrate(almostDone));
                    if (count == 5 && photonView.IsMine)
                        CreateWhale();
                }
                else if (photonView.IsMine && IsCreated)
                {
                    Debug.Log("�� �̹� ������");
                }
            }
        }
    }
    void CreateWhale()
    {
        Debug.Log("�� ����");
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
        Debug.Log("Ÿ�̸� �۵�");
        // �ð� ����
        float SetTime =100;
        // �ð��� �� ������ ���� �ݺ�
        while (SetTime > 0)
        {
            SetTime -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Ÿ�̸� ����");
        //Ÿ�̸Ӱ� �ٵǸ� ���� �ı��Ѵ�. 
        AnnounceOthers(false);
        Debug.Log("���ı�");
        DestroyWhale();
        turn = false;
    }
    IEnumerator Narrate(string ment)
    {
        float SetTime = 3;
        float curTime = 0;
        //3�ʵ��� �ڸ� Ų��.
        while (curTime < SetTime)
        {
            text_Narration.enabled = true;
            text_Narration.text = ment;
            curTime += Time.deltaTime;
            yield return null;
        }
        //������ �ڸ� ����
        text_Narration.enabled = false;

    }

    public void DestroyWhale()
    {
        photonView.RPC("RPCDestroyWhale", RpcTarget.All);
    }
    [PunRPC]
    void RPCDestroyWhale()
    {
        //������� �ִϸ��̼� ���� 
        Destroy(Whale);
        // ��� �ο����� ī��Ʈ�� �ʱ�ȭ ��Ű��
        count = 0;
    }

    void AnnounceOthers(bool b)
    {
        photonView.RPC("RPCAnnounceOthers", RpcTarget.All,b);
        Debug.Log("������ ���� : " + b);
    }
    [PunRPC]
    void RPCAnnounceOthers(bool b)
    {
        IsCreated = b;
        StartCoroutine(Narrate(createdWhale));
    }
    void JustMoving()
    {
        //�׳� ���Ʒ��� �����δ�. 
        dTime += Time.deltaTime * frequency;
        y = amptitude * Mathf.Sin(dTime);
        transform.position = new Vector3(0, y, 0);
    }
}
