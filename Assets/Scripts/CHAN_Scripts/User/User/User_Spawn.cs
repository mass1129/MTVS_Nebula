using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Spawn : MonoBehaviour
{
    // ���� �������ִ� ���� ID������ �����ϴ� ���� 
    public static string curIsland_ID;
    // �÷��̾ �ϴ� ������ �̵��Ҷ� �ҷ����� �Լ�
    public void Spawn()
    {
        transform.position = Island_Information.instance.Island_Dic[PlayerPrefs.GetString("Cur_Island")].island_Pos;
    }
    public void SetIslandID(string islandID)
    {
        PlayerPrefs.SetString("Cur_Island", islandID);
    }

}
