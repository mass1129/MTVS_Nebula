using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �̴ϸʻ� �÷��̾�, �ϴü��������� �̴ϸʻ����� ǥ�õǵ��� �����Ѵ�.
/// </summary>
public class Minimap_IconManager : MonoBehaviour
{
    public GameObject playerIcon;
    public Image IslandIcon;

    Transform player;
    void Start()
    {
        //�����Ҷ� �ϴü��� ������Ʈ, �÷��̾��� ������Ʈ�� ���´�. 
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    //[System.Obsolete]
    //void Update()
    //{
    //    playerIcon.transform.position = player.position;
    //    playerIcon.transform.localRotation = Quaternion.EulerAngles(0,User_Rotate.instance.ry,0);
    //}
}
