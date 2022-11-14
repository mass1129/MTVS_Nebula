using Photon.Pun;
using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Voice_Manager : MonoBehaviour
{
    private PhotonVoiceView photonVoiceView;
    public Image speakerImage;
    // �θ� ������Ʈ�� ���� ���̵� 
    PhotonView myPhotonView;
    private void Start()
    {
        myPhotonView = transform.GetComponentInParent<PhotonView>();
        photonVoiceView = transform.GetComponentInParent<PhotonVoiceView>();
        speakerImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!myPhotonView.IsMine&&SceneManager.GetActiveScene().name==CHAN_GameManager.instance.name_UserScene)
        {
            this.speakerImage.enabled = this.photonVoiceView.IsSpeaking;
        }
    }
}
