using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/item")]
//단순히 아이템의 정보를 저장하는 class
public class ItemObject : ScriptableObject
{

    //아이템 UI이미지 프리펩
    public Sprite uiDisplay;
    public int charCustomIndex=-1;
    public bool stackable;
    //아이템 타입
    public ItemType type;
    //아이템 설명
    [TextArea(15, 20)]
    public string description;
    public Item data = new Item();

    public Item CreateItem()
    {   
        Item newItem = new Item(this);
        return newItem;
    }
    
    
}



