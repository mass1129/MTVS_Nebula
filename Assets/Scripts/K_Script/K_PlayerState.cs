using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ~OwnedStates 네임스페이스의 클래스들(에이전트의 동작 클래스들)에 상속되는 State 추상 클래스
public abstract class K_PlayerState<T> where T : class  // T에는 클래스만 상속 가능. GM_Hunter 에이전트 클래스가 한정 매개변수로 지정된다.
{
    // 해당 상태 시작할 때 1회 호출
    public abstract void Enter(T entity);

    // 해당 상태 업데이트할 때 매 프레임 호출
    public abstract void Execute(T entity);

    // 해당 상태 종료할 때 1회 호출
    public abstract void Exit(T entity);
}

