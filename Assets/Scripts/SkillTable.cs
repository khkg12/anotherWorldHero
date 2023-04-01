using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


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


    private void OnEnable()
    {
        ActiveSkillList = new List<BaseSkill>() { Instance.doubleAttack, Instance.baldo, Instance.stunBoom }; // 스킬리스트 초기화
    }
    

    // 플레이어 스킬
    public Attack attack;
    public Defense defense;
    public StunAttack stunBoom;
    public DoubleAttack doubleAttack;
    public Baldo baldo;
        
    // 몬스터 스킬    
    // 마족기사
    public DemonSlayerSlash demonSlayerSlash;
    public DemonSlayerStabbing demonSlayerStabbing;
    public DemonSlayerSwordsmanship demonSlayerSwordsmanship;
    // 마족궁수
    public DemonArcherArrowShot demonArcherArrowShot;
    public DemonArcherDoubleShot demonArcherDouble;
    public DemonArcherDarkArrow demonArcherDarkArrow;
    // 마족 주술사
    public DemonShamanEnergyBolt demonShamanEnergyBolt;
    public DemonShamanDarkLightning demonShamanDarkLightning;
    public DemonShamanStunBall demonShamanStunBall;


    // 몬스터 단타공격스킬 함수
    public void MonsterSingleAttackSkill(MonsterController monster, PlayerController player, string Name, string SkillTrigger) // string 배열은 참조형식이기 때문에 ref사용, 없으면 오류발생 
    {       
        if (BattleManager.Instance.DodgeSucess(PlayerTable.Instance.Dodge)) // 플레이어가 회피 성공 시
        {
            player.playerAni.SetTrigger("IsDodge");
            BattleManager.Instance.BattleDialogText.text += $"\n\n적의 공격을 회피하였다!";            
        }
        else
        {
            monster.monsterAni.SetTrigger(SkillTrigger);            
            if (BattleManager.Instance.CriAttack(monster.nowMonsterCri)) // 치명타 공격이라면
            {                
                player.playerDamaged(BattleManager.Instance.CriAttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n치명타!! {monster.nowMonsterName}의 {Name}! \n{BattleManager.Instance.CriAttackAmount} 만큼 피해를 입었다! ({BattleManager.Instance.CriDefenseAmount})방어함";                
                BattleManager.Instance.FloatingText(BattleManager.Instance.PlayerDamageTextList, BattleManager.Instance.CriAttackAmount, BattleManager.Instance.SkillCount);                
            }
            else
            {
                player.playerDamaged(BattleManager.Instance.AttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n{monster.nowMonsterName}의 {Name}! \n{BattleManager.Instance.AttackAmount} 만큼 피해를 입었다! ({BattleManager.Instance.DefenseAmount})방어함";
                BattleManager.Instance.FloatingText(BattleManager.Instance.PlayerDamageTextList, BattleManager.Instance.AttackAmount, BattleManager.Instance.SkillCount);                
            }
        }
    }

    // 몬스터 여러타수공격스킬 함수    
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
    public string[] SkillDialog;
    public float CriMultiple;
    public Sprite SkillSprite;
    public string[] SkillText; // 스킬에 대한 설명
    public string SkillTrigger;
    public int AvailableCount;
      
    public virtual void SkillOption(MonsterController monster, PlayerController player) { }    
}

[System.Serializable]
public class Attack : BaseSkill
{    
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        float DamageAmount = PlayerTable.Instance.NowAtk * SkillPercentage;
        float CriDamageAmount = PlayerTable.Instance.NowAtk * SkillPercentage * CriMultiple;

        if (BattleManager.Instance.DodgeSucess(monster.nowMonsterDodge))
        {
            BattleManager.Instance.BattleDialogText.text += $"\n\n공격이 빗나갔다!";
            monster.monsterAni.SetTrigger("IsDodge");
        }
        else
        {
            player.playerAni.SetTrigger(SkillTrigger);
            if (BattleManager.Instance.CriAttack(PlayerTable.Instance.Critical)) // 치명타 공격이라면
            {
                monster.MonsterDamaged(CriDamageAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n치명타!! {Name} 스킬 적중! \n{CriDamageAmount} 만큼 피해를 입혔다!";
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, CriDamageAmount, BattleManager.Instance.SkillCount);                
            }
            else
            {
                monster.MonsterDamaged(DamageAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n{Name}! \n{DamageAmount} 만큼 피해를 입혔다!";                
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, DamageAmount, BattleManager.Instance.SkillCount);
            }
        }                        
    }    
}

[System.Serializable]
public class StunAttack : BaseSkill
{    
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        float DamageAmount = PlayerTable.Instance.NowAtk * SkillPercentage;
        float CriDamageAmount = PlayerTable.Instance.NowAtk * SkillPercentage * CriMultiple;

        if (BattleManager.Instance.DodgeSucess(monster.nowMonsterDodge))
        {
            BattleManager.Instance.BattleDialogText.text += $"\n\n공격이 빗나갔다!";
            monster.monsterAni.SetTrigger("IsDodge");
        }
        else
        {
            player.playerAni.SetTrigger(SkillTrigger);            
            if (BattleManager.Instance.CriAttack(PlayerTable.Instance.Critical)) // 치명타 공격이라면
            {
                monster.MonsterDamaged(PlayerTable.Instance.NowAtk * SkillPercentage * CriMultiple);
                BattleManager.Instance.BattleDialogText.text += $"\n\n치명타!! {Name} 스킬 적중! \n{PlayerTable.Instance.NowAtk * SkillPercentage * CriMultiple} 만큼 피해를 입혔다!";
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, CriDamageAmount, BattleManager.Instance.SkillCount);
            }
            else
            {
                monster.MonsterDamaged(PlayerTable.Instance.NowAtk * SkillPercentage);
                BattleManager.Instance.BattleDialogText.text += $"\n\n{Name}! \n{PlayerTable.Instance.NowAtk * SkillPercentage} 만큼 피해를 입혔다!";
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, DamageAmount, BattleManager.Instance.SkillCount);
            }
            monster.nowMonsterStunStack += 1; // 기절 스택 추가
            BattleManager.Instance.BattleDialogText.text += "\n적을 기절시켰다!";
        }
    }
}

[System.Serializable]
public class Defense : BaseSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        player.playerAni.SetTrigger(SkillTrigger);
        PlayerTable.Instance.NowDefense += SkillPercentage * PlayerTable.Instance.Defense;
        BattleManager.Instance.BattleDialogText.text += $"\n\n{Name}! ({SkillPercentage * PlayerTable.Instance.Defense}) 방어도를 얻었다!";
    }
}

[System.Serializable]
public class DoubleAttack : BaseSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {        
        if (BattleManager.Instance.DodgeSucess(monster.nowMonsterDodge))
        {
            monster.monsterAni.SetTrigger("IsDodge");
            BattleManager.Instance.BattleDialogText.text += "\n\n공격이 빗나갔다!";
            return;
        }
        else
        {
            BattleManager.Instance.BattleDialogText.text += $"\n\n {Name}!";
            player.playerAni.SetTrigger(SkillTrigger);            
        }        
    }
}

[System.Serializable]
public class Baldo : BaseSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        float DamageAmount = PlayerTable.Instance.NowAtk * SkillPercentage;
        float CriDamageAmount = PlayerTable.Instance.NowAtk * SkillPercentage * CriMultiple;

        if (BattleManager.Instance.DodgeSucess(monster.nowMonsterDodge))
        {
            BattleManager.Instance.BattleDialogText.text += $"\n\n공격이 빗나갔다!";
            monster.monsterAni.SetTrigger("IsDodge");
        }
        else
        {            
            player.playerAni.SetTrigger(SkillTrigger);
            if (BattleManager.Instance.CriAttack(PlayerTable.Instance.Critical)) // 치명타 공격이라면
            {
                monster.MonsterDamaged(CriDamageAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n치명타!! {Name} 스킬 적중! \n{CriDamageAmount} 만큼 피해를 입혔다!";
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, CriDamageAmount, BattleManager.Instance.SkillCount);
            }
            else
            {
                monster.MonsterDamaged(DamageAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n{Name}! \n{DamageAmount} 만큼 피해를 입혔다!";
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, DamageAmount, BattleManager.Instance.SkillCount);
            }
        }
    }
}

// 몬스터 스킬 클래스
[System.Serializable]
public class MonsterSkill
{
    public string Name;
    public float SkillPercentage;
    public float CriMultiple;
    public string[] SkillDialog;
    public string SkillTrigger;
    public virtual void SkillOption(MonsterController monster, PlayerController player) { }
}

// 마족 기사 스킬
[System.Serializable]
public class DemonSlayerSlash : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.MonsterSingleAttackSkill(monster, player, Name, SkillTrigger);
    }
}
[System.Serializable]
public class DemonSlayerStabbing : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.MonsterSingleAttackSkill(monster, player, Name, SkillTrigger);        
    }
}
[System.Serializable]
public class DemonSlayerSwordsmanship : MonsterSkill 
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.MonsterSingleAttackSkill(monster, player, Name, SkillTrigger);        
    }
}

// 마족 궁수 스킬
[System.Serializable]
public class DemonArcherArrowShot : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.MonsterSingleAttackSkill(monster, player, Name, SkillTrigger);
    }
}
[System.Serializable]
public class DemonArcherDoubleShot : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        if (BattleManager.Instance.DodgeSucess(PlayerTable.Instance.Dodge)) // 플레이어가 회피 성공 시
        {
            player.playerAni.SetTrigger("IsDodge");
            BattleManager.Instance.BattleDialogText.text += $"\n\n적의 공격을 회피하였다!";
            return;
        }
        else
        {
            BattleManager.Instance.BattleDialogText.text += $"\n\n {monster.nowMonsterName}의 {Name}!";
            monster.monsterAni.SetTrigger(SkillTrigger);
        }
    }
}
[System.Serializable]
public class DemonArcherDarkArrow : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.MonsterSingleAttackSkill(monster, player, Name, SkillTrigger);
    }
}

// 마족 주술사 스킬
[System.Serializable]
public class DemonShamanEnergyBolt : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.MonsterSingleAttackSkill(monster, player, Name, SkillTrigger);
    }
}
[System.Serializable]
public class DemonShamanDarkLightning : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        if (BattleManager.Instance.DodgeSucess(PlayerTable.Instance.Dodge)) // 플레이어가 회피 성공 시
        {
            player.playerAni.SetTrigger("IsDodge");
            BattleManager.Instance.BattleDialogText.text += $"\n\n적의 공격을 회피하였다!";
            return;
        }
        else
        {
            BattleManager.Instance.BattleDialogText.text += $"\n\n {monster.nowMonsterName}의 {Name}!";
            monster.monsterAni.SetTrigger(SkillTrigger);
        }
    }
}
[System.Serializable]
public class DemonShamanStunBall : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {       
        if (BattleManager.Instance.DodgeSucess(monster.nowMonsterDodge))
        {
            player.playerAni.SetTrigger("IsDodge");
            BattleManager.Instance.BattleDialogText.text += $"\n\n적의 공격을 회피하였다!";
        }
        else
        {
            monster.monsterAni.SetTrigger(SkillTrigger);
            if (BattleManager.Instance.CriAttack(monster.nowMonsterCri)) // 치명타 공격이라면
            {
                player.playerDamaged(BattleManager.Instance.CriAttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n치명타!! {monster.nowMonsterName}의 {Name}! \n{BattleManager.Instance.CriAttackAmount} 만큼 피해를 입었다! ({BattleManager.Instance.CriDefenseAmount})방어함";
                BattleManager.Instance.FloatingText(BattleManager.Instance.PlayerDamageTextList, BattleManager.Instance.CriAttackAmount, BattleManager.Instance.SkillCount);
            }
            else
            {
                player.playerDamaged(BattleManager.Instance.AttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n{monster.nowMonsterName}의 {Name}! \n{BattleManager.Instance.AttackAmount} 만큼 피해를 입었다! ({BattleManager.Instance.DefenseAmount})방어함";
                BattleManager.Instance.FloatingText(BattleManager.Instance.PlayerDamageTextList, BattleManager.Instance.AttackAmount, BattleManager.Instance.SkillCount);
            }
            PlayerTable.Instance.StunStack += 1; // 기절 스택 추가
            if(PlayerTable.Instance.StunStack <= 0)
            {
                BattleManager.Instance.BattleDialogText.text += "\n불굴의 의지로 기절을 견디었다!";
                return;
            }
            else
            {
                BattleManager.Instance.BattleDialogText.text += "\n적의 공격에 기절하였다!";                
            }            
        }
    }
}




