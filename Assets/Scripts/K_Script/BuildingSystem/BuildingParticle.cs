using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingParticle : MonoBehaviour
{
    public float destoryTime;
    void Start()
    {
        Destroy(gameObject, destoryTime);
    }


}
