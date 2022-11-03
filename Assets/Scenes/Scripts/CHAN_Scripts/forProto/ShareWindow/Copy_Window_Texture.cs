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
    public Transform Area_btns;
    public GameObject btn_Copy;
    Texture[] texture;
    float curtime;
    List<GameObject> copys = new List<GameObject>();
    bool isOn;
    public Image image;
    void Start()
    {
        image = GetComponent<Image>();
        image.enabled = false;
        Area_btns.gameObject.SetActive(false);
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
        if (Item_TVManager.instance.isTurn)
        { 
            isOn = b;
        }
        image.enabled = b;
        Area_btns.gameObject.SetActive(b);
        foreach (GameObject go in copys)
        {
            go.SetActive(b);
        }
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
    //버튼을 선택했을 때
    public void Btn_Select()
    {
        OnClicked(false);
    }
    //화면 선택 취소
    public void Btn_CloseWindow()
    {
        OnClicked(false);
        Item_TVManager.instance.isTurn = !Item_TVManager.instance.isTurn;
        Item_TVManager.instance.OnSpaceBar();
    }
}
