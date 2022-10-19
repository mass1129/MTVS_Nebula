using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//json에서 받아온 정보들을 해당 클래스에 저장한다. 
public class Island_Spawner : MonoBehaviour
{
    // 섬정보 클래스 생성
    IslandInformation islandInformation = new IslandInformation();
    
    List<GameObject> islands = new List<GameObject>();
    List<Vector3> islandsPos = new List<Vector3>();
    public Transform islandsTransForm;

    private void Start()
    {
        //일단 csv파일 추출해서 콘솔에 보이도록 해보자
        //key값이 2부터 시작한다 딱히 신경쓸부분은 아니다 나중에 key값에 유저 이름 들어가니까
        islandInformation.LoadFromCSV("subset30.csv");
        ForVerifyInfo();
        
        for (int i = 0; i < islandInformation.User_name.Count; i++)
        {
            // 리스트에 있는 유저들의  Key값을 가져온다.
            string userName = islandInformation.User_name[i];
            GameObject land = Instantiate(Resources.Load<GameObject>("CHAN_Resources/" + islandInformation.island_Type[userName]), islandsTransForm);
            land.transform.localScale *= 0.5f;
            islands.Add(land);
            //Vector3 pos = RandomPos();
            //for (int j = 0; j < islandsPos.Count; j++)
            //{
            //    if (pos == islandsPos[j])
            //    {
            //        pos = RandomPos();
            //    }
            //}
            land.transform.position = islandInformation.island_Pos[userName];


        //        //land.transform.localScale *=0.5f ;
        //        //그리고 스폰한 위치를 리스트에 저장 
        //        //islandsPos.Add(land.transform.position);
        //        //만약 스폰하려는 위치가 이미 리스트상에 있으면 다시 랜덤으로 돌린다.
           }

    }
    void OnLoadUserInfo()
    {
        //json 형식으로 유저의 정보를 가져온다.
        //json 정보를 파싱한다.
        //파싱한 정보들을 유저정보 클래스를 생성하여 저장한다. 
    }
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
        for (int i = 2; i < islandInformation.island_Pos.Count + 2; i++)
        {
            print(islandInformation.island_Pos[i.ToString()] + " " + islandInformation.island_Type[i.ToString()] + " "
                + islandInformation.User_image[i.ToString()]);
        }
    }
}
