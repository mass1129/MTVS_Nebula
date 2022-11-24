using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_IconMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float posY;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Map_UIManager.instance)
        {
            if (Map_UIManager.instance.state_View == "skymap")
            {
                SetIcon_Skymap();
            }
            else if (Map_UIManager.instance.state_View == "minimap")
            {
                SetIcon_Minimap();
            }
        }
    }
    public void OnPlayerIcon()
    {
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
    }
    public void OnUserIcon()
    {
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
    }
    /// <summary>
    /// 아이콘의 위치를 특정 벡터위치로 잡는다. 
    /// </summary>
    public void SetIcon_Minimap()
    {
        Vector3 parentPos = transform.parent.position;
        Quaternion parentRot = transform.parent.rotation;
        transform.GetChild(0).localRotation=  Quaternion.Euler(90, 0, 0);
        float rotY = Mathf.Deg2Rad * parentRot.eulerAngles.y;
        transform.position = new Vector3(parentPos.x, posY, parentPos.z);
        transform.rotation = Quaternion.EulerAngles(0, rotY, 0);
    }
    /// <summary>
    /// 아이콘의 위치를 플레이어의 위치로 잡는다. 
    /// 아이콘이 바라보는 방향은 카메라의 방향으로 잡는다. 
    /// 타겟 카메라는 SkyCamera
    /// </summary>
    public void SetIcon_Skymap()
    {
        transform.GetChild(0).localRotation = Quaternion.identity;
        transform.position = transform.parent.position;
        transform.LookAt(GameObject.Find("SkyMap_Camera").transform);
    }
}
