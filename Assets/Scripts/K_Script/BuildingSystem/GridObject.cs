public class GridObject
{
    private PlacedObject placedObject;
    //그리드 오브젝트 생성자
    public GridObject()
    {
        placedObject = null;
    }

    //그리드오브젝트에 건물 할당
    public void SetPlacedObject(PlacedObject placedObject)
    {
        this.placedObject = placedObject;
    }
    //그리드 오브젝트에 건물 할당 해제
    public void ClearPlacedObject()
    {
        placedObject = null;
    }
    //그리드 오브젝트에 할당된 선물 반환
    public PlacedObject GetPlacedObject()
    {
        return placedObject;
    }
    //그리드 오브젝트에 건물이 할당 되었는지 확인
    public bool CanBuild()
    {
        return placedObject == null;
    }

}

