using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager_2D : MonoBehaviour
{
    // Start is called before the first frame update
    PlanetInfo info = new PlanetInfo();
    public Material PlanetColor;
    public GameObject prefab;
    void Start()
    {

        info.eachSpeed = new float[info.PlanetCount];
        //count만큼 행성을 생성한다.
        for (int i = 0; i < info.PlanetCount; i++)
        {
            GameObject p = Instantiate(Resources.Load<GameObject>("CHAN_Resources/planet" ));
            p.transform.localScale = p.transform.localScale * Random.Range(info.minScale, info.maxScale);
            info.planets.Add(p);
            Vector2 randomPos = Random.insideUnitCircle;
            p.transform.position = transform.position + new Vector3(randomPos.x * info.radius, 0, randomPos.y * info.radius);
            Material _material = p.GetComponent<Renderer>().material;
            _material.SetColor("_EmissionColor", PlanetColor.GetColor("_EmissionColor"));


            info.eachSpeed[i] = Time.deltaTime * Random.Range(0.2f, info.rotateSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < info.planets.Count; i++)
        {
            info.planets[i].transform.RotateAround(transform.position, transform.up, info.eachSpeed[i]);
        }
    }
}
