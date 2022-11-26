using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CHAN_Btn_Event : MonoBehaviour
{
    public void Btn_On()
    {
        transform.GetChild(0).GetComponent<TMP_Text>().enabled = true;
    }
    public void Btn_Off()
    {
        transform.GetChild(0).GetComponent<TMP_Text>().enabled = false;
    }

}
