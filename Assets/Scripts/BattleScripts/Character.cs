using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

public class SkillData
{
    public string Name;
    public int Damage;
    public int CriDamage;
    public int Defense;
    public int CriDenfense;
}

public class Character : MonoBehaviour
{
    private MonsterClass target;
    public Animator characterAni;
    private int atk;
    private int critical;
    private int dodge;
    private int defense;

    private bool MonsterStun;
    [SerializeField] private BattleDialogController battleDialogController;
    public List<BaseSkill> skillList;
    public int DefenseAmount;
    public int StunStack;

    public void Initialize(MonsterClass Target) // �ʱ�ȭ �Լ�, Monster�� �� CharacterŬ������ ���� ���͸� �ٷ� ��ũ��Ʈ���� ����� ��������
    {
        // hp�� �����Ǿ���ϹǷ� playertable�� hp�� �����Ҵ�, �ƴϸ� ���Ͱ� �װ��� ���� �־��ִ°͵� ����غ���        
        atk = (int)PlayerTable.Instance.Atk;
        critical = PlayerTable.Instance.Critical;
        dodge = PlayerTable.Instance.Dodge;
        defense = (int)PlayerTable.Instance.Defense;
        skillList = PlayerTable.Instance.playerSkillList;
        this.target = Target;        
    }    

    public IEnumerator UseSkill(int index)
    {        
        BaseSkill skill = skillList[index];
        var skillData = new SkillData()
        {            
            Name = skill.Name,                        
        }; //�ִ��ʿ��� ������ ��� ����

        switch (skill.type)
        {
            case Type.Attack: // ��ų�� ���ݽ�ų�̸� ������ ȸ�ǿ� ���� ���������� �־������                
                    if (target.Dodge()) // Ÿ��, �� ���Ͱ� ȸ�ǿ� �����ߴٸ�
                    {
                        target.monsterAni.SetTrigger("IsDodge");
                        battleDialogController.PlayerAddText(BattleType.Dodge, skillData);                        
                }
                    else
                    {
                        characterAni.SetTrigger("IsAttack"); // ���ݽ�ų �ִ� ����
                        skill.SkillUse(this); // ��ưŬ�� -> useSkill -> ó�� �� ����ü�� 0�̸� ���� -> �ƴ϶�� monsterSkill                    
                    }
                break;
            case Type.Defense:
                characterAni.SetTrigger("IsDefense"); // ��ų �ִ� ����
                skill.SkillUse(this);
                break;
            case Type.Buff:
                // characterAni.SetTrigger("IsDefense"); ������ų �ִ� ����
                skill.SkillUse(this);
                break;
        }
        yield return new WaitForSeconds(0.8f);     
        if(MonsterStun == true)
        {
            MonsterStun = false;
            pc.BtnEnableAction(); // ��ưȰ��ȭ
        }
        else
        {
            target.UseSkill(); // ���� ��ų�Լ� ����                            
        }        
        yield break;
    }

    public async void Damaged(float DamageAmount)
    {
        PlayerTable.Instance.Hp -= DamageAmount;
        characterAni.SetTrigger("IsHit");
        if (PlayerTable.Instance.Hp <= 0)
        {
            await Task.Delay(100);
            BattleManager.Instance.ResurrectionUI.gameObject.SetActive(true);
        }
    }

    public IEnumerator TakeDamaged(BaseSkill Skill)
    {        
        int SkillCount = 0;
        var skillData = new SkillData() // ��ų�� �޴� �ʿ��� ����������, �ʿ��Ѱ͸� ����
        {
            Name = Skill.Name,
            Damage = (int)(atk * Skill.SkillPercentage),
            CriDamage = (int)(atk * Skill.SkillPercentage * Skill.CriMultiple)
        };        
        for (int i = 0; i < Skill.SkillTimes; i++)
        {
            // ��ų����Ʈ ����
            if (Skill.SkillType == "Physical")
            {
                bm.Instance.MonsterPhysicalHitEffectList[i].gameObject.SetActive(false);
                bm.Instance.MonsterPhysicalHitEffectList[i].gameObject.SetActive(true);
            }
            else
            {
                bm.Instance.MonsterMagicHitEffectList[i].gameObject.SetActive(false);
                bm.Instance.MonsterMagicHitEffectList[i].gameObject.SetActive(true);
            }
            if (bm.Instance.CriAttack(critical)) // ġ��Ÿ �����̶��
            {
                target.Damaged(skillData.CriDamage);
                battleDialogController.PlayerAddText(BattleType.CriAttack, skillData);                
                bm.Instance.FloatingText(bm.Instance.MonsterDamageTextList, skillData.CriDamage, SkillCount);
            }
            else
            {                
                target.Damaged(skillData.Damage);
                battleDialogController.PlayerAddText(BattleType.Attack, skillData);                
                bm.Instance.FloatingText(bm.Instance.MonsterDamageTextList, skillData.Damage, SkillCount);
            }
            SkillCount += 1;
            yield return new WaitForSeconds(0.2f);
        }

        if (Skill.StunCount > 0) // ��ų�� ����ȿ���� �ִ� ��ų�̶��
        {
            target.StunStack += 1; // Ÿ���� ���������� �ϳ� �ø���
            if (target.StunStack <= 0) // Ÿ���� ���������� 0���϶�� ���������� �ʾҴٴ� ��
            {
                battleDialogController.PlayerAddText(BattleType.EndureStun, skillData); // ������ �ߵ� �ؽ�Ʈ �߰�
            }
            else
            {
                battleDialogController.PlayerAddText(BattleType.GetStun, skillData); // ���� ����
                yield return new WaitForSeconds(0.5f);
                MonsterStun = true; // ���� ���� �÷����Լ�
                target.SelectMonsterSkill(); // ������ ������ų ���ϰ�
                battleDialogController.ActionAddText(target.monsterSkill); // ����                
                yield break;
            }
        }

        yield return null;
    }

    public void startTakeDamaged(BaseSkill Skill)
    {        
        StartCoroutine(TakeDamaged(Skill));
    }

    public bool Dodge()
    {
        return bm.Instance.DodgeSucess(dodge);
    }    
}
