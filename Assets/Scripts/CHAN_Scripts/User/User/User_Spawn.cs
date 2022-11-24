using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Spawn : MonoBehaviour
{
    // ���� �������ִ� ���� ID������ �����ϴ� ���� 
    public static string curIsland_ID;
    // �÷��̾ �ϴ� ������ �̵��Ҷ� �ҷ����� �Լ�
    public float distance;
    public void Spawn()
    {
        // ���� ���� ��ġ ��ǥ�� ���Ѵ�.
        // ������ ���� ��ġ ��ǥ�� ������ ���Ѵ�. 
        // ���� ���� ��ġ ���� ���� ���� ����� Ư�� �Ÿ��� ���Ѱ����� �÷��̾ ����
        Vector3 curIslandPosition = Island_Information.instance.Island_Dic[PlayerPrefs.GetString("User_Island_ID")].island_Pos;
        Vector3 dir = (Vector3.zero-curIslandPosition).normalized;
        transform.position = curIslandPosition + (dir * distance);
    }


}
