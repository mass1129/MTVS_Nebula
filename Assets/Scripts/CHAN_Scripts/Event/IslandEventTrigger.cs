using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IslandEventTrigger : EventTrigger
{
    public void OnHover()
    {
        Debug.LogWarning("마우스 접근");
    }
    public void OnClicked()
    {
        Debug.LogWarning("마우스 클릭");
    }
}

