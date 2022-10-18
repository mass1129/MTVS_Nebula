using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//json에서 받아온 정보들을 해당 클래스에 저장한다. 
public class Island_Spawner : MonoBehaviour
{

    
    //유저가 접속하면 유저의 정보를 가져온다. 
    //일단 인위적으로 100명의 유저가 접속했다고 가정하겠음
    List<GameObject> islands = new List<GameObject>();
    List<Vector3> islandsPos = new List<Vector3>();
    public Transform islandsTransForm;

    private void Start()
    {
        //OnLoadUserInfo();
        for (int i = 0; i < LoadJson.instance.positions.Count; i++)
        {
            //Info_Island island = new Info_Island();
            
            GameObject land = Instantiate(Resources.Load<GameObject>("CHAN_Resources/"+ LoadJson.instance.Islands[i]),islandsTransForm);
            land.transform.localScale *= 0.1f;
            islands.Add(land);
            //Vector3 pos = RandomPos();
            //for (int j = 0; j < islandsPos.Count; j++)
            //{
            //    if (pos == islandsPos[j])
            //    {
            //        pos = RandomPos();
            //    }
            //}
            land.transform.position = LoadJson.instance.positions[i];

            
            //land.transform.localScale *=0.5f ;
            //그리고 스폰한 위치를 리스트에 저장 
            //islandsPos.Add(land.transform.position);
            //만약 스폰하려는 위치가 이미 리스트상에 있으면 다시 랜덤으로 돌린다.
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
}
