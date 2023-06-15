using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Linq;
using System.Globalization;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "SkillTable", menuName = "ScriptableObjects/SkillTable", order = 3)]
public class SkillTable : ScriptableObject
{
    public static SkillTable Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = Resources.Load("SkillTable") as SkillTable;
            }
            return _Instance;
        }
    }
    private static SkillTable _Instance;

    // 스킬선택 시 랜덤으로 돌릴 스킬리스트
    public List<BaseSkill> ActiveSkillList; 
    public List<PassiveSkill> PassiveSkillList;                        
}

// 몬스터 스킬 클래스
[System.Serializable]
public class MonsterSkill : ScriptableObject
{
    public string Name;
    public float SkillPercentage;
    public float CriMultiple;
    public string SkillText; // 몬스터가 무슨 공격을 할지 암시하는 텍스트    
    public int StunCount;
    public int SkillTimes;    
    public string SkillType;    
    public virtual void SkillUse(MonsterClass monster) { }
}

// 플레이어 패시브 스킬 클래스
[System.Serializable]
public class PassiveSkill
{
    public Sprite Skillsprite;
    public string Name;
    public string[] SkillText; // 스킬에 대한 설명
    public int MaxHp;
    public int Atk;
    public int Def;
    public int Dod;
    public int Cri;
}

// 플레이어 액티브 스킬 클래스
[System.Serializable]
public class BaseSkill : ScriptableObject
{
    public string Name;
    public float SkillPercentage;    
    public float CriMultiple;
    public Sprite SkillSprite;
    public string[] SkillText; // 스킬에 대한 설명    
    public int AvailableCount;
    public int StunCount; // bool로 변경
    public string SkillType;
    public int SkillTimes;    
    // 공격타입 : 단수공격, 멀티공격    
    public virtual void SkillUse(Character character) { }    
}






