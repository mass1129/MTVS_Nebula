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
    //아이템 UI이미지 프리펩
    public Sprite uiDisplay;
    //아이템 타입
    public ItemType type;
    //아이템 설명
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
