using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoMove : MonoBehaviour
{
    public float amptitude;
    public float frequency;
    float y;
    float dTime;
    void Start()
    {
        
    }
    void Update()
    {
        dTime += Time.deltaTime * frequency;
        y = amptitude * Mathf.Sin(dTime);
        transform.localPosition = new Vector2(-284.46f, y);
    }
}
