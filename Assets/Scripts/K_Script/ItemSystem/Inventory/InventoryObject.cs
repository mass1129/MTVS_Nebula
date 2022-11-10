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
    public InventorySlot[] GetSlots => Container.Slots;

    public bool AddItem(Item item, int amount)
    {
        //빈 슬룻이 없다면 false리턴
        if (EmptySlotCount <= 0)
            return false;
        //
        InventorySlot slot = FindItemOnInventory(item);
        if (!database.ItemObjects[item.Id].stackable || slot == null)
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
            Item item = new Item(bundleList[i]);
            GetSlots[i].UpdateSlot(item, 1);
        }
       
        
        
    }


    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for(int i =0; i<GetSlots.Length; i++)
            {
                if (GetSlots[i].item.Id <=-1)
                    counter++;
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
            if (GetSlots[i].item.Id == item.Id)
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
            if (GetSlots[i].item.Id == item.data.Id)
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
            if (GetSlots[i].item.Id <= -1)
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
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
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
            Debug.Log("Quick" +item2.item.Name + ","+ item1.item.Name);
        }

    }


    [ContextMenu("Save")]
    public void Save()
    {

        string json = JsonUtility.ToJson(Container, true);
        PlayerPrefs.SetString(savePath, json);
        K_SaveSystem.Save(savePath, json, true);

    }
    [ContextMenu("Load")]
    public void Load()
    {
        if (PlayerPrefs.HasKey(savePath))
        {
            string json = PlayerPrefs.GetString(savePath);
            json = K_SaveSystem.Load(savePath);

            JsonUtility.FromJsonOverwrite(json, Container);
            for(int i = 0; i < Container.Slots.Length; i++)
            {
                Container.Slots[i].UpdateSlot(Container.Slots[i].item, Container.Slots[i].amount);
            }
        }

    }
    public async void TestLoad()
    {
        var url = "http://ec2-43-201-55-120.ap-northeast-2.compute.amazonaws.com:8001/inventory/" + loadPath + "/testAvatar";
        var httpReq = new HttpRequester(new JsonSerializationOption());

        H_I_Root result2 = await httpReq.Get<H_I_Root>(url);
       
        //string json = JsonUtility.ToJson(array, true);
        //Debug.Log(json);
        for (int i = 0; i < Container.Slots.Length; i++)
        {
            Container.Slots[i].UpdateSlot(result2.results.slots[i].item, result2.results.slots[i].amount);
        }
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < Container.Slots.Length; i++)
        {
            Container.Slots[i].UpdateSlot(Container.Slots[i].item, Container.Slots[i].amount);
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

    
    public class H_I_Item
    {
        public int uniqueId { get; set; }
        public ItemBuff[] buffs { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }


    public class H_I_Slot
    {
        public Item item { get; set; }
        public int amount { get; set; }
        public ItemType[] allowedItems { get; set; }

    }
    public class H_I_Results
    {
        public H_I_Slot[] slots { get; set; }
    }
   
    public class H_I_Root
    {
        public int httpStatus { get; set; }
        public string message { get; set; }
        public H_I_Results results { get; set; }
    }

   

}

