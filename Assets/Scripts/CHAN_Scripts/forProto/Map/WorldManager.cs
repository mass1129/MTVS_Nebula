using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour 
{
    // 하늘맵 씬의 전반적인 액션을 관리하는 스크립트
    // * 유저들에게 할당받은 좌표값을 해당 맵에 스폰할 수 있도록 만들어야함
    // * 월드의 한계선을 정해야 함, 이를 속성으로 부여
    // * 시간개념이 들어가야 함

    //속성: 월드의 너비, 높이, 한계선 경고 영역판정 비율, 유저 리스트 , 경고 텍스트 
    public float width;
    public float height;
    public float warningZoneRatio;

    [SerializeField]List<GameObject> userObjs = new List<GameObject>();

    public Text warningText;

    private void Start()
    {
        warningText.enabled = false;
    }
    private void Update()
    {
        ControlLimitZone();
    }
    //유저의 현재 위치정보를 받아서 한계선에 있는지 확인하는 기능을 구현할거다.
    // 월드 내의 모든 유저의 정보를 받는다(유저 네임태그가 붙었거나 특정 스크립트가 있는 오브젝트를 찾느다. 
    // 만약 유저들 중 일부가 한계선 기준 n%지점에 있을때
    //유저에게 경고 메세지 전달 
    void ControlLimitZone()
    {
        float warningZone_Width = width*(1-warningZoneRatio*0.01f)*0.5f;
        float warningZone_height = height * (1 - warningZoneRatio * 0.01f) * 0.5f;

        float DangerZone_width = width * 0.5f;
        float DangerZone_height = height * 0.5f;
        
        //현재 등록된 유저인원수만큼 반복
        foreach (GameObject user in userObjs)
        {
            Vector3 userPos = user.transform.position;
            //유저가 위험 구간에 도달했을 때
            if (Mathf.Abs(userPos.x) > warningZone_Width || Mathf.Abs(userPos.z) > warningZone_Width || Mathf.Abs(userPos.y) > warningZone_height)
            {
                //이때 경고 메세지를 보낸다. 
                warningText.enabled = true;
                warningText.text = "맵에서 벗어나고 있어요!";
                if (Mathf.Abs(userPos.x)> DangerZone_width || Mathf.Abs(userPos.z) > DangerZone_width || Mathf.Abs(userPos.y) > DangerZone_height)
                {
                    //일단은 원점으로 다시 돌려 보내자
                    user.transform.position = Vector3.zero;
  
                }
                
            }
            else
            {
                warningText.enabled = false;
            }
        }
            
        
    }
    #region 외부에서 유저가 로그인or 로그아웃 했을 때, 해당함수가 호출된다. 
    public void PlayerLogIn(GameObject player)
    {
        userObjs.Add(player);
    }
    public void PlayerLogOut(GameObject player)
    {
        userObjs.Remove(player);
    }
#endregion
}
