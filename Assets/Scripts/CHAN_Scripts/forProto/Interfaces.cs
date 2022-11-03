public interface Server_FriendsInfo
{
    //유저의 친구 목록관련 인터페이스
    //게임상에서 친구목록을 불러오거나 친구추가, 삭제할 경우 이용되는 함수모음

    //서버에서 친구정보 불러오기
    public void LoadFriendsInfo();
    
    //서버에게 친구정보 추가하기
    public void SaveFriendsInfo();
    // 서버에게 친구정보 삭제하기
    public void RemoveFriendsInfo();
   
}
public interface Server_IslandInfo
{
    // 유저의 하늘섬 정보를 서버에서 가져오기
    public void LoadIslandInfo();
    // 유저의 하늘섬 정보를 저장하기
    public void SaveIslandInfo();
}
public interface Server_Profile
{
    //유저의 프로필 정보를 가져옴 
    public void LoadProfile(string userName);
    public void SaveProfile(string userName);
}