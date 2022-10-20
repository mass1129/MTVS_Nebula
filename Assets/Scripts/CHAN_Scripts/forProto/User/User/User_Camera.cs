using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Camera : MonoBehaviour
{
    public float followSpeed;
    public Transform camHolderPos;
    bool turn;
    float wheelMultiplier;
    public float multi;
    Transform pos;
    // 플레이어가 이동하면 카메라가 자동으로 플레이어에게 따라가도록 구현
    // 초기에 카메라 위치를 지정해준다. camHolder
    // camHolder는 플레이어의 자식이다. 
    // 만약 camHolder의 위치가 바뀐다면
    // 카메라는 lerp 를 이용해서 쫓아간다.
    private void Start()
    {
        pos = camHolderPos;
        TranslateCamPos();
    }
    void Update()
    {
        // 카메라와 카메라 홀더 사이 거리 계산
        float distance = Vector3.Distance(transform.position, TranslateCamPos());
        // 둘 사이 거리가 멀어지면  카메라가 따라오도록 
        if (distance >= 5)
        {
            turn = true;
        }
        // 둘 사이 거리가 일정거리 가까워지면 카메라 따라오기 중지
        if (distance <= 0.1f)
        {
            turn = false;
        }
        if (turn&&!User_Move.instance.islandSelected)
        { 
            transform.position = Vector3.Lerp(transform.position, TranslateCamPos(), Time.deltaTime * followSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, camHolderPos.rotation, Time.deltaTime * followSpeed);
        }
        if(Input.GetKey(KeyCode.LeftControl))
        {
            wheelMultiplier += Input.GetAxisRaw("Mouse ScrollWheel");
            print(wheelMultiplier);
            camHolderPos.position += transform.forward * wheelMultiplier * multi;
            
        }
        
    }
    Vector3 TranslateCamPos()
    {
        Transform camPos = pos;
        Matrix4x4 trans = camPos.localToWorldMatrix;
        camPos.position = trans.GetPosition();
        return camPos.position;
    }
}
