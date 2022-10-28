using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class K_TestSceneMgr : MonoBehaviour
{   
    public void LoadCostomScene()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadSkyScene()
    {
        SceneManager.LoadScene(4);
    }

}
