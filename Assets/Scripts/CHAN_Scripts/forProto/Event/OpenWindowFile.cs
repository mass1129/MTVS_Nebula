using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OpenWindowFile : MonoBehaviour
{
    string[] paths;
    public RawImage image;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    [System.Obsolete]
    public void OnBtn()
    {
        paths=StandaloneFileBrowser.OpenFilePanel("Open File", "", "png", true);
        StartCoroutine(GetTexture());
    }

    [System.Obsolete]
    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + paths[0]);

        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            image.texture = myTexture;
        }
    }
    // 입력값 출력
    //public void WriteResult(string[] paths)
    //{
    //    if (paths.Length == 0)
    //    {
    //        return;
    //    }

    //    _path = "";
    //    foreach (var p in paths)
    //    {
    //        _path += p + "\n";
    //    }
    //}
}
