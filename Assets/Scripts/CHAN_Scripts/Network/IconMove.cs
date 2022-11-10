using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float posY;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 parentPos = transform.parent.position;
        Quaternion parentRot = transform.parent.rotation;
        float rotY = Mathf.Deg2Rad*parentRot.eulerAngles.y;
        transform.position = new Vector3(parentPos.x, posY, parentPos.z);
        transform.rotation = Quaternion.EulerAngles(0,rotY, 0);
        //만약 부모쪽의 photonView가 내것이 아니라면 초록
        //맞으면 파랑으로 전환
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
}
