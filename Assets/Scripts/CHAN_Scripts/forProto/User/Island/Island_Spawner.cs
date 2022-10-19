using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//json���� �޾ƿ� �������� �ش� Ŭ������ �����Ѵ�. 
public class Island_Spawner : MonoBehaviour
{
    // ������ Ŭ���� ����
    IslandInformation islandInformation = new IslandInformation();
    
    List<GameObject> islands = new List<GameObject>();
    List<Vector3> islandsPos = new List<Vector3>();
    public Transform islandsTransForm;

    private void Start()
    {
        //�ϴ� csv���� �����ؼ� �ֿܼ� ���̵��� �غ���
        //key���� 2���� �����Ѵ� ���� �Ű澵�κ��� �ƴϴ� ���߿� key���� ���� �̸� ���ϱ�
        islandInformation.LoadFromCSV("subset30.csv");
        ForVerifyInfo();
        
        for (int i = 0; i < islandInformation.User_name.Count; i++)
        {
            // ����Ʈ�� �ִ� ��������  Key���� �����´�.
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
        //        //�׸��� ������ ��ġ�� ����Ʈ�� ���� 
        //        //islandsPos.Add(land.transform.position);
        //        //���� �����Ϸ��� ��ġ�� �̹� ����Ʈ�� ������ �ٽ� �������� ������.
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
    // �޾ƿ� ������ Ȯ�� ���� �Լ�
    void ForVerifyInfo()
    {
        for (int i = 2; i < islandInformation.island_Pos.Count + 2; i++)
        {
            print(islandInformation.island_Pos[i.ToString()] + " " + islandInformation.island_Type[i.ToString()] + " "
                + islandInformation.User_image[i.ToString()]);
        }
    }
}
