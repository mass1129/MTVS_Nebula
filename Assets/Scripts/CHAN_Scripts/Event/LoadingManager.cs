using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    //�ش� Ŭ������ �ε��� ���� Ŭ������ 
    //������Ʈ �������� �ҷ����������� ��ũ��Ʈ �󿡼� �ٷ� �ҷ��� �� �ֵ��� ����� ����
    // �Ӽ�
    // �ε� �÷���, �̹���
    public bool isLoadingDone;
    public GameObject LoadingIcon;
    public Image LoadingImage;
    void Start()
    {
        
    }

    // Update is called once per frame

    IEnumerator ShowLoading()
    {
        //�ش� �ڷ�ƾ�� �������ڸ��� ��ü ��ũ���� fadeIn fadeOut �Ѵ�.
        //�׸��� �ε� �̹����� �����ش�. 
        //�ε� ������ ���� �� ���� ��� �ݺ��ؾ� ��
        while (!isLoadingDone)
        {
            //���⼭�� �ε��ٰ� ���ư��� ����� ���� 
            LoadingIcon.SetActive(true);
            yield return null;

        }
        //�ε��� ������ �ݺ��� �����. 
        
    }
}
