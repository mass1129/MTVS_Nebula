using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    BuildObject,
    Vehicle,
    Title,
    Default,
    Hair,
    Beard,
    Accessory,
    Hat,
    Shirt,
    Pants,
    Shoes,
    Bag,
    Weapons
}
//������ �ɷ�ġ
public enum Attributes
{
    Popularity,
    Kindness,
    Wealth,
    Exploration,
    Decoration_char,
    Decoration_House

}
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/item")]
//�ܼ��� �������� ������ �����ϴ� class
public class ItemObject : ScriptableObject
{

    //������ UI�̹��� ������
    public Sprite uiDisplay;
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

[System.Serializable]
public class Item
{
    public string Name;
    public int Id =-1;
    public ItemBuff[] buffs;
    public Item()
    {
        Name = "";
        Id = -1;
    }
    //new Item(ItemObject)�Ҷ� ItemŬ������ �����ϴ� �κ�
    public Item(ItemObject item)
    {   
        //�����ۿ�����Ʈ�� �̸�(�̰� ����Ƽ�󿡼� �������� �̸�)�� Id�� ItemŬ������ �̸��� id�� ����
        Name = item.name;
        Id = item.data.Id;
        //������ ������Ʈ�� �ִ� ��������Ʈ�� itemŬ������ ��ġ
        buffs = new ItemBuff[item.data.buffs.Length];
        for(int i = 0; i < buffs.Length; i++)
        {   
            
            //������ ��������Ʈ�� �ִ� �׸� ���� ���� ���� ���� �׸� ����
            buffs[i]= new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max)
            {   
                //����Ÿ�� ����
                attribute = item.data.buffs[i].attribute
            };
        }
    }
}

[System.Serializable]
public class ItemBuff : IModifiers
{   
    //���� Ÿ��
    public Attributes attribute;
    //���� ��, �ּ� �ִ밪 ������ �������� value�� �Ҵ�
    public int value;
    public int min;
    public int max;
    //new ItemBuff()�� itembuff����
    public ItemBuff(int _min, int _max)
    {   
        //�ִ� �ּҰ��� �޾Ƽ�
        min = _min;
        max = _max;
        //�������� value�� �������� �Ҵ�.
        GenerateValue();
    }
    public void GenerateValue()
    {   
        //�������� value�� �������� �Ҵ�.
        value = UnityEngine.Random.Range(min, max);
    }

    public void AddValue(ref int baseValue)
    {
        baseValue += value;
    }
}
