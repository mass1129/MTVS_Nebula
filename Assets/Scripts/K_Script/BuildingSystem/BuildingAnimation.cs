using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class BuildingAnimation : MonoBehaviourPun {

    [SerializeField] private AnimationCurve animationCurve = null;

    private float time;

    public Transform toRotateObj=null;
    
    public Vector3 toRotateV3;
    public GameObject effect;
    public Transform effectPos;
    public string effectPath;
    private void Update() {
        time += Time.deltaTime;

        transform.localScale = new Vector3(1, animationCurve.Evaluate(time), 1);
        if(toRotateObj != null)
        toRotateObj.eulerAngles = toRotateV3*time;

    }

    public void FactoryEvent()
    {
        photonView.RPC("RPCFactoryEvent", RpcTarget.All);
    }
    [PunRPC]
    public void RPCFactoryEvent()
    {
        GameObject obj = Instantiate(effect, effectPos.position, Quaternion.identity);
        Destroy(obj, 3);
    }

}
