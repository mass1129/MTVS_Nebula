using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInfo :MonoBehaviour
{
    public int PlanetCount=100;
    public float radius=100;
    [HideInInspector]public float[] eachSpeed;
    public float rotateSpeed=1;
    public float minScale=0.2f;
    public float maxScale=3;

    public List<GameObject> planets = new List<GameObject>();
}
