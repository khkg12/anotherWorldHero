using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BlessingTable", menuName = "ScriptableObjects/BlessingTable", order = 5)]
public class BlessingTable : ScriptableObject
{
    public static BlessingTable Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = Resources.Load("BlessingTable") as BlessingTable;
            }
            return _Instance;
        }
    }
    public static BlessingTable _Instance;

    public List<Blessing> BlessingList;
}

[System.Serializable]
public class Blessing
{
    public string BlessingName;
    public Sprite BlessingSprite;
    public int IronBodyPt;
    public int ScarePt;    
    public int FightingSpiritPt;
    public int WillPowerPt;
    public int maxHp;    
    public int Atk;
    public int Def;
    public int Dod;
    public int Cri;
    public string[] Option;
}


