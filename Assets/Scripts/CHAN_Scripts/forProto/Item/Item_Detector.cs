using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Detector : MonoBehaviour
{
    Collider[] detect;
    public float range_detect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        detect = Physics.OverlapSphere(transform.position, range_detect, 1 << 11);
        foreach (Collider c in detect)
        {
            if (c.gameObject.CompareTag("TV"))
            {
                //여기서 감지가 되면 TV 앞에 안내 Text 나오게 한다.
                Item_TVManager.instance.CanControlTV(true);
                Item_TVManager.instance.done = true;
            }
        }
        if (detect.Length == 0)
        {
            Item_TVManager.instance.CanControlTV(false);
            Item_TVManager.instance.done = false;
        }
    }
}
