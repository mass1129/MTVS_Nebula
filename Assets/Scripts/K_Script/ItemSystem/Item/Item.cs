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
        uniqueId = item.data.uniqueId;
        name = item.name;
        id = item.data.id;
        //������ ������Ʈ�� �ִ� ��������Ʈ�� itemŬ������ ��ġ
        buffs = new ItemBuff[item.data.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {

            //������ ��������Ʈ�� �ִ� �׸� ���� ���� ���� ���� �׸� ����
            buffs[i] = new ItemBuff(item.data.buffs[i].Min, item.data.buffs[i].Max)
            {
                //����Ÿ�� ����
                stat = item.data.buffs[i].stat
            };
        }
       
    }
}