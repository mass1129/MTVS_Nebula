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
//������ �ɷ�ġ
public enum Attributes
{
    Popularity,
    Kindness,
    Wealth,
    Exploration

}
//�ܼ��� �������� ������ �����ϴ� class
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
    //new Item(ItemObject)�Ҷ� ItemŬ������ �����ϴ� �κ�
    public Item(ItemObject item)
    {   
        //�����ۿ�����Ʈ�� �̸�(�̰� ����Ƽ�󿡼� �������� �̸�)�� Id�� ItemŬ������ �̸��� id�� ����
        Name = item.name;
        Id = item.Id;
        //������ ������Ʈ�� �ִ� ��������Ʈ�� itemŬ������ ��ġ
        buffs = new ItemBuff[item.buffs.Length];
        for(int i = 0; i < buffs.Length; i++)
        {   
            
            //������ ��������Ʈ�� �ִ� �׸� ���� ���� ���� ���� �׸� ����
            buffs[i]= new ItemBuff(item.buffs[i].min, item.buffs[i].max)
            {   
                //����Ÿ�� ����
                attribute = item.buffs[i].attribute
            };
        }
    }
}

[System.Serializable]
public class ItemBuff
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
}
