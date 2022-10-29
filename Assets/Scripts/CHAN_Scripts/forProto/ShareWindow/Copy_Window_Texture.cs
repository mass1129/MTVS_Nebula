using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uWindowCapture;

public class Copy_Window_Texture : MonoBehaviour
{
    public static Copy_Window_Texture instance;
    private void Awake()
    {
        instance = this;
    }
    public Transform Window;
    public GameObject btn_Copy;
    Texture[] texture;
    float curtime;
    List<GameObject> copys = new List<GameObject>();
    bool isOn;
    Image image;
    void Start()
    {
        image = GetComponent<Image>();
        image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            Copying();
        }
        
    }
    public void OnClicked(bool b)
    {
        
        isOn = b;
        image.enabled = b;
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
            if (copys[i].activeSelf)
            { 
                copys[i].transform.GetChild(0).GetComponent<Btn_window>().SetNum(i);
 
            }
            GameObject o = Window.GetChild(i).gameObject;
            Transform c = copys[i].transform.GetChild(0);
            Text c1 = copys[i].transform.GetChild(1).GetComponent<Text>();
            c.GetComponent<RawImage>().texture = o.GetComponent<Renderer>().material.mainTexture;
            c.localScale = new Vector2(o.transform.localScale.x, o.transform.localScale.y);
            c1.text = "  "+o.GetComponent<UwcWindowTexture>().window.title;
            if (c1.text.Length > 30)
            {
                c1.text.Substring(0, 30);
                c1.text += "...";
            }
                
            

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
