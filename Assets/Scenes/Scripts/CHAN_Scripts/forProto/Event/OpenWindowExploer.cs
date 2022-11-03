using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
//using UnityEngine.UI;

//public class OpenWindowExploer : MonoBehaviour
//{
//    string path;
//    public RawImage image;
    
//    //파일 탐색기로 접근하는 함수 
//    public void OpenFileExplorer()
//    {
//        path = EditorUtility.OpenFilePanel("내 PC", "", "png");
//        StartCoroutine(GetTexture());
//    }
//    //
//    IEnumerator GetTexture()
//    {
//        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///"+path);
        
//        yield return www.SendWebRequest();
//        if (www.isNetworkError || www.isHttpError)
//        {
//            Debug.Log(www.error);
//        }
//        else
//        {
//            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
//            image.texture=myTexture;
//        }
//    }
//}
