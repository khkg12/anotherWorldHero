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

    // ��ų���� �� �������� ���� ��ų����Ʈ
    public List<BaseSkill> ActiveSkillList; 
    public List<PassiveSkill> PassiveSkillList;                        
}

// ���� ��ų Ŭ����
[System.Serializable]
public class MonsterSkill : ScriptableObject
{
    public string Name;
    public float SkillPercentage;
    public float CriMultiple;
    public string SkillText; // ���Ͱ� ���� ������ ���� �Ͻ��ϴ� �ؽ�Ʈ    
    public int StunCount;
    public int SkillTimes;    
    public string SkillType;    
    public virtual void SkillUse(MonsterClass monster) { }
}

// �÷��̾� �нú� ��ų Ŭ����
[System.Serializable]
public class PassiveSkill
{
    public Sprite Skillsprite;
    public string Name;
    public string[] SkillText; // ��ų�� ���� ����
    public int MaxHp;
    public int Atk;
    public int Def;
    public int Dod;
    public int Cri;
}

// �÷��̾� ��Ƽ�� ��ų Ŭ����
[System.Serializable]
public class BaseSkill : ScriptableObject
{
    public string Name;
    public float SkillPercentage;    
    public float CriMultiple;
    public Sprite SkillSprite;
    public string[] SkillText; // ��ų�� ���� ����    
    public int AvailableCount;
    public int StunCount; // bool�� ����
    public string SkillType;
    public int SkillTimes;    
    // ����Ÿ�� : �ܼ�����, ��Ƽ����    
    public virtual void SkillUse(Character character) { }    
}






