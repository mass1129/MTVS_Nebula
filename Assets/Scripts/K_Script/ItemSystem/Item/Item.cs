[System.Serializable]
public class Item
{
    public string Name;
    public int Id = -1;
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