using UnityEngine;

/// <summary>
/// 유저의 기본 이동 입력기
/// 커맨드 값 : WASD , Shift
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
        //쉬프트키를 눌렀을 때 대쉬되도록 만든다.
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
