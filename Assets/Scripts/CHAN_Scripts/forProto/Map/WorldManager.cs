using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour 
{
    // �ϴø� ���� �������� �׼��� �����ϴ� ��ũ��Ʈ
    // * �����鿡�� �Ҵ���� ��ǥ���� �ش� �ʿ� ������ �� �ֵ��� ��������
    // * ������ �Ѱ輱�� ���ؾ� ��, �̸� �Ӽ����� �ο�
    // * �ð������� ���� ��

    //�Ӽ�: ������ �ʺ�, ����, �Ѱ輱 ��� �������� ����, ���� ����Ʈ , ��� �ؽ�Ʈ 
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
    //������ ���� ��ġ������ �޾Ƽ� �Ѱ輱�� �ִ��� Ȯ���ϴ� ����� �����ҰŴ�.
    // ���� ���� ��� ������ ������ �޴´�(���� �����±װ� �پ��ų� Ư�� ��ũ��Ʈ�� �ִ� ������Ʈ�� ã����. 
    // ���� ������ �� �Ϻΰ� �Ѱ輱 ���� n%������ ������
    //�������� ��� �޼��� ���� 
    void ControlLimitZone()
    {
        float warningZone_Width = width*(1-warningZoneRatio*0.01f)*0.5f;
        float warningZone_height = height * (1 - warningZoneRatio * 0.01f) * 0.5f;

        float DangerZone_width = width * 0.5f;
        float DangerZone_height = height * 0.5f;
        
        //���� ��ϵ� �����ο�����ŭ �ݺ�
        foreach (GameObject user in userObjs)
        {
            Vector3 userPos = user.transform.position;
            //������ ���� ������ �������� ��
            if (Mathf.Abs(userPos.x) > warningZone_Width || Mathf.Abs(userPos.z) > warningZone_Width || Mathf.Abs(userPos.y) > warningZone_height)
            {
                //�̶� ��� �޼����� ������. 
                warningText.enabled = true;
                warningText.text = "�ʿ��� ����� �־��!";
                if (Mathf.Abs(userPos.x)> DangerZone_width || Mathf.Abs(userPos.z) > DangerZone_width || Mathf.Abs(userPos.y) > DangerZone_height)
                {
                    //�ϴ��� �������� �ٽ� ���� ������
                    user.transform.position = Vector3.zero;
  
                }
                
            }
            else
            {
                warningText.enabled = false;
            }
        }
            
        
    }
    #region �ܺο��� ������ �α���or �α׾ƿ� ���� ��, �ش��Լ��� ȣ��ȴ�. 
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
