using UnityEngine;

/// <summary>
/// ������ �⺻ �̵� �Է±�
/// Ŀ�ǵ� �� : WASD , Shift
/// </summary>

public class User_Move : MonoBehaviour
{
    public float userSpeed;
    public float speedMultiplier;
    public float rotateSpeed;
    float rx, ry;
    Vector3 dir;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        dir = (/*(x* transform.right) +*/ (y * transform.forward)).normalized;
        //����ƮŰ�� ������ �� �뽬�ǵ��� �����.
        float totalSpeed;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            totalSpeed = userSpeed*speedMultiplier;
        }
        else
        {
            totalSpeed = userSpeed;
        }
        rx -= my * rotateSpeed * Time.deltaTime;
        ry += mx * rotateSpeed * Time.deltaTime;

        transform.localRotation = Quaternion.EulerAngles(rx, ry, 0);
        transform.position += dir * totalSpeed * Time.deltaTime;
    }
}
