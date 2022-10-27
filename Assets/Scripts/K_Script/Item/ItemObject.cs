using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    BuildObject,
    Vehicle,
    Equipment,
    Title,
    Default
}
//아이템 능력치
public enum Attributes
{
    Popularity,
    Kindness,
    Wealth,
    Exploration

}
//단순히 아이템의 정보를 저장하는 class
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
    public ItemBuff[] buffs;

    public Item CreateItem()
    {   
        Item newItem = new Item(this);
        return newItem;
    }
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id;
    public ItemBuff[] buffs;
    //new Item(ItemObject)할때 Item클래스를 생성하는 부분
    public Item(ItemObject item)
    {   
        //아이템오브젝트의 이름(이건 유니티상에서 지정해준 이름)과 Id를 Item클래스의 이름과 id에 대입
        Name = item.name;
        Id = item.Id;
        //아이템 오브젝트에 있는 버프리스트을 item클래스에 배치
        buffs = new ItemBuff[item.buffs.Length];
        for(int i = 0; i < buffs.Length; i++)
        {   
            
            //아이템 버프리스트에 있는 항목에 랜덤 값을 가진 버프 항목 생성
            buffs[i]= new ItemBuff(item.buffs[i].min, item.buffs[i].max)
            {   
                //버프타입 생성
                attribute = item.buffs[i].attribute
            };
        }
    }
}

[System.Serializable]
public class ItemBuff
{   
    //버프 타입
    public Attributes attribute;
    //버프 값, 최소 최대값 사이의 랜덤값이 value에 할당
    public int value;
    public int min;
    public int max;
    //new ItemBuff()시 itembuff생성
    public ItemBuff(int _min, int _max)
    {   
        //최대 최소값을 받아서
        min = _min;
        max = _max;
        //아이템의 value에 랜덤값을 할당.
        GenerateValue();
    }
    public void GenerateValue()
    {   
        //아이템의 value에 랜덤값을 할당.
        value = UnityEngine.Random.Range(min, max);
    }
}
