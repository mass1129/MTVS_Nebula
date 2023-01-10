using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Cysharp.Threading.Tasks;


public class GridBuildingSystem3D : MonoBehaviour
{

    public static GridBuildingSystem3D Instance { get; private set; }
    //배치하고자 하는 빌딩 바뀔때 Event
    public event EventHandler OnSelectedChanged;
    //건물을 배치할 그리드
    private GridXZ<GridObject> selectedGrid;

    //배치할 건물
    private PlacedObjectTypeSO placedObjectTypeSO;
    //배치할 건물의 rotation enum
    private PlacedObjectTypeSO.Dir dir;

    public InventoryObject quickSlot;

    public BuildingSystemAssetsDatabase buildingAssetDatabase;

    public string fileName;
    public void BuildingSetup()
    {
        quickSlot.Clear();
        Instance = this;
        //그리드 속성 값 세팅
        int gridWidth = 100;
        int gridHeight = 100;
        float cellSize = 1f;
        //위의 속성으로 그리드 세팅
        //GridXZ<GridObject> 생성자의 매개변수인 Func<GridXZ<TGridObject>, int, int, TGridObject>대리자를 아래와 같이 해서 GridXZ<GridObject>객체를 생성한다.
        //Func<GridXZ<TGridObject>, int, int, TGridObject> createGridObject = (GridXZ<GridObject> g, int x, int y) => new GridObject(g, x, y) 
        selectedGrid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), () => new GridObject());

        placedObjectTypeSO = null;
    }


    private void Update()
    {
        HandleNormalObjectPlacement();
        HandleDirRotation();
        HandleDemolish();
        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeselectObjectType();
        }

    }

    private void OnDisable()
    {
        DeselectObjectType();
    }


    //건물을 배치하는 메소드
    private void HandleNormalObjectPlacement() 
    {
        if (Input.GetMouseButtonDown(0) && placedObjectTypeSO != null && !UtilsClass.IsPointerOverUI())
        {
            //마우스 위치 Get
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
            //마우스 위치로 그리드 좌표 Get(내림)
            selectedGrid.GetVertext(mousePosition, out Vector2Int gridPosition);
            //건물 배치 시도
            if (TryPlaceObject(placedObjectOrigin : gridPosition, placedObjectTypeSO, dir, out PlacedObject placedObject))
            {
                //성공시 선택중인 건물 null
                DeselectObjectType();
            } 
            else 
            {
                //실패시 자리 없음 출력
                UtilsClass.CreateWorldTextPopup("자리가 없습니다!", mousePosition);
            }
        }
    }
    //배치할 건물 선택 메소드
    public void HandleTypeSelect(int i) 
    {   
        //퀵슬롯의 슬롯위치로 건물 get 후 선택한 건물에 할당
        placedObjectTypeSO = quickSlot.GetSlots[i].GetItemObject().matchToBuildingSO; 
        //이벤트 발동 -> BuildingGhost(실제 건물을 배치전 가상 건물 생성 클래스)의 메소드 실행(가상 건물 종류 바꾸기)
        RefreshSelectedObjectType();
    }

    //배치할 건물 회전값 바꾸기 메소드
    private void HandleDirRotation() {
        if (Input.GetKeyDown(KeyCode.R)) {
            //필드 dir값을 90도 회전시킨 값으로 할당
            dir = PlacedObjectTypeSO.GetNextDir(dir);
        }
    }

    //건물 삭제 메소드
    private void HandleDemolish() {
        if (Input.GetMouseButtonDown(1) && !UtilsClass.IsPointerOverUI()) 
        {
            //마우스 위치를 가져와서
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
            //그리드의 그리드 오브젝트에 PlacedObject를 가져온다
            PlacedObject placedObject = selectedGrid.GetGridObject(mousePosition).GetPlacedObject();
            //배치된 PlacedObject이 있다면
            if (placedObject != null) 
            {
                //해당 건물이 차지하고 있는 좌표 리스트를 가져와서
                List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                //좌표 리스트의 그리드오브젝트들에 할당된 건물을 null값으로 바꿔준다.
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    GetGridObject(gridPosition).ClearPlacedObject();
                }
                // 이후 PlacedObject 파괴
                placedObject.DestroySelf();
            }
        }
    }

    //선택중인 건물을 해제하는 메소드
    private void DeselectObjectType() {
        placedObjectTypeSO = null;
        dir = default;
        RefreshSelectedObjectType();
    }
    //OnSelectedChanged에 참조된 BuildingGost의 메소드 호출 
    private void RefreshSelectedObjectType() 
    {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }


    //그리드 좌표, 배치 건물 종류, 건물 방향를 받아서 PlacedObject를 반환한다.
    public bool TryPlaceObject(Vector2Int placedObjectOrigin, PlacedObjectTypeSO placedObjectTypeSO, PlacedObjectTypeSO.Dir dir, out PlacedObject placedObject) {
        // 배치할 건물의 기준점과 방향으로 배치할 그리드 좌표 리스트를 가져온다.
        List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);
        //일단 배치 가능으로 설정
        bool canBuild = true;
        //배치할 그리드 좌표 리스트를 검사한다.
        foreach (Vector2Int gridPosition in gridPositionList) {
            //먼저 좌표가 그리드 전체 범위안에 있는지, 그리드오브젝트에 건물이 할당되어있는지 확인한다.
            if (!selectedGrid.IsValidGridPosition(gridPosition) || !GetGridObject(gridPosition).CanBuild())  {
                canBuild = false;
                break;
            }
        }
        //배치 가능하다면
        if (canBuild) {
            //해당 건물의 rotationOffset를 참조해서
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            //건물 생성 위치를 만들고
            Vector3 placedObjectWorldPosition = selectedGrid.GetWorldPosition(placedObjectOrigin) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * selectedGrid.GetCellSize();
            //건물 생성
            placedObject = PlacedObject.Create(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO);
            //건물이 배치된 그리드 좌표들에 대해서
            foreach (Vector2Int gridPosition in gridPositionList) {
                //각 그리드 오브젝트들에 해당 건물을 할당한다.
                GetGridObject(gridPosition).SetPlacedObject(placedObject);
            }                       
            return true;
        } 
        //배치할수없다면
        else {
            //null값과 false 반환
            placedObject = null;
            return false;
        }
    }

    //그리드 오브젝트를 가져오는 메소드
    public GridObject GetGridObject(Vector2Int gridPosition) {
        return selectedGrid.GetGridObject(gridPosition.x, gridPosition.y);
    }

    //BuildingGhost의 타겟 위치를 반환하는 메소드
    public Vector3 GetMouseWorldSnappedPosition() {
        //마우스 커서위치를 참조해서
        Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
        //그리드 위 2차원 좌표 반환
        selectedGrid.GetVertext(mousePosition, out Vector2Int gridPosition);
        //선택중인 건물이 있다면 
        if (placedObjectTypeSO != null) {
            //건물 회전값에 따라서 위치를 반환한다.
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = selectedGrid.GetWorldPosition(gridPosition) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * selectedGrid.GetCellSize();
            return placedObjectWorldPosition; } 
        //선택중이 건물이 없다면 마우스 위치 반환
        else {
            return mousePosition;
        }
    }
    //BuildingGhost의 타겟 회전값을 반환하는 메소드
    public Quaternion GetPlacedObjectRotation() {
        if (placedObjectTypeSO != null) {
            return Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
        } else {
            return Quaternion.identity;
        }
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
        return placedObjectTypeSO;
    }

    
    public async UniTaskVoid BuildingSave(string s, bool fileSave = true)
    {
        //전체 층 세이브 데이터 리스트 선언 및 초기화
        List<SaveFloorBuilding> placedObjectSaveObjectArrayList = new List<SaveFloorBuilding>();
        //PlacedObject의 직렬화 데이터  리스트 선언 및 초기화
        List<PlacedObject.SaveObject> saveObjectList = new List<PlacedObject.SaveObject>();
        //placedObject는 생성 혹은 삭제시 K_UserWorldMg 필드에 있는 리스트에 더해지기 때문에 해당 데이터 참조
        List<PlacedObject> savedPlacedObjectList = K_UserWorldMgr.instance.loadObjectList;
        //PlacedObject 자체는 직렬화 객체가 아니기 때문에 PlacedObject의 직렬화 객체를 생성 후 직렬화 데이터  리스트에 더한다.
        foreach (PlacedObject obj in savedPlacedObjectList)
        {
            saveObjectList.Add(obj.GetSaveObject());
        }

        //층 하나의 세이브 데이터 선언 및 할당하고
        SaveFloorBuilding placedObjectSaveObjectArray = new SaveFloorBuilding { gridPlaceObjectList = saveObjectList };
        //전체 층 세이브 데이터 리스트에 더해준다.
        placedObjectSaveObjectArrayList.Add(placedObjectSaveObjectArray);
        //전체 층 세이브 데이터 객체를 생성하고 
        SaveAllBuilding saveObject = new SaveAllBuilding
        {
            //전체 층 세이브 데이터를 할당해준다
            islandGridList = placedObjectSaveObjectArrayList
        };

        string json = JsonUtility.ToJson(saveObject, true);
        if(fileSave)
        {
            K_SaveSystem.Save(fileName, json, true);
            return;
        }
        else
        {
            var url = "https://resource.mtvs-nebula.com/skyisland/" + s;
            var httpReq = new HttpRequester(new JsonSerializationOption());
            await httpReq.Post(url, json);
        }
        

    }

    public async UniTaskVoid BuildingLoad(string s, bool fileSave = true)
    {
        if(fileSave)
        {
            SaveAllBuilding result = K_SaveSystem.LoadObject<SaveAllBuilding>(fileName);
            for (int i = 0; i < result.islandGridList.Count; i++)
            {
                foreach (PlacedObject.SaveObject placedObjectSaveObject in result.islandGridList[i].gridPlaceObjectList)
                {

                    PlacedObjectTypeSO placedObjectTypeSO = buildingAssetDatabase.GetPlacedObjectTypeSOFromName(placedObjectSaveObject.placedObjectTypeSOName);

                    TryPlaceObject(placedObjectSaveObject.origin, placedObjectTypeSO, placedObjectSaveObject.dir, out PlacedObject placedObject);

                }
            }
            return;
        }

        var url = "https://resource.mtvs-nebula.com/skyisland/" + s;
        var httpReq = new HttpRequester(new JsonSerializationOption());

        H_Building_Root result2 = await httpReq.Get<H_Building_Root>(url);


        for (int i = 0; i < result2.results.placeObjects.islandGridList.Count; i++)
        {
            foreach (PlacedObject.SaveObject placedObjectSaveObject in result2.results.placeObjects.islandGridList[i].gridPlaceObjectList)
            {

                PlacedObjectTypeSO placedObjectTypeSO = buildingAssetDatabase.GetPlacedObjectTypeSOFromName(placedObjectSaveObject.placedObjectTypeSOName);

                TryPlaceObject(placedObjectSaveObject.origin, placedObjectTypeSO, placedObjectSaveObject.dir, out PlacedObject placedObject);

            }
        }

    }

    public async UniTaskVoid FirstBuildingLoad()
    {
        BuildingLoad(PlayerPrefs.GetString("User_Island_ID")).Forget();
        await UniTask.Yield();
        
        

    }

   
    [Serializable]
    public class SaveAllBuilding
    {
        public List<SaveFloorBuilding> islandGridList;
    }

    [Serializable]
    public class SaveFloorBuilding
    {

        public List<PlacedObject.SaveObject> gridPlaceObjectList;
    }



 
    public class H_SaveFloorBuilding
    {
        public List<PlacedObject.SaveObject> gridPlaceObjectList { get; set; }
    }


    public class H_SaveAllBuilding
    {
        public List<H_SaveFloorBuilding> islandGridList { get; set; }
    }




    public class H_Building_Buildings
    {
        public int id { get; set; }
        public string owner { get; set; }
        public H_SaveAllBuilding placeObjects { get; set; }
        public object dropItems { get; set; }
    }

    public class H_Building_Root
    {
        public int httpStatus { get; set; }
        public string message { get; set; }
        public H_Building_Buildings results { get; set; }
    }



    
}
