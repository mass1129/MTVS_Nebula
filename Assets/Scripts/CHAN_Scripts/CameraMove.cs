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
        // 카메라 위치 이동기
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        dir = (x * transform.right + y * transform.forward).normalized;
        transform.position += dir * cameraSpeed * Time.deltaTime;
        // 카메라 시점 이동기
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        rx -= my * rotateSpeed*Time.deltaTime;
        ry += mx * rotateSpeed * Time.deltaTime;

        transform.localRotation = Quaternion.EulerAngles(rx, ry, 0);

    }
}
