[System.Serializable]
public class Item
{
    
    public int uniqueId = -1;
    public ItemBuff[] buffs;
    public int id = -1;
    public string name;
    
   
    public Item()
    {
        name = "";
        id = -1;
        uniqueId = -1;
    }
    //new Item(ItemObject)�Ҷ� ItemŬ������ �����ϴ� �κ�
    public Item(ItemObject item)
    {
        //�����ۿ�����Ʈ�� �̸�(�̰� ����Ƽ�󿡼� �������� �̸�)�� Id�� ItemŬ������ �̸��� id�� ����
        name = item.name;
        id = item.data.id;
       
    }
}