using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV_Detect : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // collider 감지하면, 그것이 사람이면 Text 켜지게 한다. 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Item_TVManager.instance.CanControlTV(true);
            Item_TVManager.instance.done = true;
        }
        else
        {
            Item_TVManager.instance.CanControlTV(false);
            Item_TVManager.instance.done = false;
            if (Item_TVManager.instance.isTurn)
                Copy_Window_Texture.instance.OnClicked(false);
        }
    }
}
