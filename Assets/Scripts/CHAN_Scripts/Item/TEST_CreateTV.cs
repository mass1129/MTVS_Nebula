using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PN=Photon.Pun.PhotonNetwork;

public class TEST_CreateTV : MonoBehaviourPun
{


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            PN.Instantiate("TV 1", new Vector3(51, 0, 47), Quaternion.identity);
        }
    }
}
