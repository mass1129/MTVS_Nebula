using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public string loadPath;
    public ItemDatabaseObject database;
    public InterfaceType type;

    [SerializeField]
    private Inventory Container = new Inventory();
    //private�� ó���� Container�� �����ϱ����� ���� ����
    public InventorySlot[] GetSlots => Container.slots;

    public bool AddItem(Item item, int amount)
    {
        //�� ������ ���ٸ� false����
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
                Container.slots[i].UpdateSlot(item, 1);

            }
            
        }
       
        
        
    }


    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for(int i =0; i< Container.slots.Length; i++)
            {
                if (Container.slots[i].item.id <= -1)
                {
                    counter++;
                }
                   
            }
            return counter;
        }
    }

    //�κ��丮 ���Կ� �ش� item�� �ִ� ������ ã�Ƽ� �ش� slot�� �����Ѵ�.
    public InventorySlot FindItemOnInventory(Item item)
    {   
        //
        for (int i = 0; i < Container.slots.Length; i++)
        {
            if (Container.slots[i].item.id == item.id)
            {
                return Container.slots[i];
            }
            
        }
        return null;

    }
    public bool IsItemInInventory(ItemObject item)
    {
        for (int i = 0; i < Container.slots.Length; i++)
        {
            if (Container.slots[i].item.id == item.data.id)
            {
                return true;
            }
        }
        return false;
    }


    public InventorySlot GetEmptySlot()
    {
        for (int i = 0; i < Container.slots.Length; i++)
        {
            if (Container.slots[i].item.id <= -1)
            {
                return Container.slots[i];
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

   
    public async UniTask InventorySave(string s)
    {

        string json = JsonUtility.ToJson(Container, true);
        Debug.Log(json);
        //string s = PlayerPrefs.GetString(" AvatarName");
        var url = "https://resource.mtvs-nebula.com/" + savePath + s;
        var httpReq = new HttpRequester(new JsonSerializationOption());

        await httpReq.Post(url, json);
    }

    public async UniTask InventoryLoad(string s)
    {
        var url = "https://resource.mtvs-nebula.com/" + loadPath + s;
        var httpReq = new HttpRequester(new JsonSerializationOption());
        //���� ���� = �� ���� ��
        H_I_Root result2 = await httpReq.Get<H_I_Root>(url);
        for (int i = 0; i < Container.slots.Length; i++)
        {
            Container.slots[i].UpdateSlot(result2.results.slots[i].item, result2.results.slots[i].amount);
        }
        //�ڵ��� ����� ���� �ڵ� ���� -> ���� �����͸� �����ϴ� ���� �����Ͱ� �����Ǿ� �������ݷ��Ͱ� ���� ����� ������ ����  
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

 

    public class H_I_Root
    {
        public int httpStatus { get; set; }
        public string message { get; set; }
        
        public Inventory results { get; set; }
        //public InventorySlot[] slots => results.slots;
    }

   

}

