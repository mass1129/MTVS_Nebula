using UnityEngine;

/// <summary>
/// 유저의 기본 이동 입력기
/// 커맨드 값 : WASD , Shift
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
        //쉬프트키를 눌렀을 때 대쉬되도록 만든다.
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
