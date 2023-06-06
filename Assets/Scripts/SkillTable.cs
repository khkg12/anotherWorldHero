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
        

    // �÷��̾� ��ų
    public Attack attack;
    public Defense defense;
    public StunAttack stunBoom;
    public DoubleAttack doubleAttack;
    public Baldo baldo;

    public void PlayerSingleAttackSkill(MonsterController monster, PlayerController player, BaseSkill skill, string SkillType)
    {        
        if (BattleManager.Instance.DodgeSucess(monster.nowMonsterDodge))
        {
            BattleManager.Instance.BattleDialogText.text += $"\n������ ��������!";
            monster.monsterAni.SetTrigger("IsDodge");
        }
        else
        {
            player.playerAni.SetTrigger("IsAttack");
            monster.startMonsterSingleDamaged(skill, SkillType);
        }
    }

    public void PlayerMultiAttackSkill(MonsterController monster, PlayerController player, BaseSkill skill, int SkillTimes, string SkillType) // ��Ÿ���ݽ�ų �Լ�
    {
        if (BattleManager.Instance.DodgeSucess(monster.nowMonsterDodge))
        {
            BattleManager.Instance.BattleDialogText.text += $"\n������ ��������!";
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
            BattleManager.Instance.BattleDialogText.text += $"\n������ ��������!";
            monster.monsterAni.SetTrigger("IsDodge");
        }
        else
        {
            player.playerAni.SetTrigger("IsAttack");
            monster.startMonsterSingleDamaged(skill, SkillType);
            monster.nowMonsterStunStack += 1; // ���� ���� �߰�
            BattleManager.Instance.BattleDialogText.text += "\n���� �������״�!";
        }
    } // ���� �������ݽ�ų �Լ�

    public void PlayerDotAttackSkill(MonsterController monster, PlayerController player, BaseSkill skill, string SkillType)
    {
        if (BattleManager.Instance.DodgeSucess(monster.nowMonsterDodge))
        {
            BattleManager.Instance.BattleDialogText.text += $"\n������ ��������!";
            monster.monsterAni.SetTrigger("IsDodge");
        }
        else
        {
            player.playerAni.SetTrigger("IsAttack");
            monster.startMonsterSingleDamaged(skill, SkillType);
            monster.nowMonsterStunStack += 1; // ���� ���� �߰�
            BattleManager.Instance.BattleDialogText.text += "\n���� �������״�!";
        }
    } // ���� �������ݽ�ų �Լ�

    // ���� ��Ÿ���ݽ�ų �Լ�, monster �� player�� battlaManager�� �Լ����࿡ skilloption�� ������ �� �Ű������� nowPlayer, nowMonster�� �޴´�
    public void MonsterSingleAttackSkill(MonsterController monster, PlayerController player, string Name, string SkillType) // string �迭�� ���������̱� ������ ref���, ������ �����߻� 
    {       
        if (BattleManager.Instance.DodgeSucess(PlayerTable.Instance.Dodge)) // �÷��̾ ȸ�� ���� ��
        {
            player.playerAni.SetTrigger("IsDodge");
            BattleManager.Instance.BattleDialogText.text += $"\n���� ������ ȸ���Ͽ���!";            
        }
        else
        {
            monster.monsterAni.SetTrigger("IsAttack");
            player.startPlayerSingleDamaged(Name, SkillType); // ��ų�̸��� �Ű������� �ְ� player�� �ڷ�ƾ�Լ� ����
        }
    }

    public void MonsterMultiAttackSkill(MonsterController monster, PlayerController player, string Name, int SkillTimes, string SkillType) // ���� ��Ÿ���ݽ�ų �Լ�
    {
        if (BattleManager.Instance.DodgeSucess(PlayerTable.Instance.Dodge)) // �÷��̾ ȸ�� ���� ��
        {
            player.playerAni.SetTrigger("IsDodge");
            BattleManager.Instance.BattleDialogText.text += $"\n���� ������ ȸ���Ͽ���!";
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
        if (BattleManager.Instance.DodgeSucess(PlayerTable.Instance.Dodge)) // �÷��̾ ȸ�� ���� ��
        {
            player.playerAni.SetTrigger("IsDodge");
            BattleManager.Instance.BattleDialogText.text += $"\n���� ������ ȸ���Ͽ���!";
        }
        else
        {
            monster.monsterAni.SetTrigger("IsAttack");
            player.startPlayerSingleDamaged(Name, SkillType); // ��ų�̸��� �Ű������� �ְ� player�� �ڷ�ƾ�Լ� ����
            PlayerTable.Instance.StunStack += 1; // ���� ���� �߰�
            if (PlayerTable.Instance.StunStack <= 0)
            {
                BattleManager.Instance.BattleDialogText.text += "\n�ұ��� ������ ������ �ߵ����!";
                return;
            }
            else
            {
                BattleManager.Instance.BattleDialogText.text += "\n���� ���ݿ� �����Ͽ���!";
            }
        }        
    } // ���� �������ݽ�ų �Լ�
}

// ���� ��ų Ŭ����
[System.Serializable]
public class MonsterSkill : ScriptableObject
{
    public string Name;
    public float SkillPercentage;
    public float CriMultiple;
    public string SkillText; // ���Ͱ� ���� ������ ���� �Ͻ��ϴ� �ؽ�Ʈ    
    public int SkillTimes;
    public string SkillType;
    public virtual void SkillOption(MonsterController monster, PlayerController player) { }
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
    public float CriMultiple;
    public Sprite SkillSprite;
    public string[] SkillText; // ��ų�� ���� ����    
    public int AvailableCount;
    public string SkillType;
    public int SkillTimes;
    // ����Ÿ�� : �ܼ�����, ��Ƽ����
    public virtual void SkillOption(MonsterController monster, PlayerController player) { }    
}

[System.Serializable]
public class Attack : BaseSkill
{    
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.PlayerSingleAttackSkill(monster, player, this, SkillType); // ��Ʋ�Ŵ������� monster�� nowmonster�� �ް� this�� �ڱ��ڽ�, �� ��ų
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
        BattleManager.Instance.BattleDialogText.text += $"\n\n{Name}! ({SkillPercentage * PlayerTable.Instance.Defense}) ���� �����!";
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






