using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudManager : MonoBehaviour
{
    public GameObject[] clouds;
    public Transform cloudPos;
    public float randomRange;
    public int num;
    float x, y,z;
    void Start()
    {
        for (int i = 0; i < num; i++)
        {
            for (int j = 0; j < clouds.Length; j++)
            {
                x = Random.Range(-randomRange, randomRange);
                y = Random.Range(-randomRange, randomRange);
                z= Random.Range(-randomRange, randomRange);
                GameObject obj = Instantiate(clouds[j], cloudPos);
                obj.transform.localScale *= 0.8f;
                obj.SetActive(true);
                obj.transform.position=new Vector3(x, y, z);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
