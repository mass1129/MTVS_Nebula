using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Copy_Window_Texture : MonoBehaviour
{

    public Transform Window;
    public GameObject btn_Copy;
    Texture[] texture;
    float curtime;
    List<GameObject> copys = new List<GameObject>();
    bool isOn;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            Copying();
        }
        
    }
    public void OnClicked()
    {
        texture = new Texture[Window.childCount];
        // window�� �ڽ� ���� �ľ� 
        for (int i = 0; i < Window.childCount; i++)
        {
            GameObject obj = Instantiate(btn_Copy,transform);
            texture[i] = obj.GetComponent<RawImage>().texture;
            copys.Add(obj);

        }
        isOn = true;
    }
    void Copying()
    {
        for (int i = 0; i < Window.childCount; i++)
        {
            texture[i]= Window.GetChild(i).gameObject.GetComponent<Renderer>().material.mainTexture;
            copys[i].GetComponent<RawImage>().texture = texture[i];
        }
    }
    public void TurnOffTV()
    { 
        
    }
    
}
