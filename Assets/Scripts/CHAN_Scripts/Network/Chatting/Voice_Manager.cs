using Photon.Pun;
using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Voice_Manager : MonoBehaviour
{
    private PhotonVoiceView photonVoiceView;
    public Image speakerImage;
    // 부모 컴포넌트의 포톤 아이디 
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
        
        if (!myPhotonView.IsMine)
        {
            this.speakerImage.enabled = this.photonVoiceView.IsSpeaking;
        }
    }
}
