using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHAN_ClientManager : MonoBehaviour
{
    public static CHAN_ClientManager instance;
    public static List<PhotonView> players = new List<PhotonView>();
    public K_01_Character myCharacter;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    public void AddPlayer(PhotonView pv)
    {
        players.Add(pv);
    }
    public void RemovePlayer(PhotonView pv)
    {
        players.Remove(pv);
    }

}
