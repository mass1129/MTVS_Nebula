using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// InputField에서 Tab과 Shift+Tab키를 누르는 것을 관리하는 클래스
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
    /// Focus 할 InputField를 설정한다.
    /// </summary>
    /// <param name="idx">Focus 할 InputField의 index 번호</param>
    public void SetFocus(int idx = 0)
    {
        if (idx >= 0 && idx < list.Count)
            list[idx].Select();
    }

    /// <summary>
    /// Tab, Shift+Tab 키를 눌렀을 때 반응 할 InputField를 추가한다.
    /// </summary>
    /// <param name="inputField">추가 할 InputField</param>
    public void Add(TMP_InputField inputField)
    {
        list.Add(inputField);
    }

    /// <summary>
    /// 현재 위치를 얻는다.
    /// </summary>
    /// <returns>현재 위치의 Index</returns>
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
    /// 다음 InputField로 Focus한다.
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
    ///// 이전 InputField로 Focus한다.
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
    /// Tab키와 Shift + Tab키를 눌렀는지 체크하여 눌렀으면 Focus를 옮긴다.
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