using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
   public InventoryObject inventory;
    //�κ��丮 ������ ��ġ ��������(�ƴҽ� �߰����� ��ġ)
    public int X_START;
    public int Y_START;
    //������ ���� x,y ����
    public int X_SPACE_BETWEEN_ITEM;
    public int Y_SPACE_BETWEEN_ITEM;
    //������ ����
    public int NUMBER_OF_COLUMN;

    public GameObject inventoryWindow;
    public GameObject inventoryPrefab;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    private void Start()
    {
        CreateDisplay();
    }

    private void Update()
    {
        UpdateDisplay();
    }
    public  void CreateDisplay()
    {   
        //�κ��丮 ���� ī���͸�ŭ
        for(int i = 0; i<inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];
            //�� ������Ʈ�� ���� ��ü�� �ڽ����� item�� prefab(=������)�� �����Ѵ�.
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, inventoryWindow.transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.Id].uiDisplay;
            //�������� ��ġ�� ���Ѵ�.
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            //������ �ڽĿ� �ִ� �ؽ�Ʈ�� ���Կ� �ִ� ������ �Ѵ�.
            obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            itemsDisplayed.Add(slot, obj);
        }
    }
    bool isVisible = false;
    private void UpdateDisplay()
    {   
        //�κ��丮 ���Ը� �˻��Ѵ�.
        for(int i =0; i<inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];
            //��Ǿ�� �ش� Ű ���� �ִٸ�
            if (itemsDisplayed.ContainsKey(slot))
            {   
                //�ش� Ű(������ ������)�� text(����)�� ������Ʈ ���ش�. 
                itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            }
            //Ű���� ���ٸ�
            else
            {
                //�� ������Ʈ�� ���� ��ü�� �ڽ����� item�� prefab(=������)�� �����Ѵ�.
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, inventoryWindow.transform);
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.Id].uiDisplay;
                //�������� ��ġ�� ���Ѵ�.
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                //������ �ڽĿ� �ִ� �ؽ�Ʈ�� ���Կ� �ִ� ������ �Ѵ�.
                obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
                //��Ǿ�� �߰��Ѵ�.
                itemsDisplayed.Add(slot, obj);
            }
        }

        if(Input.GetKeyDown(KeyCode.I))
        {   
            if(!isVisible)
            {
                inventoryWindow.gameObject.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                isVisible = true;
            }
            else
            {
                inventoryWindow.gameObject.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                isVisible = false;
            }
            
        }
    }
    public Vector3 GetPosition(int i)
    {

        return new Vector3(X_START +(X_SPACE_BETWEEN_ITEM*(i%NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEM * (i/NUMBER_OF_COLUMN )), 0f);
    }
    
}
