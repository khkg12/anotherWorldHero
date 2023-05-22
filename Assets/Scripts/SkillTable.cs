using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Linq;
using System.Globalization;
using UnityEditor.Experimental.GraphView;
using UnityEditor;

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
            BattleManager.Instance.BattleDialogText.text += $"\n\n������ ��������!";
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
            BattleManager.Instance.BattleDialogText.text += $"\n\n������ ��������!";
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
            BattleManager.Instance.BattleDialogText.text += $"\n\n������ ��������!";
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


    // ���� ��Ÿ���ݽ�ų �Լ�, monster �� player�� battlaManager�� �Լ����࿡ skilloption�� ������ �� �Ű������� nowPlayer, nowMonster�� �޴´�
    public void MonsterSingleAttackSkill(MonsterController monster, PlayerController player, string Name, string SkillType) // string �迭�� ���������̱� ������ ref���, ������ �����߻� 
    {       
        if (BattleManager.Instance.DodgeSucess(PlayerTable.Instance.Dodge)) // �÷��̾ ȸ�� ���� ��
        {
            player.playerAni.SetTrigger("IsDodge");
            BattleManager.Instance.BattleDialogText.text += $"\n\n���� ������ ȸ���Ͽ���!";            
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
            BattleManager.Instance.BattleDialogText.text += $"\n\n���� ������ ȸ���Ͽ���!";
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
            BattleManager.Instance.BattleDialogText.text += $"\n\n���� ������ ȸ���Ͽ���!";
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
public class MonsterSkill
{
    public string Name;
    public float SkillPercentage;
    public float CriMultiple;
    public string SkillText; // ���Ͱ� ���� ������ ���� �Ͻ��ϴ� �ؽ�Ʈ    
    public virtual void SkillOption(MonsterController monster, PlayerController player) { }
}

// ���� ��� ��ų
[System.Serializable]
public class DemonSlayerSlash : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.MonsterSingleAttackSkill(monster, player, Name, "Physical");
    }
}
[System.Serializable]
public class DemonSlayerStabbing : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.MonsterSingleAttackSkill(monster, player, Name, "Physical");
    }
}
[System.Serializable]
public class DemonSlayerSwordsmanship : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.MonsterSingleAttackSkill(monster, player, Name, "Physical");
    }
}

// ���� �ü� ��ų
[System.Serializable]
public class DemonArcherArrowShot : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.MonsterSingleAttackSkill(monster, player, Name, "Physical");
    }
}
[System.Serializable]
public class DemonArcherDoubleShot : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.MonsterMultiAttackSkill(monster, player, Name, 2, "Physical"); // ���� �ַο� 2�������̹Ƿ� 2�Ű������־���
    }
}
[System.Serializable]
public class DemonArcherDarkArrow : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.MonsterSingleAttackSkill(monster, player, Name, "Physical");
    }
}

// ���� �ּ��� ��ų
[System.Serializable]
public class DemonShamanEnergyBolt : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.MonsterSingleAttackSkill(monster, player, Name, "Magic");
    }
}
[System.Serializable]
public class DemonShamanDarkLightning : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.MonsterMultiAttackSkill(monster, player, Name, 3, "Magic");
    }
}
[System.Serializable]
public class DemonShamanStunBall : MonsterSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.MonsterStunAttackSkill(monster, player, Name, "Magic");
    }
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
        SkillTable.Instance.PlayerSingleAttackSkill(monster, player, this, "Physical"); // ��Ʋ�Ŵ������� monster�� nowmonster�� �ް� this�� �ڱ��ڽ�, �� ��ų
    }    
}

[System.Serializable]
public class StunAttack : BaseSkill
{    
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.PlayerStunAttackSkill(monster, player, this, "Magic");
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
        SkillTable.Instance.PlayerMultiAttackSkill(monster, player, this, 2, "Pysical");
    }
}

[System.Serializable]
public class Baldo : BaseSkill
{
    public override void SkillOption(MonsterController monster, PlayerController player)
    {
        SkillTable.Instance.PlayerSingleAttackSkill(monster, player, this, "Physical");
    }
}






