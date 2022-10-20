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
    //아이템 UI이미지 프리펩
    public GameObject prefab;
    //아이템 타입
    public ItemType type;
    //아이템 설명
    [TextArea(15, 20)]
    public string description;



}
