using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
   public InventoryObject inventory;
    //인벤토리 아이템 배치 시작지점(아닐시 중간부터 배치)
    public int X_START;
    public int Y_START;
    //아이템 슬롯 x,y 간격
    public int X_SPACE_BETWEEN_ITEM;
    public int Y_SPACE_BETWEEN_ITEM;
    //아이템 갯수
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
        //인벤토리 슬롯 카운터만큼
        for(int i = 0; i<inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];
            //이 컴포넌트를 가진 객체의 자식으로 item의 prefab(=아이콘)를 생성한다.
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, inventoryWindow.transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.Id].uiDisplay;
            //아이콘의 위치를 정한다.
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            //아이콘 자식에 있는 텍스트를 슬롯에 있는 갯수로 한다.
            obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            itemsDisplayed.Add(slot, obj);
        }
    }
    bool isVisible = false;
    private void UpdateDisplay()
    {   
        //인벤토리 슬롯를 검사한다.
        for(int i =0; i<inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];
            //딕션어리에 해당 키 값이 있다면
            if (itemsDisplayed.ContainsKey(slot))
            {   
                //해당 키(아이템 아이콘)의 text(갯수)를 업데이트 해준다. 
                itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            }
            //키값이 없다면
            else
            {
                //이 컴포넌트를 가진 객체의 자식으로 item의 prefab(=아이콘)를 생성한다.
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, inventoryWindow.transform);
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.Id].uiDisplay;
                //아이콘의 위치를 정한다.
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                //아이콘 자식에 있는 텍스트를 슬롯에 있는 갯수로 한다.
                obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
                //딕션어리에 추가한다.
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
