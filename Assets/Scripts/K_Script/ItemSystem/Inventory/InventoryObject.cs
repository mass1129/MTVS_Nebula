using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public string loadPath;
    public ItemDatabaseObject database;
    public InterfaceType type;

    [SerializeField]
    private Inventory Container = new Inventory();
    public InventorySlot[] GetSlots => Container.slots;

    public bool AddItem(Item item, int amount)
    {
        //빈 슬룻이 없다면 false리턴
        if (EmptySlotCount <= 0)
            return false;
        //
        InventorySlot slot = FindItemOnInventory(item);
        if (!database.ItemObjects[item.id].stackable || slot == null)
        {
            GetEmptySlot().UpdateSlot(item, amount);
            return true;
        }
        slot.AddAmount(amount);
        return true;
    }
    public void AddBundleListToWindow(ItemObject[] bundleList)
    {
        Clear();
        for(int i = 0; i < bundleList.Length; i++)
        {   
            if(bundleList[i] !=null)
            {
                Item item = new Item(bundleList[i]);
                GetSlots[i].UpdateSlot(item, 1);

            }
            
        }
       
        
        
    }


    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for(int i =0; i<GetSlots.Length; i++)
            {
                if (GetSlots[i].item.id <= -1)
                {
                    counter++;
                }
                   
            }
            return counter;
        }
    }

    //인벤토리 슬롯에 해당 item이 있는 슬롯을 찾아서 해당 slot를 리턴한다.
    public InventorySlot FindItemOnInventory(Item item)
    {   
        //
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.id == item.id)
            {
                return GetSlots[i];
            }
            
        }
        return null;

    }
    public bool IsItemInInventory(ItemObject item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.id == item.data.id)
            {
                return true;
            }
        }
        return false;
    }


    public InventorySlot GetEmptySlot()
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.id <= -1)
            {
                return GetSlots[i];
            }
        }
        return null;
    }
    
    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item1 == item2)
            return;
        if (item2.CanPlaceInSlot(item1.GetItemObject()) && item1.CanPlaceInSlot(item2.GetItemObject()))
        {
            InventorySlot temp = new InventorySlot(item1.item, item1.amount);
            if (item2 != null)
                item1.UpdateSlot(item2.item, item2.amount);
            else
                item1.RemoveItem();
            item2.UpdateSlot(temp.item, temp.amount);
            Debug.Log("Swap");
        }

    }
    public void ItemsToQuickSlot(InventorySlot item1, InventorySlot item2)
    {
        if (item1 == item2)
            return;
        if (item2.CanPlaceInSlot(item1.GetItemObject()) && item1.CanPlaceInSlot(item2.GetItemObject()))
        {
            
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(item1.item, item1.amount);
            Debug.Log("Quick" +item2.item.name + ","+ item1.item.name);
        }

    }
    public Inventory GetInventory()
    {
        return Container;
    }

   
    public async void TestSave()
    {

        string json = JsonUtility.ToJson(Container, true);
        Debug.Log(json);
        //string s = PlayerPrefs.GetString(" AvatarName");
        var url = "https://resource.mtvs-nebula.com/" + savePath + PlayerPrefs.GetString("AvatarName");
        var httpReq = new HttpRequester(new JsonSerializationOption());

        await httpReq.Post(url, json);
    }
    public async void TestLoad()
    {
        var url = "https://resource.mtvs-nebula.com/" + loadPath + PlayerPrefs.GetString("AvatarName");
        var httpReq = new HttpRequester(new JsonSerializationOption());

        H_I_Root result2 = await httpReq.Get<H_I_Root>(url);

        Inventory newInven = result2.results;
        for (int i = 0; i < GetSlots.Length; i++)
        {
          GetSlots[i].UpdateSlot(newInven.slots[i].item, newInven.slots[i].amount);
            //Container.slots[i].UpdateSlot(Container.slots[i].item, Container.slots[i].amount);
        }
      



    }

    public void UpdateInventory()
    {
        for (int i = 0; i < Container.slots.Length; i++)
        {
            Container.slots[i].UpdateSlot(Container.slots[i].item, Container.slots[i].amount);
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }
    public void Click()
    {
        //Debug.Log(GetSlots.item.);
    }


    //public class Item
    //{
    //    public int uniqueId { get; set; }
    //    public ItemBuff[] buffs { get; set; }
    //    public int id { get; set; }
    //    public string name { get; set; }
    //}

    //[SerializeField]
    //public class H_I_Slot
    //{
    //    public Item item { get; set; }
    //    public int amount { get; set; }
    //    public ItemType[] allowedItems { get; set; }

    //}

 

    public class H_I_Root
    {
        public int httpStatus { get; set; }
        public string message { get; set; }
        
        public Inventory results { get; set; }
        //public InventorySlot[] slots => results.slots;
    }

   

}

