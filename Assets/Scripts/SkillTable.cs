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
            
    // 플레이어 스킬
    public Attack attack;
    public Defense defense;
    public StunAttack stunBoom;
    public DoubleAttack doubleAttack;
    public Baldo baldo;

    public void PlayerSingleAttackSkill(MonsterController monster, PlayerController player, BaseSkill skill, string SkillType)
    {        
        if (BattleManager.Instance.DodgeSucess(monster.nowMonsterDodge))
        {
            BattleManager.Instance.BattleDialogText.text += $"\n공격이 빗나갔다!";
            monster.monsterAni.SetTrigger("IsDodge");
        }
        else
        {
            player.playerAni.SetTrigger("IsAttack");
            monster.startMonsterSingleDamaged(skill, SkillType);
        }
    }

    public void PlayerMultiAttackSkill(MonsterController monster, PlayerController player, BaseSkill skill, int SkillTimes, string SkillType) // 다타공격스킬 함수
    {
        if (BattleManager.Instance.DodgeSucess(monster.nowMonsterDodge))
        {
            BattleManager.Instance.BattleDialogText.text += $"\n공격이 빗나갔다!";
            monster.monsterAni.SetTrigger("IsDodge");
        }
        else
        {
            player.playerAni.SetTrigger("IsAttack");
            monster.startMonsterMultiDamaged(skill, SkillTimes, SkillType);
        }
    }

    public void PlayerStunAttackSkill(MonsterController monster, PlayerController player, BaseSkill skill, string SkillType)
    {
        if (BattleManager.Instance.DodgeSucess(monster.nowMonsterDodge))
        {
            BattleManager.Instance.BattleDialogText.text += $"\n공격이 빗나갔다!";
            monster.monsterAni.SetTrigger("IsDodge");
        }
        else
        {
            player.playerAni.SetTrigger("IsAttack");
            monster.startMonsterSingleDamaged(skill, SkillType);
            monster.nowMonsterStunStack += 1; // 기절 스택 추가
            BattleManager.Instance.BattleDialogText.text += "\n적을 기절시켰다!";
        }
    } // 몬스터 기절공격스킬 함수

    public void PlayerDotAttackSkill(MonsterController monster, PlayerController player, BaseSkill skill, string SkillType)
    {
        if (BattleManager.Instance.DodgeSucess(monster.nowMonsterDodge))
        {
            BattleManager.Instance.BattleDialogText.text += $"\n공격이 빗나갔다!";
            monster.monsterAni.SetTrigger("IsDodge");
        }
        else
        {
            player.playerAni.SetTrigger("IsAttack"); // 출혈 or 도트대미지 애니로 바꾸기
            // monster.startMonsterDotDamaged(skill, SkillType);     startDotDamaged
        }
    } // 몬스터 기절공격스킬 함수    
    
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
    public Type type;    
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


public enum Type
{
    Attack,
    Defense,
    Buff,
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
    public int StunCount;
    public string SkillType;
    public int SkillTimes;
    public Type type;
    // 공격타입 : 단수공격, 멀티공격
    public virtual void SkillOption(MonsterController monster, PlayerController player) { }
    public virtual void SkillUse(Character character) { }    
}

[System.Serializable]
public class Attack : BaseSkill
{    
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.PlayerSingleAttackSkill(monster, player, this, SkillType); // 배틀매니저에서 monster를 nowmonster로 받고 this는 자기자신, 즉 스킬
    }    
}

[System.Serializable]
public class StunAttack : BaseSkill
{    
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.PlayerStunAttackSkill(monster, player, this, SkillType);
    }
}

[System.Serializable]
public class Defense : BaseSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        player.playerAni.SetTrigger("IsDefense");
        PlayerTable.Instance.NowDefense += SkillPercentage * PlayerTable.Instance.Defense;
        BattleManager.Instance.BattleDialogText.text += $"\n\n{Name}! ({SkillPercentage * PlayerTable.Instance.Defense}) 방어도를 얻었다!";
    }
}

[System.Serializable]
public class DoubleAttack : BaseSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.PlayerMultiAttackSkill(monster, player, this, SkillTimes, SkillType);
    }
}

[System.Serializable]
public class Baldo : BaseSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.PlayerSingleAttackSkill(monster, player, this, SkillType);
    }
}

public class FireBurn : BaseSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        
    }
}




