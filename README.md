# MTVS_Nebula
  - 메타버스 아카데미 최종 프로젝트(2022.10.4 ~ 2022.12.1)  
  - 🏆 최종 성과공유회 장려상(한국전파진흥협회장)  
  - [프로젝트 pdf](https://github.com/mass1129/MTVS_Nebula/blob/mass7/Image/52Hertz_Nebula_MTVS.pdf) 

## 애플리케이션 개요
  - 메타버스 소셜 네트워크 서비스  
  <img src="Image/ppt_intro.jpg" width="800px">  

## 애플리케이션 시연 영상
[![Video Label](http://img.youtube.com/vi/CWq_pdrmocA/0.jpg)](https://youtu.be/CWq_pdrmocA)
      

## 팀 구성 및 역할
<img src="Image/ppt_team.jpg" width="800px">  

## 기술 융합 구조도
<img src="Image/ppt_xrStructure.jpg" width="800px">  

----
# 주요 구현 요소(김혜성) : 유저 월드 담당

## HTTP 통신 모듈 제작  
  - [Assets/Scripts/K_Script/Http](https://github.com/mass1129/MTVS_Nebula/tree/mass7/Assets/Scripts/K_Script/Http)  
  - Unitask를 활용한 비동기 통신 구조   
    ```C#
    public async UniTask Post(string url, string json)
    ```
  - 어디서든 인스턴스화 가능한 모듈  
    ```C#
    //인스턴스화 예시  
    var httpReq = new HttpRequester(new JsonSerializationOption());
    await httpReq.Post(url, json);
    ```
  - Generic를 사용하여 코드 재사용  
    ```C#
    public async UniTask<TResultType> Get<TResultType>(string url)
    ```
    예시  
    ```C#
    //어떤 클래스든 역직렬화 가능
    H_I_Root result2 = await httpReq.Get<H_I_Root>(url);
    Inventory newInven = result2.results;
    ```
  - 인터페이스를 통한 전략 패턴으로 이후 시스템 확장시 ContentType과 직렬화/역직렬화 방식을 유연하게 수정할 수 있도록 함  
    ```C#
    public interface ISerializationOption
    {   
        string ContentType { get; }
        T Deserialize<T>(string text);
    }
    ```
## 아이템 시스템  
  - [Assets/Scripts/K_Script/ItemSystem](https://github.com/mass1129/MTVS_Nebula/tree/mass7/Assets/Scripts/K_Script/ItemSystem)  
  - 유니티의 Scriptable Object를 활용하여 설계  
      - 유니티에서 제공하는 대량의 데이터를 저장하는 데이터 컨테이너  
      - 인스턴스화 될때 데이터에 대한 사본을 생성하지 않고 메모리에 Scriptable Object의 데이터 사본만을 저장하고 이를 참조하는 방식 -> 메모리 사용 줄일 수 있다.  
  - **ItemObject.cs : Scriptable Object** : 아이템의 모든 정보를 저장한 스크립터블 오브젝트  
    
      -  직렬화하는 정보는 Data(item 클래스)뿐 나머지 정보는 클라이언트에 에셋 형태로 저장  
          <img src="Image/itemobject.png" width="800px">  

## 월드 꾸미기

## 게임 머니 및 상점 시스템
