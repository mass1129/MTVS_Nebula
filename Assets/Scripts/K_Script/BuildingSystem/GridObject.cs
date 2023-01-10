public class GridObject
{
    private PlacedObject placedObject;
    //�׸��� ������Ʈ ������
    public GridObject()
    {
        placedObject = null;
    }

    //�׸��������Ʈ�� �ǹ� �Ҵ�
    public void SetPlacedObject(PlacedObject placedObject)
    {
        this.placedObject = placedObject;
    }
    //�׸��� ������Ʈ�� �ǹ� �Ҵ� ����
    public void ClearPlacedObject()
    {
        placedObject = null;
    }
    //�׸��� ������Ʈ�� �Ҵ�� ���� ��ȯ
    public PlacedObject GetPlacedObject()
    {
        return placedObject;
    }
    //�׸��� ������Ʈ�� �ǹ��� �Ҵ� �Ǿ����� Ȯ��
    public bool CanBuild()
    {
        return placedObject == null;
    }

}

