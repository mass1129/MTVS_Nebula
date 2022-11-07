using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Rotate : MonoBehaviourPun,IPunObservable
{
    float rx;
    public float ry;
    public float rotateSpeed;

    Quaternion receiveRot;
    float lerpSpeed = 10;


    void Start()
    {
        
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if (photonView.IsMine)
        {
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");
            rx -= my * rotateSpeed * Time.deltaTime;
            ry += mx * rotateSpeed * Time.deltaTime;
            transform.localRotation = Quaternion.EulerAngles(rx, ry, 0);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        }
        

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.rotation);
        }
        else if (stream.IsReading)
        {
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }

}
