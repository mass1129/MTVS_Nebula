using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Camera : MonoBehaviour
{
    public float followSpeed;
    public Transform camHolderPos;
    bool turn;
    // �÷��̾ �̵��ϸ� ī�޶� �ڵ����� �÷��̾�� ���󰡵��� ����
    // �ʱ⿡ ī�޶� ��ġ�� �������ش�. camHolder
    // camHolder�� �÷��̾��� �ڽ��̴�. 
    // ���� camHolder�� ��ġ�� �ٲ�ٸ�
    // ī�޶�� lerp �� �̿��ؼ� �Ѿư���.
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
