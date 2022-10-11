using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //�Ҵ���� ��ǥ�������� �� �ݰ����� �����ǵ��� �Ѵ�. 
    // �Ӽ�=> �༺ ����
    // �� ī�װ����� ���� �ٸ��� �Ѵ�. 
    PlanetInfo info = new PlanetInfo();
    public Material PlanetColor;
    void Start()
    {
        info.eachSpeed = new float[info.PlanetCount];
        //count��ŭ �༺�� �����Ѵ�.
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
    //�����ϴ� ȿ���� �߰�����S
    void Update()
    {
        for (int i = 0; i < info.planets.Count; i++)
        {
            info.planets[i].transform.RotateAround(transform.position, transform.up, info.eachSpeed[i]);
        }
    }
}
