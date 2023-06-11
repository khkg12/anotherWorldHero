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

    public void Initialize(MonsterClass Target, Animator characterAni) // 초기화 함수, Monster는 이 Character클래스와 같이 몬스터를 다룰 스크립트값을 만들고 가져오기
    {
        // hp는 유지되어야하므로 playertable의 hp에 직접할당, 아니면 몬스터가 죽고난뒤 값을 넣어주는것도 고려해볼것        
        atk = (int)PlayerTable.Instance.Atk;
        critical = PlayerTable.Instance.Critical;
        dodge = PlayerTable.Instance.Dodge;
        defense = (int)PlayerTable.Instance.Defense;
        skillList = PlayerTable.Instance.playerSkillList;
        this.target = Target;
        ani = characterAni;
    }

    public void UseSkill(int index) // 공격스킬과 버프스킬의 차이점을 두어야함 -> 공격스킬의 경우 몬스터회피에 따라 실패, 버프는 무조건 성공
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
        }; //주는쪽에서 정보를 모두 취합

        switch (skill.type)
        {
            case Type.Attack: // 스킬이 공격스킬이면 몬스터의 회피에 따라 실행조건을 넣어줘야함
                Debug.Log("몬스터회피율" + target.nowMonsterDodge);
                if (bm.Instance.DodgeSucess(target.nowMonsterDodge)) // 타겟, 즉 몬스터가 회피에 성공했다면
                {
                    target.monsterAni.SetTrigger("IsDodge");
                    battleDialogController.PlayerAddText(BattleType.Dodge, skillData);
                }
                else
                {                    
                    ani.SetTrigger("IsAttack"); // 공격스킬 애니 실행
                    skill.SkillUse(target); // 버튼클릭 -> useSkill -> 처리 후 몬스터체력 0이면 종료 -> 아니라면 monsterSkill                    
                }
                break;
            case Type.Buff:
                skill.SkillUse(target);
                break;
        }

        // 몬스터 스킬함수 실행
        

        // 모든게 끝났을 때 버튼 활성화
        

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
        if (nowMonsterHp <= 0 && IsMonsterBoss == false) // 승리하였을 때 전투가 끝나므로 사실상 전투가 끝나고 실행될 코드들 집어넣음
        {
            await Task.Delay(200);
            GameManager.Instance.IsMonsterDead = true; // 몬스터의 체력이 0, 즉 죽으면 플래그 true
            GameManager.Instance.IsAni = true;
            GameManager.Instance.LoadMainScene();
            await Task.Delay(100);
            UiManager.Instance.ItemSelectUI.gameObject.SetActive(true);
        }
        else if (nowMonsterHp <= 0 && IsMonsterBoss == true)
        {
            // 몬스터 사망 애니
            await Task.Delay(500);
            GameManager.Instance.AfterVictory();
            // nextDialog를 실행시켜야함 어떻게실행시킬지?
        }
    }

    public IEnumerator MonsterDamaged(BaseSkill Skill, int SkillTimes, string SkillType)
    {
        for (int i = 0; i < SkillTimes; i++)
        {
            // 스킬이펙트 실행
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

            if (BattleManager.Instance.CriAttack(BattleManager.Instance.nowmonster.nowMonsterCri)) // 치명타 공격이라면
            {
                MonsterDamaged(BattleManager.Instance.PlayerCriAttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n치명타!! {Skill.Name}! {BattleManager.Instance.PlayerCriAttackAmount} 피해입힘!";
                BattleManager.Instance.FloatingText(BattleManager.Instance.MonsterDamageTextList, BattleManager.Instance.PlayerCriAttackAmount, BattleManager.Instance.SkillCount);
            }
            else
            {
                MonsterDamaged(BattleManager.Instance.PlayerAttackAmount);
                BattleManager.Instance.BattleDialogText.text += $"\n\n{Skill.Name}! {BattleManager.Instance.PlayerAttackAmount} 피해입힘!";
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
