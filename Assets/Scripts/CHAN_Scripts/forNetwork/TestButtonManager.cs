using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestButtonManager : MonoBehaviour
{
    public Button btn_Switch;
    public GameObject image1;
    public GameObject image2;

    void Start()
    {
        btn_Switch.onClick.AddListener(switchObjs);
    }
   
    public void switchObjs()
    {
        image1.SetActive(!image1.activeSelf);
        image2.SetActive(!image2.activeSelf);

    }
}
