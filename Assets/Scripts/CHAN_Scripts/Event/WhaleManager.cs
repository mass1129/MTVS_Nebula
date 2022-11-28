using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PN = Photon.Pun.PhotonNetwork;
using System;

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
    // Update is called once per frame
    void Update()
    {
        //�׳� ���Ʒ��� �����δ�. 
        dTime += Time.deltaTime * frequency;
        y = amptitude * Mathf.Sin(dTime);
        transform.position = new Vector3(0, y, 0);
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
                    if(count==5)
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
        Whale=PN.Instantiate("Player_Whale2", Vector3.up*20, Quaternion.identity);
        // ���⿡ ��Ÿ �ִϸ��̼�, �����̼� �־ ������ ��
        // �ٸ� �÷��̾�� ���� �����ƴٰ� �뺸�Ѵ�. 
        AnnounceOthers(true);
        StartCoroutine(Timer());
        //Ÿ�̸Ӹ� �۵���Ų��. 
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
        
    }
}
