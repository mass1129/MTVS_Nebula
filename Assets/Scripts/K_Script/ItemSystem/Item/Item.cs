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
    //new Item(ItemObject)할때 Item클래스를 생성하는 부분
    public Item(ItemObject item)
    {
        //아이템오브젝트의 이름(이건 유니티상에서 지정해준 이름)과 Id를 Item클래스의 이름과 id에 대입
        name = item.name;
        id = item.data.id;
       
    }
}