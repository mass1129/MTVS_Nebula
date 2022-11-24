using System.Collections.Generic;
namespace UserProfile
{
    public class Blendshape
    {
        public string blendshapeName { get; set; }
        public int type { get; set; }

        public double value { get; set; }
        public int group { get; set; }
    }
    public class Result
    {
        public int id { get; set; }
        public string name { get; set; }
        public string imageUrl { get; set; }
        public List<string> hashTags { get; set; }
        public int skyIslandId { get; set; }
        public profile_Texture texture { get; set; }
    }
    public class Root
    {
        public int httpStatus { get; set; }
        public string message { get; set; }
        public List<Result> results { get; set; }
    }
    public class Root_Agora
    {
        public string appId { get; set; }
        public string channelName { get; set; }
        public string token { get; set; }
    }
    public class SelectedElements
    {
        public int Hair { get; set; }
        public int Beard { get; set; }
        public int Accessory { get; set; }
        public int Shirt { get; set; }
        public int Item1 { get; set; }
        public int Pants { get; set; }
        public int Hat { get; set; }
        public int Shoes { get; set; }
    }

    public class profile_Texture
    {
        public List<double> TeethColor { get; set; }
        public List<double> OralCavityColor { get; set; }
        public SelectedElements selectedElements { get; set; }
        public List<double> HairColor { get; set; }
        public int MaxLod { get; set; }
        public int MinLod { get; set; }
        public List<double> EyeColor { get; set; }
        public List<double> SkinColor { get; set; }
        public List<double> UnderpantsColor { get; set; }
        public string settingsName { get; set; }
        public double HeadSize { get; set; }
        public double Height { get; set; }
        public List<Blendshape> blendshapes { get; set; }
    }
    public class ProfileInfo
    {
        public int User_Id;
        public string User_Name;
        public string User_Image_Url;
        public List<string> HashTag;
        public int User_Island_ID;
        public profile_Texture texture_info;
    }
    public class new_ProfileInfo
    {
        public string User_Name;
        public List<string> HashTag=new List<string>();
        public UnityEngine.Texture2D ProfileImage;
    }
}