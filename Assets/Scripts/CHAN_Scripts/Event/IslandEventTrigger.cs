using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IslandEventTrigger : EventTrigger
{
    public void OnHover()
    {
        Debug.LogWarning("���콺 ����");
    }
    public void OnClicked()
    {
        Debug.LogWarning("���콺 Ŭ��");
    }
}

