using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/item")]
//�ܼ��� �������� ������ �����ϴ� class
public class ItemObject : ScriptableObject
{

    //������ UI�̹��� ������
    public Sprite uiDisplay;
    public int charCustomIndex=-1;
    public bool stackable;
    //������ Ÿ��
    public ItemType type;
    //������ ����
    [TextArea(15, 20)]
    public string description;
    public Item data = new Item();

    public Item CreateItem()
    {   
        Item newItem = new Item(this);
        return newItem;
    }
    
    
}



