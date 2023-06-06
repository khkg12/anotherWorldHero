using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterTable", menuName = "ScriptableObjects/MonsterTable", order = 4)]
public class MonsterTable : ScriptableObject
{
    public static MonsterTable Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = Resources.Load("MonsterTable") as MonsterTable;
            }
            return _Instance;
        }
    }
    private static MonsterTable _Instance;
    
    public List<Monster> MonsterList;
    public List<Monster> SecondActMonsterList;
    public List<Monster> ThirdActMonsterList;
    public List<Monster> FourthActMonsterList;
    public int MonsterNum;    
}

[System.Serializable]
public class Monster
{
    public Sprite MonsterSprite;
    public string MonsterName;
    public int MonsterHp;
    public int MonsterMaxHp;
    public int MonsterAtk;
    public int MonsterDodge;
    public int MonsterCri;
    public int MonsterStunStack;
    public bool IsMonsterBoss;
    public List<MonsterSkill> monsterSkillList;
}
