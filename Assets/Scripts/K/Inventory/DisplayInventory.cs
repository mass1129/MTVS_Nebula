using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        for(int i = 0; i<inventory.Container.Count; i++)
        {   
            //�� ������Ʈ�� ���� ��ü�� �ڽ����� item�� prefab(=������)�� �����Ѵ�.
            var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            //�������� ��ġ�� ���Ѵ�.
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            //������ �ڽĿ� �ִ� �ؽ�Ʈ�� ���Կ� �ִ� ������ �Ѵ�.
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
        }
    }
    private void UpdateDisplay()
    {   
        //�κ��丮 ���Ը� �˻��Ѵ�.
        for(int i =0; i<inventory.Container.Count; i++)
        {   
            //��Ǿ�� �ش� Ű ���� �ִٸ�
            if (itemsDisplayed.ContainsKey(inventory.Container[i]))
            {   
                //�ش� Ű(������ ������)�� text(����)�� ������Ʈ ���ش�. 
                itemsDisplayed[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            }
            //Ű���� ���ٸ�
            else
            {
                //�� ������Ʈ�� ���� ��ü�� �ڽ����� item�� prefab(=������)�� �����Ѵ�.
                var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                //�������� ��ġ�� ���Ѵ�.
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                //������ �ڽĿ� �ִ� �ؽ�Ʈ�� ���Կ� �ִ� ������ �Ѵ�.
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
                //��Ǿ�� �߰��Ѵ�.
                itemsDisplayed.Add(inventory.Container[i], obj);
            }
        }
    }
    public Vector3 GetPosition(int i)
    {

        return new Vector3(X_START +(X_SPACE_BETWEEN_ITEM*(i%NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEM * (i/NUMBER_OF_COLUMN )), 0f);
    }
    
}
