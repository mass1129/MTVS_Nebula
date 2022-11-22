using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using User_Info;
using IslandInfo;

public class CHAN_UserManager : MonoBehaviour
{

    void Start()
    {
        Load();
    }

    // Update is called once per frame
    void Update()
    {

    }
    async void Load()
    {

        var url = "http://ec2-43-201-55-120.ap-northeast-2.compute.amazonaws.com:8001/skyisland";
        var httpRequest = new HttpRequester(new JsonSerializationOption());
        Root result = await httpRequest.Get<Root>(url);
        // 받아온 정보를 토대로 친구리스트에 저장하자 .
        foreach (Result item in result.results.Values)
        {
            CHAN_Users_Info.userLists.Add(item.avatarName);
        }
        Debug.LogWarning("친구리스트 저장 완료");
        string Name = null;
        foreach (string name in CHAN_Users_Info.userLists)
        {
            Name += name;
        }
        Debug.LogWarning("저장된 리스트: "+Name);

    }
}
