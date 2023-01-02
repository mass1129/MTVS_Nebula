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
  - **Inventory 데이터 Post/Get 구조도**  
     <img src="Image/InventoryPostGet1.png" width="800px">  
  - **Inventory 전체 구조도**  
     <img src="Image/inventoryAllStructure.png" width="800px">  
## UI - Inventory  
 - 개요  
    - 추상클래스를 상속받음으로서 **슬롯 세팅 - CreateSlots()** / **슬롯 Action - OnSlotUpdate(InventorySlot)** 를 자식 클래스에서 반드시 구현하도록 함.  
    - 또 한가지 이유는 UI type 별로 위에 기술한 두가지 메소드가 다르기 때문이다. virtual로 하기에는 for문 안에 코드가 다른 것들을 다시 for문으로 돌려주기에는 부담스러웠다.  
    - 세팅시 슬롯마다 등록하는 UI이벤트 메소드들은 자식 클래스에서 공통으로 사용하므로 추상클래스에 선언 후 자식클래스에서 구현하는 CreateSlots()에서 사용  
      ![Diagram](https://github.com/mass1129/MTVS_Nebula/blob/mass7/Image/testDiagram.drawio.png)
 - **K_UserInterface.cs : 추상 클래스**  
    -  **abstract Function**  
        -  void CreateSlots() : 슬롯 세팅  
        -  void OnSlotUpdate(InventorySlot slot) : 슬롯 업데이트시 호출되는 Action  
    -  **Function**  
        -  void AddEvent(GameObject obj, EventTriggerType type, UnityAction\<BaseEventData> action) : 오브젝트(슬롯 or UI창), EventTriggerType, Action를 매개변수로 받아서 이벤트를 등록하는 메소드  
    -  **Action** : MouseData라는 static클래스에 필드 값을 EventTriggerType 마다 설정한다.  
        - **UI 창에 적용** 
        - void OnEnterInterface(GameObject obj) : 마우스가 UI상에 있을 때 MouseData.interfaceMouseIsOver = 해당 UI의 K_UserInterface  
        - void OnExitInterface(GameObject obj) : 마우스가 UI에서 이탈할 때 MouseData.interfaceMouseIsOver = null  
        - **슬롯에 적용**
        - void OnEnter(GameObject obj) : 마우스가 슬롯 안에 있을 때 MouseData.slotHoveredOver = obj  
        - void OnExit(GameObject obj) : 마우스가 슬롯에서 나갈 때 MouseData.slotHoveredOver = null  
        - void OnDragStart(GameObject obj) : 슬롯 드래그 시작할 때 MouseData.tempItemBeingDragged = CreateTempItem(obj) => 슬롯 이미지 복사본 생성  
        - void OnDrag(GameObject obj) : 슬롯 드래그 중일 때 슬롯 이미지 복사본 마우스 커서 위치로 세팅  
        - void OnDragEnd(GameObject obj) : 드래그 종료시 복사본 삭제, 마우스가 슬롯 위에 있을시 슬롯 스왑, 마우스가 UI 밖에 있을때 아이템 포기  
        - GameObject CreateTempItem(GameObject obj) : 이미지 복사본 오브젝트를 생성 기능  
- **자식 클래스** : 장비, 옷, 빌딩 퀵슬롯, 상점 UI  

|   |  <center>K_EquipmentInterface</center> |  <center>K_ClothesInterface</center> |<center>K_BuildingQuickSlotInterface</center> |  <center>K_ShopListInterface</center> |
|:--------:|:--------:|:--------:|:--------:|:--------:|
|**등록 이벤트 개수** | <center>7 </center> | <center>7 </center> |<center>0 </center> |<center>0 </center> |
|**슬롯 업데이트 Action** | <center>img 업뎃 + 디폴드 img on/off </center> |<center>img 업뎃 </center> |<center>img 업뎃 </center> |<center>전용 슬롯 업뎃 함수, 버튼 onClick 등록</center> |
|**슬롯 parent설정 여부** | <center> X </center> |<center>O </center>|<center>O </center> |<center>X </center> |
|**비고** | <center> - </center> |<center>- </center>|<center>- </center> |<center>상점 시스템 참조 </center> |
  
## 게임 머니 및 상점 시스템  
  
## 월드 꾸미기



# 프로젝트 종료 이후 (22.12.2~) 이슈 관리 및 개선 사항  
### (photon) master client 변경시 건물이 중복 생성 및 삭제 불가능한 이슈 [#1](https://github.com/mass1129/MTVS_Nebula/issues/1)  
### 유저월드에 입장할때 먼저 입장한 유저의 아바타가 업데이트가 안되는 이슈 [#2](https://github.com/mass1129/MTVS_Nebula/issues/2)  
### 인벤토리 저장할 때 "부정한 행위가 발생하였습니다"라는 네트워크 오류가 발생하는 이슈 [#3](https://github.com/mass1129/MTVS_Nebula/issues/3)  
### 아이템 시스템 관련 코드 리펙토링 [#4](https://github.com/mass1129/MTVS_Nebula/issues/4)  
### 빌딩시스템 코드 리펙토링 [#5](https://github.com/mass1129/MTVS_Nebula/issues/5)  
### UI관련 코드 리펙토링  [#6](https://github.com/mass1129/MTVS_Nebula/issues/6)
### HTTP 통신 모듈 개선 [#7](https://github.com/mass1129/MTVS_Nebula/issues/7)
### Photon Multiplay 개선 작업 [#8](https://github.com/mass1129/MTVS_Nebula/issues/8)
