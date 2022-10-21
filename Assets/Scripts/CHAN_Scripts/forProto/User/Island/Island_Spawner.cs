using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//json���� �޾ƿ� �������� �ش� Ŭ������ �����Ѵ�. 
public class Island_Spawner : MonoBehaviour
{
    // ������ Ŭ���� ����
    //IslandInformation islandInformation = new IslandInformation();
    //List<GameObject> islands = new List<GameObject>();
    public Transform islandsTransForm;
    public string CSV_File_Name;
    public string CSV_File_Name2;
    GameObject islands;

    private void Start()
    {
       

    }
    //�� ���� �������� �ϴ� Ʈ���� �Լ�
    public void InitializeIsland()
    {
        //�ϴ� csv���� �����ؼ� �ֿܼ� ���̵��� �غ���
        //key���� 2���� �����Ѵ� ���� �Ű澵�κ��� �ƴϴ� ���߿� key���� ���� �̸� ���ϱ�
        IslandInformation.instance.LoadFromCSV(CSV_File_Name+".csv");
        islands = new GameObject("Islands");
        for (int i = 0; i < IslandInformation.instance.User_name.Count; i++)
        {
            // ����Ʈ�� �ִ� ��������  Key���� �����´�.
            string userName = IslandInformation.instance.User_name[i];
            GameObject land = Instantiate(Resources.Load<GameObject>("CHAN_Resources/" + IslandInformation.instance.island_Type[userName]), islands.transform);
            land.transform.localScale *= 0.5f;
            //������ �ϴü����� �̸��� �ο�
            land.transform.GetChild(0).GetChild(0).GetComponent<Island_Profile>().user_name = userName;
            land.transform.position = IslandInformation.instance.island_Pos[userName];
            IslandInformation.instance.UserObj.Add(userName, land);

            //        //land.transform.localScale *=0.5f ;
            //        //�׸��� ������ ��ġ�� ����Ʈ�� ���� 
            //        //���� �����Ϸ��� ��ġ�� �̹� ����Ʈ�� ������ �ٽ� �������� ������.
        }
    }
    public void ReLoadIsland()
    {
        IslandInformation.instance.UserObj.Clear();
        Destroy(islands);
        islands = new GameObject("Islands");
        IslandInformation.instance.LoadFromCSV(CSV_File_Name2 + ".csv");
        for (int i = 0; i < IslandInformation.instance.User_name.Count; i++)
        {
            // ����Ʈ�� �ִ� ��������  Key���� �����´�.
            string userName = IslandInformation.instance.User_name[i];
            GameObject land = Instantiate(Resources.Load<GameObject>("CHAN_Resources/" + IslandInformation.instance.island_Type[userName]), islands.transform);
            land.transform.localScale *= 0.5f;
            //������ �ϴü����� �̸��� �ο�
            land.transform.GetChild(0).GetChild(0).GetComponent<Island_Profile>().user_name = userName;
            land.transform.position = IslandInformation.instance.island_Pos[userName];
            IslandInformation.instance.UserObj.Add(userName, land);

            //        //land.transform.localScale *=0.5f ;
            //        //�׸��� ������ ��ġ�� ����Ʈ�� ���� 
            //        //���� �����Ϸ��� ��ġ�� �̹� ����Ʈ�� ������ �ٽ� �������� ������.
        }
    }
    //���� ��ǥ�� ������Ű�� �Լ�
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
    //void ForVerifyInfo()
    //{
    //    for (int i = 2; i < IslandInformation.instance.island_Pos.Count + 2; i++)
    //    {
    //        print(IslandInformation.instance.island_Pos[i.ToString()] + " " + IslandInformation.instance.island_Type[i.ToString()]
    //            + IslandInformation.instance.User_image[i.ToString()]);
    //    }
    //}
}
