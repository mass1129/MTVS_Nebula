using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInfo :MonoBehaviour
{
    public int PlanetCount=400;
    public float radius=250;
    [HideInInspector]public float[] eachSpeed;
    public float rotateSpeed=1.5f;
    public float minScale=0.5f;
    public float maxScale=10;

    public List<GameObject> planets = new List<GameObject>();
}
