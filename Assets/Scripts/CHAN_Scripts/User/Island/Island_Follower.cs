using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Island_Follower : MonoBehaviour
{
    // 섬의 이미지가 유저를 따라가도록 만들것임
    Transform playerPos;
    public float popupMultiplier;
    void Start()
    {
        playerPos = CHAN_PlayerManger.LocalPlayerInstance.transform.GetChild(5).gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponentInParent<Island_Profile>().turn)
        {
            GetComponent<Image>().enabled = false;
            return;
        }  
        ShowImage();
    }
    void ShowImage()
    {
        //transform.forward = playerPos.forward;
        GetComponent<Image>().enabled = true;
        transform.LookAt(playerPos);
    }
    public IEnumerator PopUp()
    {
        float initial = 0;
        transform.localScale *= 0;
        while(transform.localScale.x<=0.97f)
        {
            transform.localScale = new Vector3(initial, initial, initial);
            initial = Mathf.Lerp(initial, 1, Time.deltaTime * popupMultiplier);
            yield return null;
        }
        transform.localScale= new Vector3(1, 1, 1);
        Debug.Log("끝");
    }
}
