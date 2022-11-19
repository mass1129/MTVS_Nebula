using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Categories", menuName = "Scriptable Object/Categories", order = int.MaxValue)]
public class Categories:ScriptableObject
{

    Dictionary<string, List<string>> keyword_Dic;
    [SerializeField]
    List<string> _keyword1;
    [SerializeField]
    List<string> _keyword_Entertament;
    [SerializeField]
    List<string> _keyword_LifeStyle;
    [SerializeField]
    List<string> _keyword_Travel;
    [SerializeField]
    List<string> _keyword_Sports;
    [SerializeField]
    List<string> _keyword_Knowledge;


}

