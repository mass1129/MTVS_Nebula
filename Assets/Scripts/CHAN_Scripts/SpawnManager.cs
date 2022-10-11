using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //할당받은 좌표기준으로 구 반경으로 스폰되도록 한다. 
    // 속성=> 행성 갯수
    // 각 카테고리마다 색을 다르게 한다. 
    PlanetInfo info = new PlanetInfo();
    public Material PlanetColor;
    void Start()
    {
        info.eachSpeed = new float[info.PlanetCount];
        //count만큼 행성을 생성한다.
        for (int i = 0; i < info.PlanetCount; i++)
        {
            GameObject p = Instantiate(Resources.Load<GameObject>("CHAN_Resources/planet"));
            p.transform.localScale =p.transform.localScale*Random.Range(info.minScale, info.maxScale);
            info.planets.Add(p);
            p.transform.position = transform.position + Random.insideUnitSphere * info.radius;
            Material _material = p.GetComponent<Renderer>().material;
            _material.SetColor("_EmissionColor", PlanetColor.GetColor("_EmissionColor"));
            info.eachSpeed[i] = Time.deltaTime * Random.Range(0.2f, info.rotateSpeed);
        }
    }
    //공전하는 효과를 추가하자S
    void Update()
    {
        for (int i = 0; i < info.planets.Count; i++)
        {
            info.planets[i].transform.RotateAround(transform.position, transform.up, info.eachSpeed[i]);
        }
    }
}
