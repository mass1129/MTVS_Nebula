public interface Server_FriendsInfo
{
    //������ ģ�� ��ϰ��� �������̽�
    //���ӻ󿡼� ģ������� �ҷ����ų� ģ���߰�, ������ ��� �̿�Ǵ� �Լ�����

    //�������� ģ������ �ҷ�����
    public void LoadFriendsInfo();
    
    //�������� ģ������ �߰��ϱ�
    public void SaveFriendsInfo();
    // �������� ģ������ �����ϱ�
    public void RemoveFriendsInfo();
   
}
public interface Server_IslandInfo
{
    // ������ �ϴü� ������ �������� ��������
    public void LoadIslandInfo();
    // ������ �ϴü� ������ �����ϱ�
    public void SaveIslandInfo();
}
public interface Server_Profile
{
    //������ ������ ������ ������ 
    public void LoadProfile(string userName);
    public void SaveProfile(string userName);
}