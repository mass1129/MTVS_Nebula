using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Photon.Pun;
using Cysharp.Threading.Tasks;
using Photon.Realtime;

public class GridBuildingSystem3D : MonoBehaviourPun, IPunObservable
{

    public static GridBuildingSystem3D Instance { get; private set; }

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;

    private const float GRID_HEIGHT = 2.5f;
    
    private List<GridXZ<GridObject>> gridList;
    private GridXZ<GridObject> selectedGrid;

   
    private PlacedObjectTypeSO placedObjectTypeSO;
    private PlacedObjectTypeSO.Dir dir;

    private bool isDemolishActive;
    public InventoryObject quickSlot;
    private void Awake() 
    {
        


    }
 
    public void BuildingSetup()
    {
        quickSlot.Clear();
        Instance = this;
        //그리드 세팅
        int gridWidth = 100;
        int gridHeight = 100;
        float cellSize = 1f;
        gridList = new List<GridXZ<GridObject>>();
        int gridVerticalCount = 1;
        float gridVerticalSize = GRID_HEIGHT;
        for (int i = 0; i < gridVerticalCount; i++)
        {
            GridXZ<GridObject> grid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, gridVerticalSize * i, 0), (GridXZ<GridObject> g, int x, int y) => new GridObject(g, x, y));
            gridList.Add(grid);
        }

        placedObjectTypeSO = null;
        selectedGrid = gridList[0];
    }


    private void Start()
    {

    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        HandleNormalObjectPlacement();
        HandleDirRotation();
        HandleDemolish();
        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeselectObjectType();
        }

    }


    public class GridObject {

        private GridXZ<GridObject> grid;
        private int x;
        private int y;
        public PlacedObject placedObject;

        public GridObject(GridXZ<GridObject> grid, int x, int y) 
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
            placedObject = null;
        }

        public override string ToString() {
            return x + ", " + y + "\n" + placedObject;
        }

        public void TriggerGridObjectChanged() {
            grid.TriggerGridObjectChanged(x, y);
        }

        public void SetPlacedObject(PlacedObject placedObject) {
            this.placedObject = placedObject;
            TriggerGridObjectChanged();
        }

        public void ClearPlacedObject() {
            placedObject = null;
            TriggerGridObjectChanged();
        }

        public PlacedObject GetPlacedObject() {
            return placedObject;
        }

        public bool CanBuild() {
            return placedObject == null;
        }

    }

    

    private void HandleNormalObjectPlacement() 
    {
        if (Input.GetMouseButtonDown(0) && placedObjectTypeSO != null && !UtilsClass.IsPointerOverUI())
        {
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
            selectedGrid.GetXZ(mousePosition, out int x, out int z);

            Vector2Int placedObjectOrigin = new Vector2Int(x, z);

            if (TryPlaceObject(placedObjectOrigin, placedObjectTypeSO, dir, out PlacedObject placedObject))
            {
                DeselectObjectType();
            } 
            else 
            {
                // Error!
                UtilsClass.CreateWorldTextPopup("자리가 없습니다!", mousePosition);
            }
        }
    }

    public void HandleTypeSelect(int i) 
    {
        placedObjectTypeSO = quickSlot.GetSlots[i].GetItemObject().matchToBuildingSO; RefreshSelectedObjectType();
    }

    private void HandleDirRotation() {
        if (Input.GetKeyDown(KeyCode.R)) {
            dir = PlacedObjectTypeSO.GetNextDir(dir);
        }
    }

    private void HandleDemolish() {
        if (Input.GetMouseButtonDown(1) && !UtilsClass.IsPointerOverUI()) 
        {
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
            PlacedObject placedObject = selectedGrid.GetGridObject(mousePosition).GetPlacedObject();
            if (placedObject != null) 
            {
                // Demolish
                placedObject.DestroySelf(true);

                List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    selectedGrid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                }
            }
        }
    }

    private void DeselectObjectType() {
        placedObjectTypeSO = null;
        isDemolishActive = false;
        RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType() 
    {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

   

    public bool TryPlaceObject(int x, int y, PlacedObjectTypeSO placedObjectTypeSO, PlacedObjectTypeSO.Dir dir) {
        return TryPlaceObject(new Vector2Int(x, y), placedObjectTypeSO, dir, out PlacedObject placedObject);
    }

    public bool TryPlaceObject(Vector2Int placedObjectOrigin, PlacedObjectTypeSO placedObjectTypeSO, PlacedObjectTypeSO.Dir dir) {
        return TryPlaceObject(placedObjectOrigin, placedObjectTypeSO, dir, out PlacedObject placedObject);
    }

    public bool TryPlaceObject(Vector2Int placedObjectOrigin, PlacedObjectTypeSO placedObjectTypeSO, PlacedObjectTypeSO.Dir dir, out PlacedObject placedObject) {
        // Test Can Build
        List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);
        bool canBuild = true;
        foreach (Vector2Int gridPosition in gridPositionList) {
            //bool isValidPosition = grid.IsValidGridPositionWithPadding(gridPosition);
            bool isValidPosition = selectedGrid.IsValidGridPosition(gridPosition);
            if (!isValidPosition) {
                // Not valid
                canBuild = false;
                break;
            }
            if (!selectedGrid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                canBuild = false;
                break;
            }
        }

        if (canBuild) {
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = selectedGrid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * selectedGrid.GetCellSize();

            placedObject = PlacedObject.Create(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO);
            //var _placedObject =new PlacedObject();
            //placedObject = _placedObject.TestCreate(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO);

            //placedObject= photonView.RPC("RPCPlacedObjectCreate", RpcTarget.AllBuffered, placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO);

            foreach (Vector2Int gridPosition in gridPositionList) {
                selectedGrid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
            }                       

            placedObject.GridSetupDone();

            OnObjectPlaced?.Invoke(placedObject, EventArgs.Empty);

            return true;
        } else {
            // Cannot build here
            placedObject = null;
            return false;
        }
    }


    public Vector2Int GetGridPosition(Vector3 worldPosition) {
        selectedGrid.GetXZ(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public Vector3 GetWorldPosition(Vector2Int gridPosition) {
        return selectedGrid.GetWorldPosition(gridPosition.x, gridPosition.y);
    }

    public GridObject GetGridObject(Vector2Int gridPosition) {
        return selectedGrid.GetGridObject(gridPosition.x, gridPosition.y);
    }

    public GridObject GetGridObject(Vector3 worldPosition) {
        return selectedGrid.GetGridObject(worldPosition);
    }

    public bool IsValidGridPosition(Vector2Int gridPosition) {
        return selectedGrid.IsValidGridPosition(gridPosition);
    }

    public Vector3 GetMouseWorldSnappedPosition() {
        Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
        selectedGrid.GetXZ(mousePosition, out int x, out int z);

        if (placedObjectTypeSO != null) {
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = selectedGrid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * selectedGrid.GetCellSize();
            return placedObjectWorldPosition;
        } else {
            return mousePosition;
        }
    }

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

    public void SetSelectedPlacedObject(PlacedObjectTypeSO placedObjectTypeSO) {
        this.placedObjectTypeSO = placedObjectTypeSO;
        isDemolishActive = false;
        RefreshSelectedObjectType();
    }

    public void SetDemolishActive() {
        placedObjectTypeSO = null;
        isDemolishActive = true;
        RefreshSelectedObjectType();
    }

    public bool IsDemolishActive() {
        return isDemolishActive;
    }

    
    public async UniTaskVoid TestSave(string s)
    {
        List<PlacedObjectSaveObjectArray> placedObjectSaveObjectArrayList = new List<PlacedObjectSaveObjectArray>();

        foreach (GridXZ<GridObject> grid in gridList)
        {
            List<PlacedObject.SaveObject> saveObjectList = new List<PlacedObject.SaveObject>();
            List<PlacedObject> savedPlacedObjectList = new List<PlacedObject>();

            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    PlacedObject placedObject = grid.GetGridObject(x, y).GetPlacedObject();
                    if (placedObject != null && !savedPlacedObjectList.Contains(placedObject))
                    {
                        // Save object
                        savedPlacedObjectList.Add(placedObject);
                        saveObjectList.Add(placedObject.GetSaveObject());
                    }
                }
            }

            PlacedObjectSaveObjectArray placedObjectSaveObjectArray = new PlacedObjectSaveObjectArray { gridPlaceObjectList = saveObjectList};
            placedObjectSaveObjectArrayList.Add(placedObjectSaveObjectArray);
        }
        SaveAllBuilding saveObject = new SaveAllBuilding
        {
            islandGridList = placedObjectSaveObjectArrayList
        };

        string json = JsonUtility.ToJson(saveObject, true);
        Debug.Log(json);
        var url = "https://resource.mtvs-nebula.com/skyisland/" +s;
        var httpReq = new HttpRequester(new JsonSerializationOption());

        await httpReq.Post(url,json);

    }

   
    public async UniTaskVoid TestLoad(string s)
    {
        
        var url = "https://resource.mtvs-nebula.com/skyisland/" + s;
        var httpReq = new HttpRequester(new JsonSerializationOption());

        H_Building_Root result2 = await httpReq.Get<H_Building_Root>(url);


        for (int i = 0; i < gridList.Count; i++)
        {
            GridXZ<GridObject> grid = gridList[i];
            foreach (PlacedObject.SaveObject  placedObjectSaveObject in result2.results.placeObjects.islandGridList[i].gridPlaceObjectList)
            {   
                
                PlacedObjectTypeSO placedObjectTypeSO = BuildingSystemAssets.Instance.GetPlacedObjectTypeSOFromName(placedObjectSaveObject.placedObjectTypeSOName);
              
                TryPlaceObject(placedObjectSaveObject.origin, placedObjectTypeSO, placedObjectSaveObject.dir, out PlacedObject placedObject);
               
            }
        }
        
    }

    public async UniTaskVoid FirstBuildingLoad()
    {
        TestLoad(CHAN_GameManager.instance.ownIslandId).Forget();
        await UniTask.Yield();
        
        

    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

   

    [Serializable]
    public class SaveAllBuilding
    {
        public List<PlacedObjectSaveObjectArray> islandGridList;
    }

    [Serializable]
    public class PlacedObjectSaveObjectArray
    {

        public List<PlacedObject.SaveObject> gridPlaceObjectList;
    }



 
    public class H_IslandGridList
    {
        public List<PlacedObject.SaveObject> gridPlaceObjectList { get; set; }
    }


    public class H_PlaceObjects
    {
        public List<H_IslandGridList> islandGridList { get; set; }
    }




    public class H_Building_Results
    {
        public int id { get; set; }
        public string owner { get; set; }
        public H_PlaceObjects placeObjects { get; set; }
        public object dropItems { get; set; }
    }

    public class H_Building_Root
    {
        public int httpStatus { get; set; }
        public string message { get; set; }
        public H_Building_Results results { get; set; }
    }



    
}
