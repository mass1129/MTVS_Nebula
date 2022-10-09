using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float cameraSpeed;
    public float rotateSpeed;
    Vector3 dir;
    public GameObject cam;
    float rx, ry;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ī�޶� ��ġ �̵���
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        dir = (x * transform.right + y * transform.forward).normalized;
        transform.position += dir * cameraSpeed * Time.deltaTime;
        // ī�޶� ���� �̵���
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rx -= my * rotateSpeed*Time.deltaTime;
        ry += mx * rotateSpeed * Time.deltaTime;

        transform.localRotation = Quaternion.EulerAngles(rx, ry, 0);

    }
}
