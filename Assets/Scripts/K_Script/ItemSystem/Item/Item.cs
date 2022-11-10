[System.Serializable]
public class Item
{
    public string name;
    public int Id = -1;
    public int uniqueId = -1;
    public ItemBuff[] buffs;
    public Item()
    {
        name = "";
        Id = -1;
        uniqueId = -1;
    }
    //new Item(ItemObject)할때 Item클래스를 생성하는 부분
    public Item(ItemObject item)
    {
        //아이템오브젝트의 이름(이건 유니티상에서 지정해준 이름)과 Id를 Item클래스의 이름과 id에 대입
        name = item.name;
        Id = item.data.Id;
        //아이템 오브젝트에 있는 버프리스트을 item클래스에 배치
        buffs = new ItemBuff[item.data.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {

            //아이템 버프리스트에 있는 항목에 랜덤 값을 가진 버프 항목 생성
            buffs[i] = new ItemBuff(item.data.buffs[i].Min, item.data.buffs[i].Max)
            {
                //버프타입 생성
                stat = item.data.buffs[i].stat
            };
        }
       
    }
}