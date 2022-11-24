using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_UserWorldMgr : MonoBehaviour
{
    // Start is called before the first frame update
    bool isBuildingLoaded = false;
    void Start()
    {   

        CHAN_ClientManager.instance.myCharacter.SetActiveObj();
        CHAN_ClientManager.instance.myCharacter.GetComponent<>


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
