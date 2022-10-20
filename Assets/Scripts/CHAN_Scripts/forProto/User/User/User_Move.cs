using UnityEngine;

/// <summary>
/// ������ �⺻ �̵� �Է±�
/// Ŀ�ǵ� �� : WASD , Shift
/// </summary>

public class User_Move : MonoBehaviour
{
    public static User_Move instance;
    private void Awake()
    {
        instance = this;
    }
    public float userSpeed;
    public float speedMultiplier;
  
    
    Vector3 dir;
    public bool islandSelected;



    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        

        dir = (/*(x* transform.right) +*/ (y * transform.forward)).normalized;
        //����ƮŰ�� ������ �� �뽬�ǵ��� �����.
        float totalSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalSpeed = userSpeed*speedMultiplier;
        }
        else
        {
            totalSpeed = userSpeed;
        }

            
        transform.position += dir * totalSpeed * Time.deltaTime;
    }
}
