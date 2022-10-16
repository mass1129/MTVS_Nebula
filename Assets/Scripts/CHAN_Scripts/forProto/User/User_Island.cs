using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class User_Island : MonoBehaviour
{
    //������ �����ϸ� ������ ������ �����´�. 
    //�ϴ� ���������� 100���� ������ �����ߴٰ� �����ϰ���
    List<GameObject> islands = new List<GameObject>();
    List<Vector3> islandsPos = new List<Vector3>();
    public Transform islandsTransForm;
    private void Start()
    {
        
        for (int i = 0; i < 100; i++)
        {
            //Info_Island island = new Info_Island();
            GameObject land = Instantiate(Resources.Load<GameObject>("CHAN_Resources/Island 1"));
            islands.Add(land);
            Vector3 pos = RandomPos();
            for (int j = 0; j < islandsPos.Count; j++)
            {
                if (pos == islandsPos[j])
                { 
                    pos= RandomPos();
                }
            }
            land.transform.position = pos;
            //land.transform.localScale *=0.5f ;
            //�׸��� ������ ��ġ�� ����Ʈ�� ���� 
            islandsPos.Add(land.transform.position);
            //���� �����Ϸ��� ��ġ�� �̹� ����Ʈ�� ������ �ٽ� �������� ������.
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnLoadUserInfo()
    { 
        //json �������� ������ ������ �����´�.
        //json ������ �Ľ��Ѵ�.
        //�Ľ��� �������� �������� Ŭ������ �����Ͽ� �����Ѵ�. 
    }
    Vector3 RandomPos()
    {
        float width = WorldManager.instance.width*0.5f;
        float height = WorldManager.instance.height*0.5f;

        float random_W = Random.Range(-width, width);
        float random_L = Random.Range(-width, width);
        float random_H = Random.Range(-height, height);
        Vector3 pos = new Vector3(random_W, random_H, random_L);
        return pos;
    }
}
