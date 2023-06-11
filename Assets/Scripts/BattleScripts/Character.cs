using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
    private Animator ani;
    public int atk;
    public int critical;
    public int dodge;
    private int defense;    
    private BattleDialogController battleDialogController;    
    public List<BaseSkill> skillList;    

    public void Initialize(MonsterClass Target, Animator characterAni) // �ʱ�ȭ �Լ�, Monster�� �� CharacterŬ������ ���� ���͸� �ٷ� ��ũ��Ʈ���� ����� ��������
    {
        // hp�� �����Ǿ���ϹǷ� playertable�� hp�� �����Ҵ�, �ƴϸ� ���Ͱ� �װ��� ���� �־��ִ°͵� ����غ���        
        atk = (int)PlayerTable.Instance.Atk;
        critical = PlayerTable.Instance.Critical;
        dodge = PlayerTable.Instance.Dodge;
        defense = (int)PlayerTable.Instance.Defense;
        skillList = PlayerTable.Instance.playerSkillList;
        this.target = Target;
        ani = characterAni;
    }

    public void UseSkill(int index) // ���ݽ�ų�� ������ų�� �������� �ξ���� -> ���ݽ�ų�� ��� ����ȸ�ǿ� ���� ����, ������ ������ ����
    {
        Debug.Log(index);
        BaseSkill skill = skillList[index];
        Debug.Log(skill);
        var skillData = new SkillData() 
        {
            /*
            SkillName = skill.Name,
            Atk = (int)(atk * skill.SkillPercentage),
            */
        }; //�ִ��ʿ��� ������ ��� ����

        switch (skill.type)
        {
            case Type.Attack: // ��ų�� ���ݽ�ų�̸� ������ ȸ�ǿ� ���� ���������� �־������
                Debug.Log("����ȸ����" + target.nowMonsterDodge);
                if (bm.Instance.DodgeSucess(target.nowMonsterDodge)) // Ÿ��, �� ���Ͱ� ȸ�ǿ� �����ߴٸ�
                {
                    target.monsterAni.SetTrigger("IsDodge");
                    battleDialogController.PlayerAddText(BattleType.Dodge, skillData);
                }
                else
                {                    
                    ani.SetTrigger("IsAttack"); // ���ݽ�ų �ִ� ����
                    skill.SkillUse(target); // ��ưŬ�� -> useSkill -> ó�� �� ����ü�� 0�̸� ���� -> �ƴ϶�� monsterSkill                    
                }
                break;
            case Type.Buff:
                skill.SkillUse(target);
                break;
        }

        // ���� ��ų�Լ� ����
        

        // ���� ������ �� ��ư Ȱ��ȭ
        

        /*int atk = skill.Use();
        var atkData = new AttackData() { attackerName = Name, atk = atk, skillName = skill.GetName() };
        target.TakeDamage(atkData);
        scriptManager.AddText("blabla");*/
    }

    /*
    public async void Damaged(float DamageAmount)
    {
        target.noWmONSTER -= DamageAmount;
        monsterAni.SetTrigger("IsHit");
        if (nowMonsterHp <= 0 && IsMonsterBoss == false) // �¸��Ͽ��� �� ������ �����Ƿ� ��ǻ� ������ ������ ����� �ڵ�� �������
        {
            await Task.Delay(200);
            GameManager.Instance.IsMonsterDead = true; // ������ ü���� 0, �� ������ �÷��� true
            GameManager.Instance.IsAni = true;
            GameManager.Instance.LoadMainScene();
            await Task.Delay(100);
            UiManager.Instance.ItemSelectUI.gameObject.SetActive(true);
        }
        else if (nowMonsterHp <= 0 && IsMonsterBoss == true)
        {
            // ���� ��� �ִ�
            await Task.Delay(500);
            GameManager.Instance.AfterVictory();
            // nextDialog�� ������Ѿ��� ��Խ����ų��?
        }
    }

    public IEnumerator MonsterDamaged(BaseSkill Skill, int SkillTimes, string SkillType)
    {
        for (int i = 0; i < SkillTimes; i++)
        {
            // ��ų����Ʈ ����
            if (SkillType == "Physical")
            {
                BattleManager.Instance.MonsterPhysicalHitEffectList[i].gameObject.SetActive(false);
                BattleManager.Instance.MonsterPhysicalHitEffectList[i].gameObject.SetActive(true);
            }
            else
            {
                BattleManager.Instance.MonsterMagicHitEffectList[i].gameObject.SetActive(false);
                BattleManager.Instance.MonsterMagicHitEffectList[i].gameObject.SetActive(true);
            }

            if (BattleManager.Instance.CriAttack(BattleManager.Instance.nowmonster.nowMonsterCri)) // ġ��Ÿ �����̶��
            {
                MonsterDamaged(BattleManager.Instance.PlayerCriAttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\nġ��Ÿ!! {Skill.Name}! {BattleManager.Instance.PlayerCriAttackAmount} ��������!";
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, BattleManager.Instance.PlayerCriAttackAmount, BattleManager.Instance.SkillCount);
            }
            else
            {
                MonsterDamaged(BattleManager.Instance.PlayerAttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n{Skill.Name}! {BattleManager.Instance.PlayerAttackAmount} ��������!";
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, BattleManager.Instance.PlayerAttackAmount, BattleManager.Instance.SkillCount);
            }
            BattleManager.Instance.SkillCount += 1;
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void startMonsterDamaged(BaseSkill Skill, string SkillType)
    {
        StartCoroutine(MonsterDamaged(Skill, SkillType));
    }




    /*public void TakeDamage(AttackData atk)
    {
        hp -= atk.atk;
        scriptManager.AddText(BattleType.TakeDamage, atk);
    }*/

}
