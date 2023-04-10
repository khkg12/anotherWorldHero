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

    // ��ų���� �� �������� ���� ��ų����Ʈ
    public List<BaseSkill> ActiveSkillList; 
    public List<PassiveSkill> PassiveSkillList;


    private void OnEnable()
    {
        ActiveSkillList = new List<BaseSkill>() { Instance.doubleAttack, Instance.baldo, Instance.stunBoom }; // ��ų����Ʈ �ʱ�ȭ
    }
    

    // �÷��̾� ��ų
    public Attack attack;
    public Defense defense;
    public StunAttack stunBoom;
    public DoubleAttack doubleAttack;
    public Baldo baldo;
        
    // ���� ��ų    
    // �������
    public DemonSlayerSlash demonSlayerSlash;
    public DemonSlayerStabbing demonSlayerStabbing;
    public DemonSlayerSwordsmanship demonSlayerSwordsmanship;
    // �����ü�
    public DemonArcherArrowShot demonArcherArrowShot;
    public DemonArcherDoubleShot demonArcherDouble;
    public DemonArcherDarkArrow demonArcherDarkArrow;
    // ���� �ּ���
    public DemonShamanEnergyBolt demonShamanEnergyBolt;
    public DemonShamanDarkLightning demonShamanDarkLightning;
    public DemonShamanStunBall demonShamanStunBall;


    // ���� ��Ÿ���ݽ�ų �Լ�
    public void MonsterSingleAttackSkill(MonsterController monster, PlayerController player, string Name, string SkillTrigger) // string �迭�� ���������̱� ������ ref���, ������ �����߻� 
    {       
        if (BattleManager.Instance.DodgeSucess(PlayerTable.Instance.Dodge)) // �÷��̾ ȸ�� ���� ��
        {
            player.playerAni.SetTrigger("IsDodge");
            BattleManager.Instance.BattleDialogText.text += $"\n\n���� ������ ȸ���Ͽ���!";            
        }
        else
        {
            monster.monsterAni.SetTrigger(SkillTrigger);            
            if (BattleManager.Instance.CriAttack(monster.nowMonsterCri)) // ġ��Ÿ �����̶��
            {                
                player.playerDamaged(BattleManager.Instance.CriAttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\nġ��Ÿ!! {monster.nowMonsterName}�� {Name}! \n{BattleManager.Instance.CriAttackAmount} ��ŭ ���ظ� �Ծ���! ({BattleManager.Instance.CriDefenseAmount})�����";                
                BattleManager.Instance.FloatingText(BattleManager.Instance.PlayerDamageTextList, BattleManager.Instance.CriAttackAmount, BattleManager.Instance.SkillCount);                
            }
            else
            {
                player.playerDamaged(BattleManager.Instance.AttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n{monster.nowMonsterName}�� {Name}! \n{BattleManager.Instance.AttackAmount} ��ŭ ���ظ� �Ծ���! ({BattleManager.Instance.DefenseAmount})�����";
                BattleManager.Instance.FloatingText(BattleManager.Instance.PlayerDamageTextList, BattleManager.Instance.AttackAmount, BattleManager.Instance.SkillCount);                
            }
        }
    }

    // ���� ����Ÿ�����ݽ�ų �Լ�    
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
public class BaseSkill
{
    public string Name;
    public float SkillPercentage;
    public string[] SkillDialog;
    public float CriMultiple;
    public Sprite SkillSprite;
    public string[] SkillText; // ��ų�� ���� ����
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
            BattleManager.Instance.BattleDialogText.text += $"\n\n������ ��������!";
            monster.monsterAni.SetTrigger("IsDodge");
        }
        else
        {
            player.playerAni.SetTrigger(SkillTrigger);
            if (BattleManager.Instance.CriAttack(PlayerTable.Instance.Critical)) // ġ��Ÿ �����̶��
            {
                monster.MonsterDamaged(CriDamageAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\nġ��Ÿ!! {Name} ��ų ����! \n{CriDamageAmount} ��ŭ ���ظ� ������!";
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, CriDamageAmount, BattleManager.Instance.SkillCount);                
            }
            else
            {
                monster.MonsterDamaged(DamageAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n{Name}! \n{DamageAmount} ��ŭ ���ظ� ������!";                
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
            BattleManager.Instance.BattleDialogText.text += $"\n\n������ ��������!";
            monster.monsterAni.SetTrigger("IsDodge");
        }
        else
        {
            player.playerAni.SetTrigger(SkillTrigger);            
            if (BattleManager.Instance.CriAttack(PlayerTable.Instance.Critical)) // ġ��Ÿ �����̶��
            {
                monster.MonsterDamaged(PlayerTable.Instance.NowAtk * SkillPercentage * CriMultiple);
                BattleManager.Instance.BattleDialogText.text += $"\n\nġ��Ÿ!! {Name} ��ų ����! \n{PlayerTable.Instance.NowAtk * SkillPercentage * CriMultiple} ��ŭ ���ظ� ������!";
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, CriDamageAmount, BattleManager.Instance.SkillCount);
            }
            else
            {
                monster.MonsterDamaged(PlayerTable.Instance.NowAtk * SkillPercentage);
                BattleManager.Instance.BattleDialogText.text += $"\n\n{Name}! \n{PlayerTable.Instance.NowAtk * SkillPercentage} ��ŭ ���ظ� ������!";
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, DamageAmount, BattleManager.Instance.SkillCount);
            }
            monster.nowMonsterStunStack += 1; // ���� ���� �߰�
            BattleManager.Instance.BattleDialogText.text += "\n���� �������״�!";
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
        BattleManager.Instance.BattleDialogText.text += $"\n\n{Name}! ({SkillPercentage * PlayerTable.Instance.Defense}) ���� �����!";
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
            BattleManager.Instance.BattleDialogText.text += "\n\n������ ��������!";
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
            BattleManager.Instance.BattleDialogText.text += $"\n\n������ ��������!";
            monster.monsterAni.SetTrigger("IsDodge");
        }
        else
        {            
            player.playerAni.SetTrigger(SkillTrigger);
            if (BattleManager.Instance.CriAttack(PlayerTable.Instance.Critical)) // ġ��Ÿ �����̶��
            {
                monster.MonsterDamaged(CriDamageAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\nġ��Ÿ!! {Name} ��ų ����! \n{CriDamageAmount} ��ŭ ���ظ� ������!";
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, CriDamageAmount, BattleManager.Instance.SkillCount);
            }
            else
            {
                monster.MonsterDamaged(DamageAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n{Name}! \n{DamageAmount} ��ŭ ���ظ� ������!";
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, DamageAmount, BattleManager.Instance.SkillCount);
            }
        }
    }
}

// ���� ��ų Ŭ����
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

// ���� ��� ��ų
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

// ���� �ü� ��ų
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
        if (BattleManager.Instance.DodgeSucess(PlayerTable.Instance.Dodge)) // �÷��̾ ȸ�� ���� ��
        {
            player.playerAni.SetTrigger("IsDodge");
            BattleManager.Instance.BattleDialogText.text += $"\n\n���� ������ ȸ���Ͽ���!";
            return;
        }
        else
        {
            BattleManager.Instance.BattleDialogText.text += $"\n\n {monster.nowMonsterName}�� {Name}!";
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

// ���� �ּ��� ��ų
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
        if (BattleManager.Instance.DodgeSucess(PlayerTable.Instance.Dodge)) // �÷��̾ ȸ�� ���� ��
        {
            player.playerAni.SetTrigger("IsDodge");
            BattleManager.Instance.BattleDialogText.text += $"\n\n���� ������ ȸ���Ͽ���!";
            return;
        }
        else
        {
            BattleManager.Instance.BattleDialogText.text += $"\n\n {monster.nowMonsterName}�� {Name}!";
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
            BattleManager.Instance.BattleDialogText.text += $"\n\n���� ������ ȸ���Ͽ���!";
        }
        else
        {
            monster.monsterAni.SetTrigger(SkillTrigger);
            if (BattleManager.Instance.CriAttack(monster.nowMonsterCri)) // ġ��Ÿ �����̶��
            {
                player.playerDamaged(BattleManager.Instance.CriAttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\nġ��Ÿ!! {monster.nowMonsterName}�� {Name}! \n{BattleManager.Instance.CriAttackAmount} ��ŭ ���ظ� �Ծ���! ({BattleManager.Instance.CriDefenseAmount})�����";
                BattleManager.Instance.FloatingText(BattleManager.Instance.PlayerDamageTextList, BattleManager.Instance.CriAttackAmount, BattleManager.Instance.SkillCount);
            }
            else
            {
                player.playerDamaged(BattleManager.Instance.AttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n{monster.nowMonsterName}�� {Name}! \n{BattleManager.Instance.AttackAmount} ��ŭ ���ظ� �Ծ���! ({BattleManager.Instance.DefenseAmount})�����";
                BattleManager.Instance.FloatingText(BattleManager.Instance.PlayerDamageTextList, BattleManager.Instance.AttackAmount, BattleManager.Instance.SkillCount);
            }
            PlayerTable.Instance.StunStack += 1; // ���� ���� �߰�
            if(PlayerTable.Instance.StunStack <= 0)
            {
                BattleManager.Instance.BattleDialogText.text += "\n�ұ��� ������ ������ �ߵ����!";
                return;
            }
            else
            {
                BattleManager.Instance.BattleDialogText.text += "\n���� ���ݿ� �����Ͽ���!";                
            }            
        }
    }
}




