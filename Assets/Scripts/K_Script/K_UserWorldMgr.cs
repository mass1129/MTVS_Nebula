using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_UserWorldMgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   
       if (CHAN_ClientManager.instance.myCharacter.photonView.IsMine)
        CHAN_ClientManager.instance.myCharacter.SetActiveObj();
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
