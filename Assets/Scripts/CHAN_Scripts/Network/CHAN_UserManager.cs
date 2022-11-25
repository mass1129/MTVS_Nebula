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

        var url = "https://resource.mtvs-nebula.com/skyisland";
        var httpRequest = new HttpRequester(new JsonSerializationOption());
        Root result = await httpRequest.Get<Root>(url);
        // �޾ƿ� ������ ���� ģ������Ʈ�� �������� .
        foreach (Result item in result.results.Values)
        {
            CHAN_Users_Info.userLists.Add(item.avatarName);
        }
        Debug.LogWarning("ģ������Ʈ ���� �Ϸ�");
        string Name = null;
        foreach (string name in CHAN_Users_Info.userLists)
        {
            Name += name;
        }
        Debug.LogWarning("����� ����Ʈ: "+Name);

    }
}
