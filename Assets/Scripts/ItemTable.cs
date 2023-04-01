using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "ItemTable", menuName = "ScriptableObjects/ItemTable", order = 1)]
public class ItemTable : ScriptableObject
{
    public static ItemTable Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = Resources.Load("ItemTable") as ItemTable;
            }
            return _Instance;
        }
    }
    private static ItemTable _Instance;
    
    public List<Item> ItemList;
}

[System.Serializable]
public class Item
{
    public Sprite sprite;
    public string name;
    public string[] option;
    public int maxHp;
    public int Hp;
    public int Atk;
    public int Def;
    public int Dod;
    public int Cri;
}
