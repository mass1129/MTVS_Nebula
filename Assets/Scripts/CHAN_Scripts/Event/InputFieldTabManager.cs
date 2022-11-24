using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// InputField���� Tab�� Shift+TabŰ�� ������ ���� �����ϴ� Ŭ����
/// </summary>
public class InputFieldTabManager
{
    private List<TMP_InputField> list;
    private int curPos;

    public InputFieldTabManager()
    {
        list = new List<TMP_InputField>();
    }

    /// <summary>
    /// Focus �� InputField�� �����Ѵ�.
    /// </summary>
    /// <param name="idx">Focus �� InputField�� index ��ȣ</param>
    public void SetFocus(int idx = 0)
    {
        if (idx >= 0 && idx < list.Count)
            list[idx].Select();
    }

    /// <summary>
    /// Tab, Shift+Tab Ű�� ������ �� ���� �� InputField�� �߰��Ѵ�.
    /// </summary>
    /// <param name="inputField">�߰� �� InputField</param>
    public void Add(TMP_InputField inputField)
    {
        list.Add(inputField);
    }

    /// <summary>
    /// ���� ��ġ�� ��´�.
    /// </summary>
    /// <returns>���� ��ġ�� Index</returns>
    private int GetCurerntPos()
    {
        for (int i = 0; i < list.Count; ++i)
        {
            if (list[i].isFocused == true)
            {
                curPos = i;
                break;
            }
        }
        return curPos;
    }

    /// <summary>
    /// ���� InputField�� Focus�Ѵ�.
    /// </summary>
    private void MoveNext()
    {
        //GetCurerntPos();
        Debug.Log(curPos);
        if (curPos <= list.Count-1 )
        {
            list[curPos].Select();
        }
        if (curPos > list.Count-1 )
        {
            curPos = 0;
            list[curPos].Select();                  
        }
    }

    ///// <summary>
    ///// ���� InputField�� Focus�Ѵ�.
    ///// </summary>
    //private void MovePrev()
    //{
    //    //GetCurerntPos();
    //    if (curPos > 0)
    //    {
    //        --curPos;
    //        list[curPos].Select();
    //    }
    //}

    /// <summary>
    /// TabŰ�� Shift + TabŰ�� �������� üũ�Ͽ� �������� Focus�� �ű��.
    /// </summary>
    public void CheckFocus()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !Input.GetKey(KeyCode.LeftShift))
        {
            ++curPos;
            MoveNext();
        }
        //if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Tab))
        //{
        //    MovePrev();
        //}
    }
}