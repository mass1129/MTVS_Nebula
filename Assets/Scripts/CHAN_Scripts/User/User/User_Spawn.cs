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
        transform.position = Island_Information.instance.Island_Dic[PlayerPrefs.GetString("User_Island_ID")].island_Pos;
    }


}
