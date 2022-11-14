using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Temp_NewSky : MonoBehaviour
{
    public Button go_Sky;
    void Start()
    {
        go_Sky.onClick.AddListener(Go);
    }

    // Update is called once per frame
    void Go()
    {
        SceneManager.LoadScene(3);
    }
}
