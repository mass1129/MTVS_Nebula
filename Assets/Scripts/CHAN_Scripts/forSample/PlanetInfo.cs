using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetInfo :MonoBehaviour
{
    public int PlanetCount=400;
    // 기준 반경을 설정
    public float radius=250;
    [HideInInspector]public float[] eachSpeed;
    public float rotateSpeed=2.5f;
    public float minScale=0.5f;
    public float maxScale=10;

    public List<GameObject> planets = new List<GameObject>();

    // 반경값 제어 함수부
    public void ControlRadius()
    {
        // 슬라이더 초기 위치는 중간
        // 스크롤바가 왼쪽으로 가면 반경 감소 
        // 스크롤바가 오른쪽으로 가면 반경 증가 
    }
    // 행성 갯수 제어 함수부
    public void ControlNOP()
    {

    }
    //행성 크기 제어 함수부
    public void ControlScale()
    {

    }
    //행성 공전속도 제어 함수부
    public void ControlSpeed()
    {

    }
}
