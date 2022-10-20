using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Island_Profile : MonoBehaviour
{
    //이미지 파일을 가져와서  섬에 띄우도록 한다.
    float visualDistance =100;
    public string user_name;
    Image profileImage;
    Text userName_Text;
    Transform camPos;
    //Transform player;
    bool turn;
    void Start()
    {
        //player = GameObject.Find("Player").transform;
        profileImage = GetComponent<Image>();
        profileImage.enabled = false;
        camPos = Camera.main.transform;
        userName_Text = gameObject.transform.GetComponentInChildren<Text>();

       

    }

    // Update is called once per frame
    void Update()
    {
        if (!turn)
        { 
            LoadImage();
            turn = true;
        }
        if (visualDistance > GetDistanceToPlayer())
        {
            profileImage.enabled = true;
            userName_Text.enabled = true;
            userName_Text.transform.LookAt(camPos.position);
            profileImage.transform.LookAt(camPos.position);
        }
        else
        {
            profileImage.enabled = false;
            userName_Text.enabled = false;
            //이미지를 끈다.
        }
    }
    float GetDistanceToPlayer()
    {
        float dis;
        dis = Vector3.Distance(transform.position, camPos.position);
        return dis;
    }
    void LoadImage()
    {
        StartCoroutine(GetTexture(IslandInformation.instance.User_image[user_name]));
        //profileImage.sprite = Resources.Load<Sprite>("CHAN_Resources/subset_30/" + IslandInformation.instance.User_image[user_name]);
        userName_Text.text = "UserName_" + user_name;
        userName_Text.enabled = false;
    }
    // 이 코루틴은 한번만 사용되어야 한다. 
    IEnumerator GetTexture(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            profileImage.sprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), Vector2.zero);
            
        }
    }


}
