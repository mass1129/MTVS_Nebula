using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public string loadPath;
    public ItemDatabaseObject database;
    public InterfaceType type;
    public string fileName;
    [SerializeField]
    private Inventory Container = new Inventory();
    //private�� ó���� Container�� �����ϱ����� �б� ���� ����
    public InventorySlot[] GetSlots => Container.slots;


    public void AddBundleListToWindow(ItemObject[] bundleList)
    {
        for(int i = 0; i < bundleList.Length; i++)
        {   
            if(bundleList[i] !=null)
            {
                Item item = new Item(bundleList[i]);
                Container.slots[i].UpdateSlot(item, 1);

            }
        }

    }


    public void SwapItems(InventorySlot dragExitSlot, InventorySlot dragStartSlot)
    {
        if (dragExitSlot == dragStartSlot || !dragStartSlot.CanPlaceInSlot(dragExitSlot.GetItemObject()) 
            || !dragExitSlot.CanPlaceInSlot(dragStartSlot.GetItemObject()))
            return;
        
        InventorySlot temp = new InventorySlot(dragExitSlot.item, dragExitSlot.amount);

        dragExitSlot.UpdateSlot(dragStartSlot.item, dragStartSlot.amount);
       
        dragStartSlot.UpdateSlot(temp.item, temp.amount);

    }

    public Inventory GetInventory()
    {
        return Container;
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



    public async UniTaskVoid InventorySave(string s, bool fileSave= true)
    {

        string json = JsonUtility.ToJson(Container, true);
        Debug.Log(json);    
        if(fileSave)
        {
            K_SaveSystem.Save(fileName, json, true);
            return;
        }
        var url = "https://resource.mtvs-nebula.com/" + savePath + s;
        var httpReq = new HttpRequester(new JsonSerializationOption());

        await httpReq.Post(url, json);
    }

    public async UniTaskVoid InventoryLoad(string s, bool fileSave = true)
    {   
        if(fileSave)
        {
            Inventory result = K_SaveSystem.LoadObject<Inventory>(fileName);
            for (int i = 0; i < Container.slots.Length; i++)
            {
                Container.slots[i].UpdateSlot(result.slots[i].item, result.slots[i].amount);
            }
            return;
        }    
        var url = "https://resource.mtvs-nebula.com/" + loadPath + s;
        var httpReq = new HttpRequester(new JsonSerializationOption());
        //���� ���� = ���� ���� �� ����
        H_I_Root result2 = await httpReq.Get<H_I_Root>(url);
        this.Container = result2.results;
        UpdateInventory();
        //�ڵ��� ����� ����(result2) �ڵ� ���� -> ���� �����͸� �����ϴ� ����(result2)�� �����Ǿ����� �÷��̾� �κ��丮�� �����ϰ��־ �� ����
    }

    public async UniTaskVoid ForGiveItem(InventorySlot dropSlot, string avatarName)
    {
        DropItem dropItem = new DropItem
        {
            uniqueId = dropSlot.item.uniqueId
        };
        if (dropItem.uniqueId < 0) return;
        string json = JsonUtility.ToJson(dropItem, true);
        var url = "https://resource.mtvs-nebula.com/achieve/drop/" + avatarName;
        var httpReq = new HttpRequester(new JsonSerializationOption());

        await httpReq.Post(url, json);

        InventoryLoad(avatarName).Forget();
    }




    public class H_I_Root
    {
        public int httpStatus { get; set; }
        public string message { get; set; }
        
        public Inventory results { get; set; }
    }

    #region �ּ�ó��
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

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < Container.slots.Length; i++)
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
    #endregion 
}

[System.Serializable]
public class DropItem
{
    public int uniqueId;
}

