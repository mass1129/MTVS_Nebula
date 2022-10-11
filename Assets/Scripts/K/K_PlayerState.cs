using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ~OwnedStates ���ӽ����̽��� Ŭ������(������Ʈ�� ���� Ŭ������)�� ��ӵǴ� State �߻� Ŭ����
public abstract class K_PlayerState<T> where T : class  // T���� Ŭ������ ��� ����. GM_Hunter ������Ʈ Ŭ������ ���� �Ű������� �����ȴ�.
{
    // �ش� ���� ������ �� 1ȸ ȣ��
    public abstract void Enter(T entity);

    // �ش� ���� ������Ʈ�� �� �� ������ ȣ��
    public abstract void Execute(T entity);

    // �ش� ���� ������ �� 1ȸ ȣ��
    public abstract void Exit(T entity);
}

