using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandInfo :MonoBehaviour
{
    public int PlanetCount=150;
    public float radius=300;
    [HideInInspector]public float[] eachSpeed;
    public float rotateSpeed=1.5f;
    public float minScale=3f;
    public float maxScale=20;

    public List<GameObject> planets = new List<GameObject>();
}
