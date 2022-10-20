using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    BuildObject,
    Vehicle,
    Equipment,
    Default
}

public abstract class ItemObject : ScriptableObject
{   
    //������ UI�̹��� ������
    public GameObject prefab;
    //������ Ÿ��
    public ItemType type;
    //������ ����
    [TextArea(15, 20)]
    public string description;



}
