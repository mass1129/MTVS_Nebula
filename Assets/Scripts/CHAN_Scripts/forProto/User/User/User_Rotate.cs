using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Rotate : MonoBehaviour
{
    public static User_Rotate instance;
    private void Awake()
    {
        instance = this;
    }
    float rx;
    public float ry;
    public float rotateSpeed;

    void Start()
    {
        
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        rx -= my * rotateSpeed * Time.deltaTime;
        ry += mx * rotateSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.EulerAngles(rx, ry, 0);

    }
}
