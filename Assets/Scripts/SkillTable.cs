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
            player.playerAni.SetTrigger("IsAttack");
            monster.startMonsterSingleDamaged(skill, SkillType);
            monster.nowMonsterStunStack += 1; // 기절 스택 추가
            BattleManager.Instance.BattleDialogText.text += "\n적을 기절시켰다!";
        }
    } // 몬스터 기절공격스킬 함수

    // 몬스터 단타공격스킬 함수, monster 및 player를 battlaManager의 함수실행에 skilloption을 실행할 때 매개변수로 nowPlayer, nowMonster를 받는다
    public void MonsterSingleAttackSkill(MonsterController monster, PlayerController player, string Name, string SkillType) // string 배열은 참조형식이기 때문에 ref사용, 없으면 오류발생 
    {       
        if (BattleManager.Instance.DodgeSucess(PlayerTable.Instance.Dodge)) // 플레이어가 회피 성공 시
        {
            player.playerAni.SetTrigger("IsDodge");
            BattleManager.Instance.BattleDialogText.text += $"\n적의 공격을 회피하였다!";            
        }
        else
        {
            monster.monsterAni.SetTrigger("IsAttack");
            player.startPlayerSingleDamaged(Name, SkillType); // 스킬이름을 매개변수로 주고 player의 코루틴함수 실행
        }
    }

    public void MonsterMultiAttackSkill(MonsterController monster, PlayerController player, string Name, int SkillTimes, string SkillType) // 몬스터 다타공격스킬 함수
    {
        if (BattleManager.Instance.DodgeSucess(PlayerTable.Instance.Dodge)) // 플레이어가 회피 성공 시
        {
            player.playerAni.SetTrigger("IsDodge");
            BattleManager.Instance.BattleDialogText.text += $"\n적의 공격을 회피하였다!";
            return;
        }
        else
        {
            monster.monsterAni.SetTrigger("IsAttack");
            player.startPlayerMultiDamaged(Name, SkillTimes, SkillType);
        }
    }
    
    public void MonsterStunAttackSkill(MonsterController monster, PlayerController player, string Name, string SkillType)
    {
        if (BattleManager.Instance.DodgeSucess(PlayerTable.Instance.Dodge)) // 플레이어가 회피 성공 시
        {
            player.playerAni.SetTrigger("IsDodge");
            BattleManager.Instance.BattleDialogText.text += $"\n적의 공격을 회피하였다!";
        }
        else
        {
            monster.monsterAni.SetTrigger("IsAttack");
            player.startPlayerSingleDamaged(Name, SkillType); // 스킬이름을 매개변수로 주고 player의 코루틴함수 실행
            PlayerTable.Instance.StunStack += 1; // 기절 스택 추가
            if (PlayerTable.Instance.StunStack <= 0)
            {
                BattleManager.Instance.BattleDialogText.text += "\n불굴의 의지로 기절을 견디었다!";
                return;
            }
            else
            {
                BattleManager.Instance.BattleDialogText.text += "\n적의 공격에 기절하였다!";
            }
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
    public int SkillTimes;
    public string SkillType;
    public virtual void SkillOption(MonsterController monster, PlayerController player) { }
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
public class BaseSkill 
{
    public string Name;
    public float SkillPercentage;    
    public float CriMultiple;
    public Sprite SkillSprite;
    public string[] SkillText; // 스킬에 대한 설명    
    public int AvailableCount;
    public string SkillType;
    public int SkillTimes;
    // 공격타입 : 단수공격, 멀티공격
    public virtual void SkillOption(MonsterController monster, PlayerController player) { }    
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






