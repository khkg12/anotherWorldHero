using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BackGroundTable", menuName = "ScriptableObjects/BackGroundTable", order = 6)]
public class BackGroundTable : ScriptableObject
{
    public static BackGroundTable Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = Resources.Load("BackGroundTable") as BackGroundTable;
            }
            return _Instance;
        }
    }
    private static BackGroundTable _Instance;

    public List<BgImage> MainBackGroundImageList;
    public List<BgImage> RandomBackGroundImageList;
    public List<BgImage> BattleBackGroundImageList;
}

[System.Serializable]
public class BgImage
{
    public string bgName;
    public Sprite bgSprite;    
}

