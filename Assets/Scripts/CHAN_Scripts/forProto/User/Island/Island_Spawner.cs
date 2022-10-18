using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//json���� �޾ƿ� �������� �ش� Ŭ������ �����Ѵ�. 
public class Island_Spawner : MonoBehaviour
{

    
    //������ �����ϸ� ������ ������ �����´�. 
    //�ϴ� ���������� 100���� ������ �����ߴٰ� �����ϰ���
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
            //�׸��� ������ ��ġ�� ����Ʈ�� ���� 
            //islandsPos.Add(land.transform.position);
            //���� �����Ϸ��� ��ġ�� �̹� ����Ʈ�� ������ �ٽ� �������� ������.
        }
    }

    void OnLoadUserInfo()
    {
        //json �������� ������ ������ �����´�.
        //json ������ �Ľ��Ѵ�.
        //�Ľ��� �������� �������� Ŭ������ �����Ͽ� �����Ѵ�. 
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
