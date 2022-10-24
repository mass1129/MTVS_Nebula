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
    public int Id;
    //������ UI�̹��� ������
    public Sprite uiDisplay;
    //������ Ÿ��
    public ItemType type;
    //������ ����
    [TextArea(15, 20)]
    public string description;
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id;
    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.Id;
    }
}
