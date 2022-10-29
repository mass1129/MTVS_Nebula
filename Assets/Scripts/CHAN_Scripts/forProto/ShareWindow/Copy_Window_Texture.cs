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
        
        isOn = true;
    }
    void Copying()
    {
        texture = new Texture[Window.childCount];
        for (int i = 0; i < Window.childCount; i++)
        {
            if (copys.Count < Window.childCount)
            {
                GameObject obj = Instantiate(btn_Copy, transform);
                copys.Add(obj);
            }
            else if (copys.Count > Window.childCount)
            {
                Destroy(copys[i].gameObject);
                copys.Remove(copys[i].gameObject);
            }
            copys[i].GetComponent<RawImage>().texture = Window.GetChild(i).gameObject.GetComponent<Renderer>().material.mainTexture;
        }
    }
    void ClearList()
    {
        if (copys.Count != 0)
        {
            for (int i = 0; i < copys.Count; i++)
            {
                Destroy(copys[i].gameObject);
            }
        }
            copys.Clear();
    }
    
}
