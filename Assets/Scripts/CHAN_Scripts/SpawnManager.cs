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
    bool turn;
    void Start()
    {
      
    }
    //�����ϴ� ȿ���� �߰�����S
    // ���Ӿ����� ũ��, �ӵ�, �ݰ��� �ٲ� �� �ֵ��� �Ѵ�.
    void Update()
    {
        if (!turn)
        {
            StartCoroutine(initialize());
            turn = true;
        }
        
        for (int i = 0; i < info.planets.Count; i++)
        {
            info.planets[i].transform.RotateAround(transform.position, transform.up, info.eachSpeed[i]);
        }
    }

    IEnumerator initialize()
    {
        info.eachSpeed = new float[info.PlanetCount];
        //count��ŭ �༺�� �����Ѵ�.
        // �����̸� �ຸ��

        for (int i = 0; i < info.PlanetCount; i++)
        {
            yield return new WaitForSeconds(0.2f);
            GameObject p = Instantiate(Resources.Load<GameObject>("CHAN_Resources/planet"), gameObject.transform);
            p.transform.localScale = p.transform.localScale * Random.Range(info.minScale, info.maxScale);
            info.planets.Add(p);
            p.transform.localPosition = Random.insideUnitSphere * info.radius;
            Material _material = p.GetComponent<Renderer>().material;
            _material.SetColor("_EmissionColor", PlanetColor.GetColor("_EmissionColor"));
            info.eachSpeed[i] = Time.deltaTime * Random.Range(0.2f, info.rotateSpeed);

        }
        
    }


}
