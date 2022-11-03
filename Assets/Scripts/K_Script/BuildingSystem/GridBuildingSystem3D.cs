using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Photon.Pun;

public class GridBuildingSystem3D : MonoBehaviourPun
{

    public static GridBuildingSystem3D Instance { get; private set; }

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;

    private const float GRID_HEIGHT = 2.5f;
    
    private List<GridXZ<GridObject>> gridList;
    private GridXZ<GridObject> selectedGrid;

    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList = null;
    private PlacedObjectTypeSO placedObjectTypeSO;
    private PlacedObjectTypeSO.Dir dir;

    private bool isDemolishActive;

    private void Awake() {
        Instance = this;
        //그리드 세팅
        int gridWidth = 100;
        int gridHeight =100;
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
        Load();
    }
    private void Update()
    {
        HandleTypeSelect();
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

        public GridObject(GridXZ<GridObject> grid, int x, int y) {
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

    

    private void HandleNormalObjectPlacement() {
        if (Input.GetMouseButtonDown(0) && placedObjectTypeSO != null && !UtilsClass.IsPointerOverUI())
        {
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
            selectedGrid.GetXZ(mousePosition, out int x, out int z);

            Vector2Int placedObjectOrigin = new Vector2Int(x, z);

            if (TryPlaceObject(placedObjectOrigin, placedObjectTypeSO, dir, out PlacedObject placedObject))
            {
                // Object placed
            } 
            else 
            {
                // Error!
                UtilsClass.CreateWorldTextPopup("Cannot Build Here!", mousePosition);
            }
        }
    }

    private void HandleTypeSelect() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { placedObjectTypeSO = placedObjectTypeSOList[0]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { placedObjectTypeSO = placedObjectTypeSOList[1]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { placedObjectTypeSO = placedObjectTypeSOList[2]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { placedObjectTypeSO = placedObjectTypeSOList[3]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { placedObjectTypeSO = placedObjectTypeSOList[4]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { placedObjectTypeSO = placedObjectTypeSOList[5]; RefreshSelectedObjectType(); }

        
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
                placedObject.DestroySelf();

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

    private void RefreshSelectedObjectType() {
        //UpdateCanBuildTilemap();

        if (placedObjectTypeSO == null) {
            //TilemapVisual.Instance.Hide();
        } else {
            //TilemapVisual.Instance.Show();
        }

        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateCanBuildTilemap() {
        /*
        // Not implemented by default
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                // Tilemap
                Tilemap.Instance.SetTilemapSprite(new Vector3(x, y),
                    grid.GetGridObject(x, y).CanBuild() ?
                    Tilemap.TilemapObject.TilemapSprite.CanBuild :
                    Tilemap.TilemapObject.TilemapSprite.CannotBuild);
            }
        }*/
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

    public void Save()
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

            PlacedObjectSaveObjectArray placedObjectSaveObjectArray = new PlacedObjectSaveObjectArray { GridPlaceObjectList = saveObjectList.ToArray() };
            placedObjectSaveObjectArrayList.Add(placedObjectSaveObjectArray);
        }
        SaveObject saveObject = new SaveObject
        {
            IslandGridList = placedObjectSaveObjectArrayList.ToArray()
        };
        string json = JsonUtility.ToJson(saveObject,true);
        PlayerPrefs.SetString("HouseBuildingSystemSave", json);
        K_SaveSystem.Save("HouseBuildingSystemSave", json, true);
        
    }
    public void Load()
    {
        if (PlayerPrefs.HasKey("HouseBuildingSystemSave"))
        {
            string json = PlayerPrefs.GetString("HouseBuildingSystemSave");
            json = K_SaveSystem.Load("HouseBuildingSystemSave");

            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(json);



            for (int i = 0; i < gridList.Count; i++)
            {
                GridXZ<GridObject> grid = gridList[i];
                foreach (PlacedObject.SaveObject placedObjectSaveObject in saveObject.IslandGridList[i].GridPlaceObjectList)
                {
                    PlacedObjectTypeSO placedObjectTypeSO = BuildingSystemAssets.Instance.GetPlacedObjectTypeSOFromName(placedObjectSaveObject.placedObjectTypeSOName);
                    TryPlaceObject(placedObjectSaveObject.origin, placedObjectTypeSO, placedObjectSaveObject.dir, out PlacedObject placedObject);
                }
            }  
        }
        Debug.Log("Load!");
    }
    [Serializable]
    public class SaveObject
    {
        public PlacedObjectSaveObjectArray[] IslandGridList;
    }

    [Serializable]
    public class PlacedObjectSaveObjectArray
    {
        public PlacedObject.SaveObject[] GridPlaceObjectList;
    }
}
