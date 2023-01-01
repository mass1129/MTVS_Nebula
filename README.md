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
  - 인터페이스를 통한 전략 패턴으로 종속성 삭제 및 이후 시스템 확장시 ContentType과 직렬화/역직렬화 방식을 유연하게 수정할 수 있도록 함  
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
      - 인스턴스화 될때 데이터에 대한 사본을 생성하지 않고 메모리(힙)에 Scriptable Object의 데이터 사본만을 저장하고 이를 참조하는 방식 -> 메모리 사용 줄일 수 있다.  
      - json 정적 데이터를 런타임에서 바로 읽는것이 아니라 SCriptable Object에 먼저 파싱하고 읽음으로서 성능 최적화 효과  
  - **ItemObject.cs : Scriptable Object** : 아이템의 모든 정보를 저장한 스크립터블 오브젝트  
    
      -  직렬화하는 정보는 Data(item 클래스)뿐 나머지 정보는 클라이언트에 에셋 형태로 저장  
          <img src="Image/itemobject.png" width="800px">  
   
  - **ItemDatabaseObject.cs : Scriptable Object** : 모든 아이템 Scriptable Object를 배열로 저장해 놓은 아이템 데이터베이스  
    - 아이템를 만들고 데이터베이스에 추가시 id가 자동으로 업데이트(유니티에서 제공하는 OnValidate() 메소드 사용, 또는 인스펙터에서 Update ID's 클릭)    
  - **Item.cs** : 서버와 주고 받을 직렬화 아이템 데이터 클래스  
    - 생성자  
      - Item() : 아이템 초기화(id = -1 => item = null)
      - Item(ItemObject item) : 아이템 Scriptable Object 데이터로 세팅된 아이템 생성  
      
  - **InventorySlot.cs** : 인벤토리를 구성하는 단위   
    - **field**
      - 직렬화 데이터 : 해당 슬롯에 있는 아이템과 해당 아이템 개수, 저장할수있는 아이템 타입 배열
      - 기타 속성 : 관련 UI, 슬롯 디스플레이, 슬롯 업데이트 전후 Action 대리자  
    - **Method**  
      - **ItemObject GetItemObject()** : 아이템의 id로 아이템 데이터 베이스에 등록된 ItemObject를 찾아서 반환  
      - **void UpdateSlot(Item itemValue, int amountValue)** : **슬롯을 업데이트하는 핵심 메소드**    
        ```C#
        public void UpdateSlot(Item itemValue, int amountValue)
        {
            onBeforeUpdated?.Invoke(this); //슬롯 업데이트 전 Action (ex. 기존 장비 벗기, 기존 아이템에 의한 능력치 제거) 
            item = itemValue;
            amount = amountValue;  //슬롯 아이템 변경  
            onAfterUpdated?.Invoke(this); //슬롯 업데이트 후 Action   (ex. 새로운 장비 입기, 새 아이템에 의한 능력치 추가)
        }
        ```
      - **bool CanPlaceInSlot(ItemObject itemObject)** : 아이템 드래그&드롭시 해당 슬롯에 배치할 수 있는지 여부 체크  
  
  - **InventoryObject.cs : Scriptable Object** : 인벤토리의 정보를 저장하는 Scriptable Object  
    - **field**  
      - 직렬화 데이터 : Inventory 클래스(InventorySlot[]로 이루어짐)  
          ```C#
          [SerializeField]
          private Inventory Container = new Inventory(); // 에디터상에서 인벤토리 Scriptable Object 생성시에만 호출  

          public InventorySlot[] GetSlots => Container.slots; //외부에서 인벤토리 슬롯에 접근하기 위한 변수 
          ```
      - 기타 속성 : Save & Load api 호출 경로, ItemDatabaseObject, UI타입  
    - **Method**  
      - **void AddBundleListToWindow(ItemObject[] bundleList)** : ItemObject.cs의 subItem[]배열를 받아 빌딩번들 리스트(InventoryObject로 관리)에 추가  
      - **void SwapItems(InventorySlot dragExitSlot, InventorySlot dragStartSlot)** : UI상에서 드래그&드롭시 슬롯 위치 변경  
      - **Inventory GetInventory()** : 직렬화 데이터(Inventory) return - equipment와 clothes 인벤토리는 데이터 멱등성 유지를 위해 함께 저장하는데 이때 필요  
      - **void UpdateInventory()** : 인벤토리가 로드되고 슬롯 Action에 관련 메소드가 등록되면(UI와 장비에서) 해당 메소드가 스킵되기 때문에 Action에 메소드를 등록후 해당 메소드를 호출하기 위해 만든 메소드  
      - **async UniTask InventorySave(string s)** : 비동기 인벤토리 세이브 메소드  
      - **async UniTask InventoryLoad(string s)** : 비동기 인벤토리 로드 메소드   
      - **async UniTask ForGiveItem(InventorySlot dropSlot, string avatarName)** : 비동기 아이템 소유권 포기 후 인벤토리 로드 메소드(데이터 멱등성 유지)    
      
  - **Inventory.cs** : 서버와 주고 받을 인벤토리 데이터 클래스(InventorySlot[]으로 구성)    








## 월드 꾸미기

## 게임 머니 및 상점 시스템

# 이슈 관리 및 개선 사항  
### (photon) master client 변경시 건물이 중복 생성 및 삭제 불가능한 이슈 [#1](https://github.com/mass1129/MTVS_Nebula/issues/1)  
### 유저월드에 입장할때 먼저 입장한 유저의 아바타가 업데이트가 안되는 이슈 [#2](https://github.com/mass1129/MTVS_Nebula/issues/2)  
### 인벤토리 저장할 때 "부정한 행위가 발생하였습니다"라는 네트워크 오류가 발생하는 이슈 [#3](https://github.com/mass1129/MTVS_Nebula/issues/3)  
### 아이템 시스템 관련 코드 리펙토링 [#4](https://github.com/mass1129/MTVS_Nebula/issues/4)  
### 빌딩시스템 코드 리펙토링 [#5](https://github.com/mass1129/MTVS_Nebula/issues/5)  
### UI관련 코드 리펙토링  [#6](https://github.com/mass1129/MTVS_Nebula/issues/6)
### HTTP 통신 모듈 개선 [#7](https://github.com/mass1129/MTVS_Nebula/issues/7)
### Photon Multiplay 개선 작업 [#8](https://github.com/mass1129/MTVS_Nebula/issues/8)
