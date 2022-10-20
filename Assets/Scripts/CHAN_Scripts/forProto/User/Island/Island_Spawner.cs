using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//json에서 받아온 정보들을 해당 클래스에 저장한다. 
public class Island_Spawner : MonoBehaviour
{
    // 섬정보 클래스 생성
    //IslandInformation islandInformation = new IslandInformation();
    //List<GameObject> islands = new List<GameObject>();
    public Transform islandsTransForm;

    private void Start()
    {
       

    }
    //섬 정보 가져오게 하는 트리거 함수
    public void OnLoadUserInfo()
    {
        //일단 csv파일 추출해서 콘솔에 보이도록 해보자
        //key값이 2부터 시작한다 딱히 신경쓸부분은 아니다 나중에 key값에 유저 이름 들어가니까
        IslandInformation.instance.LoadFromCSV("subset_30_v2_fin.csv");
        ForVerifyInfo();

        for (int i = 0; i < IslandInformation.instance.User_name.Count; i++)
        {
            // 리스트에 있는 유저들의  Key값을 가져온다.
            string userName = IslandInformation.instance.User_name[i];
            GameObject land = Instantiate(Resources.Load<GameObject>("CHAN_Resources/" + IslandInformation.instance.island_Type[userName]), islandsTransForm);
            land.transform.localScale *= 0.5f;
            //생성한 하늘섬에게 이름을 부여
            land.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Island_Profile>().user_name = userName;
            land.transform.position = IslandInformation.instance.island_Pos[userName];
            IslandInformation.instance.UserObj.Add(land);

            //        //land.transform.localScale *=0.5f ;
            //        //그리고 스폰한 위치를 리스트에 저장 
            //        //만약 스폰하려는 위치가 이미 리스트상에 있으면 다시 랜덤으로 돌린다.
        }
    }
    //랜덤 좌표로 생성시키는 함수
    Vector3 RandomPos()
    {
        float width = WorldManager.instance.width * 0.5f;
        float height = WorldManager.instance.height * 0.5f;

        float random_W = Random.Range(-width, width);
        float random_L = Random.Range(-width, width);
        float random_H = Random.Range(-height, height);
        Vector3 pos = new Vector3(random_W, random_H, random_L);
        return pos;
    }
    // 받아온 데이터 확인 전용 함수
    void ForVerifyInfo()
    {
        for (int i = 2; i < IslandInformation.instance.island_Pos.Count + 2; i++)
        {
            print(IslandInformation.instance.island_Pos[i.ToString()] + " " + IslandInformation.instance.island_Type[i.ToString()]
                + IslandInformation.instance.User_image[i.ToString()]);
        }
    }
}
