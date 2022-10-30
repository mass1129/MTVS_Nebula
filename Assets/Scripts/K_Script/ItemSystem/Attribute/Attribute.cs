[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public K_PlayerStats parent;
    public Attributes type;
    public ModifiableInt value;
    public void SetParent(K_PlayerStats _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified);
    }
    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}