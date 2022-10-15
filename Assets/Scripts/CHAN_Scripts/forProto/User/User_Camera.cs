using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Camera : MonoBehaviour
{
    public float followSpeed;
    public Transform camHolderPos;
    bool turn;
    // 플레이어가 이동하면 카메라가 자동으로 플레이어에게 따라가도록 구현
    // 초기에 카메라 위치를 지정해준다. camHolder
    // camHolder는 플레이어의 자식이다. 
    // 만약 camHolder의 위치가 바뀐다면
    // 카메라는 lerp 를 이용해서 쫓아간다.
    private void Start()
    {
        TranslateCamPos();
    }
    void Update()
    {
        float distance = Vector3.Distance(transform.position, TranslateCamPos());
        if (distance >= 5)
        {
            turn = true;
        }
        if (distance <= 0.1f)
        {
            turn = false;
        }
        if (turn)
        { 
            transform.position = Vector3.Lerp(transform.position, TranslateCamPos(), Time.deltaTime * followSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, camHolderPos.rotation, Time.deltaTime * followSpeed);
        }
    }
    Vector3 TranslateCamPos()
    {
        Transform camPos = camHolderPos;
        Matrix4x4 trans = camPos.localToWorldMatrix;
        camPos.position = trans.GetPosition();
        return camPos.position;
    }
}
